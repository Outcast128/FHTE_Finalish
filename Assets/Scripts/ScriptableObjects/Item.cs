using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public string itemDescription;
    public bool isKey;
    public bool isHeart;
    public bool isSword;
    public bool isBoots;
    public bool isDash;
    public bool isArrow;

}
