using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EDirection
{
    LEFT = 0,
    RIGHT = 1
}

public class PlayerController : CharacterController
{
    [Header("Keys")]
    [SerializeField] private KeyCode m_upKey;
    [SerializeField] private KeyCode m_downKey;
    [SerializeField] private KeyCode m_leftKey;
    [SerializeField] private KeyCode m_rightKey;
    [SerializeField] protected KeyCode m_dashKey;
    [SerializeField] protected KeyCode m_jumpKey;
    [SerializeField] protected KeyCode m_attackKey;

    [Header("Debug")]
    [SerializeField] private bool m_debug = false;

    private Animator m_animator;
    private HealthBehaviour m_healthSystem;

    protected override void Start()
    {
        base.Start();
        m_animator = GetComponent<Animator>();
        m_healthSystem = GetComponent<HealthBehaviour>();

        m_healthSystem.OnHit += GetAttacked;
    }

    private void GetAttacked(GameObject p_attacker, int p_damages)
    {
        Debug.Log("Attacked!");

        Vector2 direction = new Vector2(0f, 0f);
        if (p_attacker.transform.position.x > transform.position.x)
            direction.x = -1f;
        else
            direction.x = 1f;

        if (m_healthSystem.Life > 0)
            StartCoroutine(_HitEffect(direction));
    }

    private IEnumerator _HitEffect(Vector2 p_direction)
    {
        var material = GetComponent<SpriteRenderer>().material;

        material.SetColor("_BlinkColor", Color.white);
        material.SetFloat("_BlinkAmount", 1f);
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }

        material.SetColor("_BlinkColor", Color.red);
        material.SetFloat("_BlinkAmount", 0.5f);
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        material.SetFloat("_BlinkAmount", 0f);
    }

    void Update()
    {
        Direction = Vector2.zero;

        if(!m_isDashing)
        {
            if (Input.GetKey(m_upKey))
                Direction += new Vector2(0f, 1f);
            else if (Input.GetKey(m_downKey))
                Direction += new Vector2(0f, -1f);

            if (Input.GetKey(m_leftKey))
            {
                FacingDirection = EDirection.LEFT;
                Direction += new Vector2(-1f, 0f);
            }
            else if (Input.GetKey(m_rightKey))
            {
                FacingDirection = EDirection.RIGHT;
                Direction += new Vector2(1f, 0f);
            }

            m_animator.SetFloat("hSpeed", Mathf.Abs(Direction.x));
            m_animator.SetFloat("vSpeed", Mathf.Abs(Direction.y));
        }

        if (Input.GetKeyDown(m_jumpKey))
        {
            m_jump = true;
            m_animator.SetBool("jump", true);
        }

        if (Input.GetKeyDown(m_attackKey))
            Attack();

        // Attack debug
        if(IsAttacking)
        {
            float direction = FacingDirection == EDirection.LEFT ? -1f : 1f;
            Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                               transform.position.y + m_sr.size.y / 2f);
            VisualDebug.DrawCross(attackOrigin, 0.15f, Color.green);
            Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
            VisualDebug.DrawBox(boxOrigin, new Vector2(m_attackX, m_attackY), Color.cyan);
        }

        if (FacingDirection == EDirection.LEFT && transform.rotation.y != 180f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (FacingDirection == EDirection.RIGHT && transform.rotation.y != 0f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        if (Input.GetKey(m_dashKey) && !m_isDashing && (Time.time >= m_lastDash + m_dashCooldown))
        {
            m_lastDash = Time.time;
            m_isDashing = true;
        }

        if (m_debug)
        {
            Debug.DrawRay(transform.position, transform.right * 5f, Color.blue);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(m_jump)
        {
            m_animator.SetBool("jump", false);
        }
    }

    //! Move this part to a weapon part or something ? Let it do it's own
    //! animations and collision things

    private void Attack()
    {
        if (m_weapon.weaponData != null)
        {
            if (Time.time > LastAttack + m_attackCooldown)
            {
                LastAttack = Time.time;

                float direction = FacingDirection == EDirection.LEFT ? -1f : 1f;

                // TODO: Externalize this to a Weapon script ?
                Sequence sequence = DOTween.Sequence();
                sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, direction * 25f), m_weapon.weaponData.attackAnticipation));
                sequence.AppendCallback(() => { m_trail.gameObject.SetActive(true); });
                sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, direction * -90f), m_weapon.weaponData.attackStrike));
                sequence.AppendCallback(() => { IsAttackActive = true; });
                sequence.AppendCallback(CheckAttackCollisions);
                sequence.AppendInterval(m_weapon.weaponData.attackWait);
                sequence.AppendCallback(() => { IsAttackActive = false; });
                sequence.AppendCallback(() => { m_trail.gameObject.SetActive(false); });
                sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, 0f), m_weapon.weaponData.attackRecovery));
                sequence.Play();
            }
        } else
        {
            Debug.Log("No weapon equipped. Cannot attack");
        }
    }

    private void CheckAttackCollisions()
    {
        float direction = FacingDirection == EDirection.LEFT ? -1f : 1f;
        Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                           transform.position.y + m_sr.size.y / 2f);

        Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxOrigin, new Vector2(m_attackX, m_attackY), 0f, LayerMask.GetMask("HitBox"));

        for (int i = 0; i < collisions.Length; i++)
        {
            HealthBehaviour hittable = collisions[i].GetComponentInParent<HealthBehaviour>();
            if (hittable != null && hittable.gameObject != gameObject)
            {
                hittable.Hit(gameObject, m_weapon.weaponData.attackDamage);
            }
        }
    }
}
