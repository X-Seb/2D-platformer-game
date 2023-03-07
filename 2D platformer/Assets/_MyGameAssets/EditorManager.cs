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
}