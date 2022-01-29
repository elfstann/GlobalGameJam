using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        CustomButton button = (CustomButton)target;

        //button.Overlay = (GameObject)EditorGUILayout.ObjectField("Overlay:", targetMyButton.Overlay, typeof(GameObject), true);

        base.OnInspectorGUI();
    }
}
