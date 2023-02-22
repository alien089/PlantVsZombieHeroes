using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cards Data", menuName = "Cards Data")]
public class Card : ScriptableObject
{
    public GameObject PhysicBody;
    public GameObject UIBody;
    public string CardName;
    public int HealthPoints;
    public int AttackPoints;
    public string Effect;
    public int CardID;
}
