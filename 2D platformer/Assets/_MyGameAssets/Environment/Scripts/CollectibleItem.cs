using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItem")]
public class CollectibleItem : ScriptableObject
{
    public enum itemCategory
    {
        relic,
        potion,
        key
    }

    [Header("General ")]
    public string itemName;
    public itemCategory itemType;
    public AudioClip collectedSFX;
    [Header("Potion specific info: ")]
    public string abilityName;
    [Header("Relic specific info: ")]
    public int relicID;
    [Header("Key specific info: ")]
    public int keyID;
    [Header("Text information: ")]
    public string topText;
    public string loreText;
    public string descriptionText;
}