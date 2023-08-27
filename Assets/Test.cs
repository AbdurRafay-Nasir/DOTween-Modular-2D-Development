using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    public float field1;
    public float field2;
    public float field3;
}


[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    private SerializedProperty field1Prop;
    private SerializedProperty field2Prop;
    private SerializedProperty field3Prop;

    private bool[] tabStates;

    private void OnEnable()
    {
        int numTabs = 3; // Change this to the number of tabs you want
        tabStates = new bool[numTabs];

        field1Prop = serializedObject.FindProperty("field1");
        field2Prop = serializedObject.FindProperty("field2");
        field3Prop = serializedObject.FindProperty("field3");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(10);

        DrawTabs();

        if (tabStates[0])
        {
            EditorGUILayout.PropertyField(field1Prop);
        }

        if (tabStates[1])
        {
            EditorGUILayout.PropertyField(field2Prop);
        }

        if (tabStates[2])
        {
            EditorGUILayout.PropertyField(field3Prop);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTabs()
    {
        GUILayout.BeginHorizontal();

        for (int i = 0; i < tabStates.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            bool newState = GUILayout.Toggle(tabStates[i], "Tab " + (i + 1), EditorStyles.toolbarButton);

            if (EditorGUI.EndChangeCheck())
            {
                // Allow multiple tabs to be selected simultaneously
                tabStates[i] = newState;
            }
        }

        GUILayout.EndHorizontal();
    }
}
