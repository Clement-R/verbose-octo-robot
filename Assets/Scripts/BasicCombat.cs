using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombat : MonoBehaviour
{
    [SerializeField] private KeyCode c_attack;

    private PlayerController player;

    private Animation c_swipe;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponent<PlayerController>();
        c_swipe = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(c_attack))
        {
            c_swipe.Play();
        }

        if (player.FacingDirection == EDirection.LEFT)
        {
            transform.localScale = new Vector3(.5f, -.5f, -.5f);
        } else
        {
            transform.localScale = new Vector3(.5f, .5f, .5f);
        }

    }
}
