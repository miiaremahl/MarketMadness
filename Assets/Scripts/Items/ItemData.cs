using UnityEngine;

/*
 * Itemdata.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 9.1.2021
 * 
 * Refs:
 * 1.Brackeys - ITEMS - Making an RPG in Unity (E04) ,https://www.youtube.com/watch?v=HQNl3Ff2Lpo&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=5&t=21s, inspiration on how to do the class
 * 
 */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory")]
public class ItemData : ScriptableObject
{
    //name
    new public string type = "Name";

    //sprite image for the item
    public Sprite image;

    //is item small or big
    public string itemSize;

}
