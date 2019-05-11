using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCombat : BasicCombat
{
    public override void Attack()
    {
        if (CanAttack)
        {
            LastAttack = Time.time;

            float direction = m_characterController.FacingDirection == EDirection.LEFT ? -1f : 1f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DORotate(new Vector3(0f, 0f, direction * -45f), 0.5f));
            sequence.Append(transform.DORotate(new Vector3(0f, 0f, direction * 65f), 0.1f));
            sequence.AppendCallback(() => { IsAttackActive = true; });
            sequence.AppendCallback(CheckAttackCollisions);
            sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() => { IsAttackActive = false; });
            sequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0.25f));
            sequence.Play();
        }
    }
}
