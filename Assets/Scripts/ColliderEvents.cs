using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderEvents : MonoBehaviour
{

    [HideInInspector] public new Collider2D collider;

    public event Action<Collision2D> OnEnterCollision;
    public event Action<Collision2D> OnStayCollision;
    public event Action<Collision2D> OnExitCollision;

    public event Action<Collider2D> OnEnterTrigger;
    public event Action<Collider2D> OnStayTrigger;
    public event Action<Collider2D> OnExitTrigger;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnEnterCollision?.Invoke(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        OnStayCollision?.Invoke(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        OnExitCollision?.Invoke(col);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        OnEnterTrigger?.Invoke(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        OnStayTrigger?.Invoke(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        OnExitTrigger?.Invoke(col);
    }
}
