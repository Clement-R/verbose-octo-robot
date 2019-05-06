using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class PickableItem : MonoBehaviour
{
    private void Awake()
    {
        if (!GetComponent<BoxCollider2D>().isTrigger)
            Debug.LogWarning("Pickable item box collider should be a trigger");
    }

    protected abstract void OnEnter(Collider2D p_collision);
    protected abstract void OnStay(Collider2D p_collision);
    protected abstract void OnExit(Collider2D p_collision);

    private void OnTriggerEnter2D(Collider2D p_collision)
    {
        OnEnter(p_collision);
    }

    private void OnTriggerExit2D(Collider2D p_collision)
    {
        OnStay(p_collision);
    }

    private void OnTriggerStay2D(Collider2D p_collision)
    {
        OnExit(p_collision);
    }
}
