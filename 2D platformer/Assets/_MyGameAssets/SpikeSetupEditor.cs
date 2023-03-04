using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpikeSetup))]
public class SpikeSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpikeSetup script = (SpikeSetup)target;
        if (GUILayout.Button("Ajust size"))
        {
            script.AdjustSize();
        }

    }
}