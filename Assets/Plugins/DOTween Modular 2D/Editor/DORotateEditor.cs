#if UNITY_EDITOR

namespace DOTweenModular2D.Editor
{

using DOTweenModular2D.Enums;
using DG.Tweening;
using DG.DOTweenEditor;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DORotate)), CanEditMultipleObjects]
public class DORotateEditor : DOBaseEditor 
{

#region Serialized Properties

    private SerializedProperty rotateModeProp;
    private SerializedProperty useLocalProp;
    private SerializedProperty speedBasedProp;
    private SerializedProperty relativeProp;
    private SerializedProperty targetZRotationProp;

#endregion

    private DORotate doRotate;

    private string savedRotateSettingsFoldout;
    private bool rotateSettingsFoldout = true;

    private void OnEnable() 
    {
        doRotate = (DORotate) target;

        SetupSerializedProperties();
        SetupSavedVariables(doRotate);
    }

    public override void OnInspectorGUI() 
    {
        EditorGUILayout.Space();

        // Draw Life Time Settings
        lifeTimeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lifeTimeSettingsFoldout, "Life Time Settings");
        EditorPrefs.SetBool(savedLifeTimeSettingsFoldout, lifeTimeSettingsFoldout);
        if (lifeTimeSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawLifeTimeSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        DrawTweenObjectHelpBox();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Draw Type Settings
        typeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(typeSettingsFoldout, "Type Settings");
        EditorPrefs.SetBool(savedTypeSettingsFoldout, typeSettingsFoldout);
        if (typeSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawTypeSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        // Draw Rotate Settings
        rotateSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(rotateSettingsFoldout, "Rotate Settings");
        EditorPrefs.SetBool(savedRotateSettingsFoldout, rotateSettingsFoldout);
        if (rotateSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawRotateSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        

        // Draw Values
        valuesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(valuesFoldout, "Values");
        EditorPrefs.SetBool(savedValuesFoldout, valuesFoldout);
        if (valuesFoldout)
        {
            EditorGUI.indentLevel++;

            DrawValues();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        // Draw Events
        eventsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(eventsFoldout, "Events");
        EditorPrefs.SetBool(savedEventsFoldout, eventsFoldout);
        if (eventsFoldout)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.Space();
            DrawEvents();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();

        if (EditorApplication.isPlaying)
            return;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        DrawPreviewButtons();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        if (doRotate.begin == Begin.After || 
            doRotate.begin == Begin.With)
        {
            Handles.color = Color.white;

            if (doRotate.tweenObject != null)
                DrawLineToTweenObject();            
        }
    }

#region Draw Functions

    protected override void DrawTypeSettings()
    {
        base.DrawTypeSettings();

        EditorGUILayout.PropertyField(rotateModeProp);
    }

    private void DrawRotateSettings()
    {
        EditorGUILayout.PropertyField(useLocalProp);
        EditorGUILayout.PropertyField(speedBasedProp);
        EditorGUILayout.PropertyField(relativeProp);
    }

    protected override void DrawValues()
    {
        EditorGUILayout.PropertyField(targetZRotationProp);

        base.DrawValues();
    }

#endregion

#region Setup Functions

    protected override void SetupSerializedProperties()
    {
        base.SetupSerializedProperties();

        rotateModeProp = serializedObject.FindProperty("rotateMode");
        speedBasedProp = serializedObject.FindProperty("speedBased");
        useLocalProp = serializedObject.FindProperty("useLocal");
        relativeProp = serializedObject.FindProperty("relative");
        targetZRotationProp = serializedObject.FindProperty("targetZRotation");
    }

    protected override void SetupSavedVariables(DOBase dORotate)
    {
        base.SetupSavedVariables(dORotate);
        
        savedRotateSettingsFoldout = "DORotateEditor_rotateSettingsFoldout_" + dORotate.GetInstanceID();
        rotateSettingsFoldout = EditorPrefs.GetBool(savedRotateSettingsFoldout, true);
    }

#endregion

}

}

#endif