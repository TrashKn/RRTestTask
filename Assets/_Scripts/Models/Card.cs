using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Card", order = 2)]
public class Card : ScriptableObject
{
    [SerializeField] public string Title;
    [SerializeField] public string Description;
    [SerializeField] public int ManaValue;
    [SerializeField] public int AttackValue;
    [SerializeField] public int HpValue;
}
