using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorManager : Editor
{
    [MenuItem("Tools/Clear all PlayerPrefs")]
    public static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Clear relics and keys")]
    public static void DelelteRelicsAndKey()
    {
        PlayerPrefs.DeleteKey("Relic_1_Collected");
        PlayerPrefs.DeleteKey("Relic_2_Collected");
        PlayerPrefs.DeleteKey("Relic_3_Collected");
        PlayerPrefs.DeleteKey("Relic_4_Collected");
        PlayerPrefs.DeleteKey("Relic_5_Collected");
        PlayerPrefs.DeleteKey("Relic_6_Collected");
        PlayerPrefs.DeleteKey("Key_1_Collected");
        PlayerPrefs.DeleteKey("Key_2_Collected");
        PlayerPrefs.DeleteKey("Key_3_Collected");
        PlayerPrefs.DeleteKey("Key_4_Collected");
        PlayerPrefs.DeleteKey("Key_5_Collected");
        PlayerPrefs.DeleteKey("Key_6_Collected");
    }
}