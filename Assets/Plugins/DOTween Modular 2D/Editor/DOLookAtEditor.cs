#if UNITY_EDITOR

namespace DOTweenModular2D.Editor
{

using DOTweenModular2D;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DOLookAt))]
public class DOLookAtEditor : DOBaseEditor
{

#region Serialized Properties

    public SerializedProperty lookAtProp;
    public SerializedProperty lookAtTargetProp;
    public SerializedProperty lookAtPositionProp;
    public SerializedProperty offsetProp;
    public SerializedProperty minProp;
    public SerializedProperty maxProp;
    public SerializedProperty smoothFactorProp;

#endregion

#region Foldout Bool

    private bool lookAtSettingsFoldout = true;
    private string savedLookAtSettingsFoldout;

#endregion

    private DOLookAt doLookAt;

#region Unity Functions

    private void OnEnable()
    {
        doLookAt = (DOLookAt) target;

        SetupSerializedProperties();
        SetupSavedVariables(doLookAt);
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

        lookAtSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lookAtSettingsFoldout, "LookAt Settings");
        EditorPrefs.SetBool(savedLookAtSettingsFoldout, lookAtSettingsFoldout);
        if (lookAtSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawLookAtSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        if (doLookAt.lookAt == Enums.LookAtSimple.Transform && doLookAt.lookAtTarget == null)
        {
            EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
        }
        else if (doLookAt.lookAt != Enums.LookAtSimple.Transform && doLookAt.lookAtTarget != null)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);
        
            GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
            trashButton.tooltip = "Remove Look At Target";

            if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
            {
                doLookAt.lookAtTarget = null;
            }

            EditorGUILayout.EndHorizontal(); 
        }

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


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

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
        if (doLookAt.begin == Enums.Begin.After || 
            doLookAt.begin == Enums.Begin.With)
        {
            Handles.color = Color.white;

            if (doLookAt.tweenObject != null)
                DrawLineToTweenObject();            
        }

        if (doLookAt.lookAt == Enums.LookAtSimple.None) 
            return;

        Handles.color = Color.green;

        if (doLookAt.lookAt == Enums.LookAtSimple.Position)
        {
            DrawLookAtHandle();
        }

        DrawRotationClampCircle();

        DrawLookAtLine();
    }

#endregion

#region Scene View Draw Functions

    private void DrawLookAtHandle()
    {
        Vector2 newLookAtPosition = Handles.PositionHandle(doLookAt.lookAtPosition, Quaternion.identity);

        if (newLookAtPosition != doLookAt.lookAtPosition)
        {
            Undo.RecordObject(doLookAt, "Change Look At Position_DOLookAt");
            doLookAt.lookAtPosition = newLookAtPosition;
        }
    }

    private void DrawRotationClampCircle()
    {
        Vector3 position = doLookAt.transform.position;

        // Calculate the endpoints of the arc based on the min and max angles
        float minAngle = (doLookAt.min + 90) * Mathf.Deg2Rad;
        float maxAngle = (doLookAt.max + 90) * Mathf.Deg2Rad;
        Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
        Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

        // Draw the circle representing the range
        Handles.DrawWireArc(position, Vector3.forward, minDir, doLookAt.max - doLookAt.min, 2f);

        // Draw lines from the center to the min and max angles
        Handles.DrawLine(position, position + minDir * 2f);
        Handles.DrawLine(position, position + maxDir * 2f);
    }

    private void DrawLookAtLine()
    {
        if (doLookAt.lookAt == Enums.LookAtSimple.Position)
        {
            Handles.DrawDottedLine(doLookAt.transform.position, doLookAt.lookAtPosition, 5f);
        }
        else if (doLookAt.lookAtTarget != null)                
        {
            Handles.DrawDottedLine(doLookAt.transform.position, doLookAt.lookAtTarget.position, 5f);
        }
    }

#endregion

#region Inspector Draw Functions

    protected override void DrawTypeSettings()
    {
        EditorGUILayout.PropertyField(beginProp);

        if (doLookAt.begin == Enums.Begin.With ||
            doLookAt.begin == Enums.Begin.After)
        {
            EditorGUILayout.PropertyField(tweenObjectProp);
        }
    }

    protected override void DrawValues()
    {
        EditorGUILayout.PropertyField(delayProp);
        EditorGUILayout.PropertyField(durationProp);
    }

    private void DrawLookAtSettings()
    {
        EditorGUILayout.PropertyField(lookAtProp);

        if (doLookAt.lookAt == Enums.LookAtSimple.None)
            return;

        switch (doLookAt.lookAt)
        {
            case Enums.LookAtSimple.Position:
                EditorGUILayout.PropertyField(lookAtPositionProp);
            break;

            case Enums.LookAtSimple.Transform:
                EditorGUILayout.PropertyField(lookAtTargetProp);
            break;
        }
        EditorGUILayout.PropertyField(offsetProp);
        EditorGUILayout.PropertyField(smoothFactorProp);
        EditorGUILayout.PropertyField(minProp);
        EditorGUILayout.PropertyField(maxProp);
    }

#endregion

#region Setup Functions

    protected override void SetupSerializedProperties()
    {
        base.SetupSerializedProperties();

        lookAtProp = serializedObject.FindProperty("lookAt");
        lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
        lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
        offsetProp = serializedObject.FindProperty("offset");
        smoothFactorProp = serializedObject.FindProperty("smoothFactor");
        minProp = serializedObject.FindProperty("min");
        maxProp = serializedObject.FindProperty("max");
    }

    protected override void SetupSavedVariables(DOBase doLookAt)
    {
        base.SetupSavedVariables(doLookAt);

        int instanceId = doLookAt.GetInstanceID();

        savedLookAtSettingsFoldout = "DOMoveEditor_lookAtSettingsFoldout_" + instanceId;
        lookAtSettingsFoldout = EditorPrefs.GetBool(savedLookAtSettingsFoldout, true);
    }

#endregion
}

}

#endif