using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : BasicCombat
{
    public WeaponHolder Weapon;

    [SerializeField] protected TrailRenderer m_trail = null;

    protected override void Start()
    {
        base.Start();
        Weapon.OnWeaponChange += UpdateWeapon;
    }

    private void UpdateWeapon(WeaponData p_w)
    {
        m_damage = p_w.attackDamage;
    }

    public override void Attack()
    {
        if (Weapon.WeaponData != null)
        {
            if (CanAttack)
            {
                LastAttack = Time.time;

                float direction = m_characterController.FacingDirection == EDirection.LEFT ? -1f : 1f;

                // TODO: Change sequence depending on weapon
                Sequence sequence = DOTween.Sequence();
                sequence.Append(Weapon.transform.DORotate(new Vector3(0f, 0f, direction * 25f), Weapon.WeaponData.attackAnticipation));
                sequence.AppendCallback(() => { m_trail.gameObject.SetActive(true); });
                sequence.Append(Weapon.transform.DORotate(new Vector3(0f, 0f, direction * -90f), Weapon.WeaponData.attackStrike));
                sequence.AppendCallback(() => { IsAttackActive = true; });
                sequence.AppendCallback(CheckAttackCollisions);
                sequence.AppendInterval(Weapon.WeaponData.attackWait);
                sequence.AppendCallback(() => { IsAttackActive = false; });
                sequence.AppendCallback(() => { m_trail.gameObject.SetActive(false); });
                sequence.Append(Weapon.transform.DORotate(new Vector3(0f, 0f, 0f), Weapon.WeaponData.attackRecovery));
                sequence.Play();
            }
        }
        else
        {
            Debug.Log("No weapon equipped. Cannot attack");
        }
    }
}
