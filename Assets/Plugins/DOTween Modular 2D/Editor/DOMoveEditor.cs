#if UNITY_EDITOR

namespace DOTweenModular2D.Editor
{
using DOTweenModular2D.Enums;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DOMove)), CanEditMultipleObjects]
public class DOMoveEditor : DOBaseEditor 
{

#region Serialized Properties

    private SerializedProperty speedBasedProp;
    private SerializedProperty useLocalProp;
    private SerializedProperty relativeProp;
    private SerializedProperty snappingProp;
    private SerializedProperty targetPositionProp;

    private SerializedProperty lookAtProp;
    private SerializedProperty lookAtTargetProp;
    private SerializedProperty lookAtPositionProp;
    private SerializedProperty minProp;
    private SerializedProperty maxProp;
    private SerializedProperty offsetProp;
    private SerializedProperty smoothFactorProp;

#endregion

    private DOMove doMove;
    private Vector3 beginPosition;

#region Saved Variables

    private bool firstTimeNonRelative = true;
    private bool firstTimeRelative = false;

    private string savedFirstTimeNonRelative;
    private string savedFirstTimeRelative;

#endregion

#region Foldout bool Properties

    private bool moveSettingsFoldout = true;
    private string savedMoveSettingsFoldout;

    private bool lookAtSettingsFoldout = true;
    private string savedLookAtSettingsFoldout;

#endregion

#region Unity Functions

    private void OnEnable() 
    {
        doMove = (DOMove) target;
        beginPosition = doMove.transform.position;

        SetupSerializedProperties();
        SetupSavedVariables(doMove);
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


        // Draw Move Settings
        moveSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(moveSettingsFoldout, "Move Settings");
        EditorPrefs.SetBool(savedMoveSettingsFoldout, moveSettingsFoldout);
        if (moveSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawMoveSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        // Draw LookAt Settings
        lookAtSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lookAtSettingsFoldout, "LookAt Settings");
        EditorPrefs.SetBool(savedLookAtSettingsFoldout, lookAtSettingsFoldout);
        if (lookAtSettingsFoldout)
        {
            EditorGUI.indentLevel++;

            DrawLookAtSettings();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        if (doMove.lookAt == LookAtSimple.Transform && doMove.lookAtTarget == null)
        {
            EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
        }
        else if (doMove.lookAt != LookAtSimple.Transform && doMove.lookAtTarget != null)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);
        
            GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
            trashButton.tooltip = "Remove Look At Target";

            if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
            {
                doMove.lookAtTarget = null;
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

        // Draw Editor
        editorFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(editorFoldout, "Editor");
        EditorPrefs.SetBool(savedEditorFoldout, editorFoldout);
        if (editorFoldout)
        {
            EditorGUI.indentLevel++;

            DrawEditorProperties();

            EditorGUILayout.Space();            

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        DrawEditButton();

        if (!EditorApplication.isPlaying)
            DrawPreviewButtons();
            
        DrawResetEditorPropertiesButton();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI() 
    {
        if (doMove.begin == Begin.After || 
            doMove.begin == Begin.With)
        {
            Handles.color = Color.white;

            if (doMove.tweenObject != null)
                DrawLineToTweenObject();            
        }

        Color handleColor = color[currentHandleColorIndex];
        Color lineColor = color[currentLineColorIndex];


        Vector3 startPosition;

        if (EditorApplication.isPlaying)
            startPosition = beginPosition;
        else if (TweenPreviewing)
            startPosition = positionBeforePreview;
        else
            startPosition = doMove.transform.position;

        Vector3 handlePosition = CalculateTargetPosition(startPosition);
        DrawTargetLineAndSphere(startPosition, handlePosition, lineColor);

        if (doMove.lookAt != LookAtSimple.None) 
        {
            DrawLookAtLine(lineColor);
            DrawRotationClampCircle();
        }

        if (!EditorApplication.isPlaying && editPath)
        {
            DrawTargetHandle(handlePosition, handleColor);

            if (doMove.lookAt == LookAtSimple.Position)
                DrawLookAtHandle();
        }

    }

#endregion

    private Vector3 CalculateTargetPosition(Vector2 startPosition)
    {
        Vector3 handlePosition;

        if (doMove.useLocal)
        {
            if (doMove.transform.parent != null)
            {
                handlePosition = doMove.transform.parent.TransformPoint(doMove.targetPosition);
            }
            else
            {
                handlePosition = doMove.targetPosition;
            }
        }

        else
        {

            if (doMove.relative)
            {
                if (firstTimeRelative)
                {
                    doMove.targetPosition = doMove.targetPosition - (Vector2)doMove.transform.position;

                    firstTimeRelative = false;
                    EditorPrefs.SetBool(savedFirstTimeRelative, firstTimeRelative);
                }

                handlePosition = startPosition + doMove.targetPosition;

                firstTimeNonRelative = true;
                EditorPrefs.SetBool(savedFirstTimeNonRelative, firstTimeNonRelative);
            }
            else
            {
                if (firstTimeNonRelative)
                {
                    doMove.targetPosition = doMove.targetPosition + (Vector2)doMove.transform.position;
                    
                    firstTimeNonRelative = false;
                    EditorPrefs.SetBool(savedFirstTimeNonRelative, firstTimeNonRelative);
                }

                handlePosition = doMove.targetPosition;

                firstTimeRelative = true;
                EditorPrefs.SetBool(savedFirstTimeRelative, firstTimeRelative);
            }
            
        }

        return handlePosition;
    }

#region Draw Functions

    private void DrawTargetLineAndSphere(Vector3 startPosition, Vector3 endPosition, Color lineColor)
    {
        Handles.color = lineColor;

        Handles.SphereHandleCap(2, endPosition, Quaternion.identity, currentHandleRadius, EventType.Repaint);
        Handles.DrawLine(startPosition, endPosition, currentLineWidth);
    }

    private void DrawTargetHandle(Vector3 handlePosition, Color handleColor)
    {
        Vector3 newHandlePosition;

        if (currentHandleIndex == 0)
            newHandlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);

        else
        {
            Handles.color = handleColor;

            newHandlePosition = Handles.FreeMoveHandle(handlePosition, Quaternion.identity, 
                                        currentHandleRadius, Vector3.zero, Handles.SphereHandleCap);
        }

        if (newHandlePosition != handlePosition)
        {
            // Register the current object for undo
            Undo.RecordObject(doMove, "Move Handle");

            // Perform the handle move and update the serialized data
            Vector2 delta = newHandlePosition - handlePosition;
            doMove.targetPosition += delta;
        }
    }

    private void DrawLookAtLine(Color lineColor)
    {
        Handles.color = lineColor;

        if (doMove.lookAt == LookAtSimple.Position)
        {
            Handles.DrawDottedLine(doMove.transform.position, doMove.lookAtPosition, 5f);
        }
        else if (doMove.lookAtTarget != null)
        {
            Handles.DrawDottedLine(doMove.transform.position, doMove.lookAtTarget.position, 5f);
        }
    }

    private void DrawRotationClampCircle()
    {
        Vector3 position = doMove.transform.position;

        // Calculate the endpoints of the arc based on the min and max angles
        float minAngle = (doMove.min + 90) * Mathf.Deg2Rad;
        float maxAngle = (doMove.max + 90) * Mathf.Deg2Rad;
        Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
        Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

        // Draw the circle representing the range
        Handles.DrawWireArc(position, Vector3.forward, minDir, doMove.max - doMove.min, 2f);

        // Draw lines from the center to the min and max angles
        Handles.DrawLine(position, position + minDir * 2f);
        Handles.DrawLine(position, position + maxDir * 2f);
    }

    private void DrawLookAtHandle()
    {
        Vector2 newLookAtPosition = Handles.PositionHandle(doMove.lookAtPosition, Quaternion.identity);

        if (newLookAtPosition != doMove.lookAtPosition)
        {
            Undo.RecordObject(doMove, "lookAtPosition Handle");
            doMove.lookAtPosition = newLookAtPosition;
        }
    }

#endregion

#region Draw Foldout Functions

    private void DrawMoveSettings()
    {
        EditorGUILayout.PropertyField(speedBasedProp);
        EditorGUILayout.PropertyField(useLocalProp);
        EditorGUILayout.PropertyField(relativeProp);
        EditorGUILayout.PropertyField(snappingProp);
    }

    private void DrawLookAtSettings()
    {
        EditorGUILayout.PropertyField(lookAtProp);

        if ((LookAtSimple) lookAtProp.enumValueIndex == LookAtSimple.None)
            return;

        switch ((LookAtSimple)lookAtProp.enumValueIndex)
        {
            case LookAtSimple.Position:
                EditorGUILayout.PropertyField(lookAtPositionProp);
            break;

            case LookAtSimple.Transform:
                EditorGUILayout.PropertyField(lookAtTargetProp);
            break;
        }
        EditorGUILayout.PropertyField(offsetProp);
        EditorGUILayout.PropertyField(minProp);
        EditorGUILayout.PropertyField(maxProp);
        EditorGUILayout.PropertyField(smoothFactorProp);
    }

    protected override void DrawValues()
    {
        EditorGUILayout.PropertyField(targetPositionProp);
        base.DrawValues();
    }

#endregion

#region Setup Functions

    protected override void SetupSerializedProperties()
    {
        base.SetupSerializedProperties();
        speedBasedProp = serializedObject.FindProperty("speedBased");
        useLocalProp = serializedObject.FindProperty("useLocal");
        relativeProp = serializedObject.FindProperty("relative");
        snappingProp = serializedObject.FindProperty("snapping");
        targetPositionProp = serializedObject.FindProperty("targetPosition");

        lookAtProp = serializedObject.FindProperty("lookAt");
        lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
        lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
        minProp = serializedObject.FindProperty("min");
        maxProp = serializedObject.FindProperty("max");
        offsetProp = serializedObject.FindProperty("offset");
        smoothFactorProp = serializedObject.FindProperty("smoothFactor");
    }

    protected override void SetupSavedVariables(DOBase doMove)
    {
        base.SetupSavedVariables(doMove);

        int instanceId = doMove.GetInstanceID();

        savedFirstTimeNonRelative = "DOMoveEditor_firstTimeNonRelative_" + instanceId;
        firstTimeNonRelative = EditorPrefs.GetBool(savedFirstTimeNonRelative, false);

        savedFirstTimeRelative = "DOMoveEditor_firstTimeRelative_" + instanceId;
        firstTimeRelative = EditorPrefs.GetBool(savedFirstTimeRelative, true);

        savedMoveSettingsFoldout = "DOMoveEditor_moveSettingsFoldout_" + instanceId;
        moveSettingsFoldout = EditorPrefs.GetBool(savedMoveSettingsFoldout, true);

        savedLookAtSettingsFoldout = "DOMoveEditor_lookAtSettingsFoldout_" + instanceId;
        lookAtSettingsFoldout = EditorPrefs.GetBool(savedLookAtSettingsFoldout, true);
    }

#endregion

}

}
#endif