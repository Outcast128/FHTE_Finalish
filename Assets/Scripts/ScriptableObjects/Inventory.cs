using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;
    public int numberOfHearts;
    public int numberOfSwords;
    public int numberOfDash;
    public int numberOfBoots;
    public int numberOfArrows;


    public void AddItem(Item itemToAdd)
    {
        // Check ig the item is a key
        if (itemToAdd.isKey)
        {
            numberOfKeys++;
        }
        else if (itemToAdd.isHeart)
        {
            numberOfHearts++;
        }
        else if (itemToAdd.isSword)
        {
            numberOfSwords++;
        }
        else if (itemToAdd.isDash)
        {
            numberOfDash++;
        }
        else if (itemToAdd.isBoots)
        {
            numberOfBoots++;
        }
        else if (itemToAdd.isArrow)
        {
            numberOfArrows++;
        }
        else
        {
            if (!items.Contains(itemToAdd))
            {
                items.Add(itemToAdd);
            }
        }
    }
}
