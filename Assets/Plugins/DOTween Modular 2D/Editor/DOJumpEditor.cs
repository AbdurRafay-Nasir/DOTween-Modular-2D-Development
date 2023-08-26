#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEditor;
using UnityEngine;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOJump))]
    [CanEditMultipleObjects]
    public class DOJumpEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty jumpPowerProp;
        private SerializedProperty jumpsProp;
        private SerializedProperty useLocalProp;
        private SerializedProperty relativeProp;
        private SerializedProperty targetPositionProp;
        private SerializedProperty snappingProp;

        private SerializedProperty lookAtProp;
        private SerializedProperty lookAtTargetProp;
        private SerializedProperty lookAtPositionProp;
        private SerializedProperty offsetProp;
        private SerializedProperty minProp;
        private SerializedProperty maxProp;
        private SerializedProperty smoothFactorProp;

        #endregion

        #region Saved Variables

        private bool firstTimeNonRelative = true;
        private bool firstTimeRelative = false;

        private string savedFirstTimeNonRelative;
        private string savedFirstTimeRelative;

        #endregion

        #region Foldout Bools

        private bool jumpSettingsFoldout = true;
        private string savedJumpSettingsFoldout;

        private bool lookAtSettingsFoldout = true;
        private string savedLookAtSettingsFoldout;

        #endregion

        private DOJump doJump;
        private Vector2 beginPosition;

        #region Unity Functions

        private void OnEnable()
        {
            doJump = (DOJump)target;
            beginPosition = doJump.transform.position;

            SetupSerializedProperties();
            SetupSavedVariables(doJump);
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
            jumpSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(jumpSettingsFoldout, "Jump Settings");
            EditorPrefs.SetBool(savedJumpSettingsFoldout, jumpSettingsFoldout);
            if (jumpSettingsFoldout)
            {
                EditorGUI.indentLevel++;

                DrawJumpSettings();

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

            if (doJump.lookAt == LookAtSimple.Transform && doJump.lookAtTarget == null)
            {
                EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
            }
            else if (doJump.lookAt != LookAtSimple.Transform && doJump.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove Look At Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                {
                    doJump.lookAtTarget = null;
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
            if (doJump.begin == Begin.After ||
                doJump.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doJump.tweenObject != null)
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
                startPosition = doJump.transform.position;

            Vector3 handlePosition = CalculateTargetPosition(startPosition);

            if (doJump.lookAt != LookAtSimple.None)
            {
                DrawLookAtLine(lineColor);
                DrawRotationClampCircle();
            }

            if (!EditorApplication.isPlaying && editPath)
            {
                DrawTargetHandle(handlePosition, handleColor);

                if (doJump.lookAt == LookAtSimple.Position)
                    DrawLookAtHandle();
            }

        }

        #endregion

        private Vector3 CalculateTargetPosition(Vector2 startPosition)
        {
            Vector3 handlePosition;

            if (doJump.useLocal)
            {
                if (doJump.transform.parent != null)
                {
                    handlePosition = doJump.transform.parent.TransformPoint(doJump.targetPosition);
                }
                else
                {
                    handlePosition = doJump.targetPosition;
                }
            }

            else
            {

                if (doJump.relative)
                {
                    if (firstTimeRelative)
                    {
                        doJump.targetPosition = doJump.targetPosition - (Vector2)doJump.transform.position;

                        firstTimeRelative = false;
                        EditorPrefs.SetBool(savedFirstTimeRelative, firstTimeRelative);
                    }

                    handlePosition = startPosition + doJump.targetPosition;

                    firstTimeNonRelative = true;
                    EditorPrefs.SetBool(savedFirstTimeNonRelative, firstTimeNonRelative);
                }
                else
                {
                    if (firstTimeNonRelative)
                    {
                        doJump.targetPosition = doJump.targetPosition + (Vector2)doJump.transform.position;

                        firstTimeNonRelative = false;
                        EditorPrefs.SetBool(savedFirstTimeNonRelative, firstTimeNonRelative);
                    }

                    handlePosition = doJump.targetPosition;

                    firstTimeRelative = true;
                    EditorPrefs.SetBool(savedFirstTimeRelative, firstTimeRelative);
                }

            }

            return handlePosition;
        }

        #region Draw Sceneview Functions

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
                Undo.RecordObject(doJump, "Move Handle");

                // Perform the handle move and update the serialized data
                Vector2 delta = newHandlePosition - handlePosition;
                doJump.targetPosition += delta;
            }
        }

        private void DrawRotationClampCircle()
        {
            Vector3 position = doJump.transform.position;

            // Calculate the endpoints of the arc based on the min and max angles
            float minAngle = (doJump.min + 90) * Mathf.Deg2Rad;
            float maxAngle = (doJump.max + 90) * Mathf.Deg2Rad;
            Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
            Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

            // Draw the circle representing the range
            Handles.DrawWireArc(position, Vector3.forward, minDir, doJump.max - doJump.min, 2f);

            // Draw lines from the center to the min and max angles
            Handles.DrawLine(position, position + minDir * 2f);
            Handles.DrawLine(position, position + maxDir * 2f);
        }

        private void DrawLookAtLine(Color lineColor)
        {
            Handles.color = lineColor;

            if (doJump.lookAt == LookAtSimple.Position)
            {
                Handles.DrawDottedLine(doJump.transform.position, doJump.lookAtPosition, 5f);
            }
            else if (doJump.lookAtTarget != null)
            {
                Handles.DrawDottedLine(doJump.transform.position, doJump.lookAtTarget.position, 5f);
            }
        }

        private void DrawLookAtHandle()
        {
            Vector2 newLookAtPosition = Handles.PositionHandle(doJump.lookAtPosition, Quaternion.identity);

            if (newLookAtPosition != doJump.lookAtPosition)
            {
                Undo.RecordObject(doJump, "lookAtPosition Handle");
                doJump.lookAtPosition = newLookAtPosition;
            }
        }

        #endregion

        #region Draw Inspector Functions

        private void DrawJumpSettings()
        {
            EditorGUILayout.PropertyField(jumpPowerProp);
            EditorGUILayout.PropertyField(jumpsProp);
            EditorGUILayout.PropertyField(useLocalProp);
            
            if (!doJump.useLocal) 
                EditorGUILayout.PropertyField(relativeProp);
        }

        private void DrawLookAtSettings()
        {
            EditorGUILayout.PropertyField(lookAtProp);

            if ((LookAtSimple)lookAtProp.enumValueIndex == LookAtSimple.None)
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
            EditorGUILayout.PropertyField(snappingProp);

            base.DrawValues();
        }

        #endregion

        #region Setup Functions

        protected override void SetupSavedVariables(DOBase doBase)
        {
            base.SetupSavedVariables(doBase);

            int instanceId = doJump.GetInstanceID();

            savedFirstTimeNonRelative = "DOMoveEditor_firstTimeNonRelative_" + instanceId;
            firstTimeNonRelative = EditorPrefs.GetBool(savedFirstTimeNonRelative, false);

            savedFirstTimeRelative = "DOMoveEditor_firstTimeRelative_" + instanceId;
            firstTimeRelative = EditorPrefs.GetBool(savedFirstTimeRelative, true);

            savedJumpSettingsFoldout = "doJumpEditor_moveSettingsFoldout_" + instanceId;
            jumpSettingsFoldout = EditorPrefs.GetBool(savedJumpSettingsFoldout, true);
        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            jumpPowerProp = serializedObject.FindProperty("jumpPower");
            jumpsProp = serializedObject.FindProperty("jumps");
            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");

            targetPositionProp = serializedObject.FindProperty("targetPosition");
            snappingProp = serializedObject.FindProperty("snapping");

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            offsetProp = serializedObject.FindProperty("offset");
            minProp = serializedObject.FindProperty("min");
            maxProp = serializedObject.FindProperty("max");
            smoothFactorProp = serializedObject.FindProperty("smoothFactor");
        }

        #endregion
    }
}

#endif