using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSword", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    // Antoine : Based on Brackey tutorial. I have no idea of what I'm doing now

    public new string name;
    public string description;

    public Sprite handle;
    public Sprite guard;
    public Sprite blade;

    public int attackDamage = 100;

    public float attackAnticipation = .1f;
    public float attackStrike = .1f;
    public float attackWait = .1f;
    public float attackRecovery = .05f;

}
