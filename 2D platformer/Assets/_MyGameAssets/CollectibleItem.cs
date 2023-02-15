using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItem")]
public class CollectibleItem : ScriptableObject
{
    public enum itemCategory
    {
        relic,
        potion
    }
    public string itemName;
    public itemCategory itemType;

    public string topText;
    public string loreText;
    public string descriptionText;
}