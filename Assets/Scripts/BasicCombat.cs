using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BasicCombat : MonoBehaviour
{
    public bool CanAttack { get { return Time.time > LastAttack + m_attackCooldown; } }
    public float LastAttack;
    public bool IsAttacking { get { return Time.time < LastAttack + m_attackCooldown; } }

    [Header("Combat")]
    public string TargetTag = string.Empty;
    public bool IsAttackActive = false;
    
    [SerializeField] protected float m_attackCooldown = 0.5f;
    [SerializeField] protected float m_attackX = 1f;
    [SerializeField] protected float m_attackY = 0.25f;
    [SerializeField] protected int m_damage = 25;

    protected CharacterController m_characterController;
    protected SpriteRenderer m_sr;

    protected virtual void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_sr = GetComponentInChildren<SpriteRenderer>();
    }

    public abstract void Attack();

    private void Update()
    {
        // Attack debug
        if (IsAttacking)
        {
            float direction = m_characterController.FacingDirection == EDirection.LEFT ? -1f : 1f;
            Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                               transform.position.y + m_sr.size.y / 2f);
            VisualDebug.DrawCross(attackOrigin, 0.15f, Color.green);
            Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
            VisualDebug.DrawBox(boxOrigin, new Vector2(m_attackX, m_attackY), Color.cyan);
        }
    }

    protected virtual void CheckAttackCollisions()
    {
        float direction = m_characterController.FacingDirection == EDirection.LEFT ? -1f : 1f;
        Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                           transform.position.y + m_sr.size.y / 2f);

        Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxOrigin, new Vector2(m_attackX, m_attackY), 0f, LayerMask.GetMask("HitBox"));

        Debug.Log("CheckAttackCollisions : " + collisions.Length);

        for (int i = 0; i < collisions.Length; i++)
        {
            Debug.Log("CheckAttackCollisions : " + TargetTag);
            Debug.Log("CheckAttackCollisions : " + collisions[i].tag);

            if (collisions[i].CompareTag(TargetTag))
            {
                HealthBehaviour hittable = collisions[i].GetComponentInParent<HealthBehaviour>();
                if (hittable != null && hittable.gameObject != gameObject)
                {
                    hittable.Hit(gameObject, m_damage);
                }
            }
        }
        Debug.Log("CheckAttackCollisions : END");
    }
}
