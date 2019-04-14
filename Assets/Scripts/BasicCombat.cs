using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCombat : MonoBehaviour
{
    [SerializeField] private KeyCode c_attack;

    private Animation c_swipe;

    // Start is called before the first frame update
    void Start()
    {
        c_swipe = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(c_attack))
        {
            c_swipe.Play();
        }

    }
}
