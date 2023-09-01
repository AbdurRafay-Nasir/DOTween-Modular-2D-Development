#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOShapeCircle))]
    public class DOShapeCircleEditor : DOLookAtBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty useLocalProp;
        private SerializedProperty relativeProp;
        private SerializedProperty snappingProp;
        private SerializedProperty centerProp;
        private SerializedProperty endDegreeProp;

        #endregion

        private DOShapeCircle doShapeCircle;
        private Vector2 beginPosition;

        private bool[] tabStates = new bool[7];
        private string[] savedTabStates = new string[7];

        #region Saved Variables

        private bool firstTimeNonRelative = true;
        private bool firstTimeRelative = false;

        private string savedFirstTimeNonRelative;
        private string savedFirstTimeRelative;

        #endregion

        #region Foldout Settings

        private bool moveSettingsFoldout = true;
        private string savedMoveSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doShapeCircle = (DOShapeCircle)target;
            beginPosition = doShapeCircle.transform.position;

            SetupSerializedProperties();
            SetupSavedVariables(doShapeCircle);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            DrawTabs();

            EditorGUILayout.Space();

            if (tabStates[0])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Life Time Settings
                lifeTimeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lifeTimeSettingsFoldout, "Life Time Settings");
                EditorPrefs.SetBool(savedLifeTimeSettingsFoldout, lifeTimeSettingsFoldout);
                if (lifeTimeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawLifeTimeSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawTweenObjectHelpBox();

            if (tabStates[1])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Type Settings
                typeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(typeSettingsFoldout, "Type Settings");
                EditorPrefs.SetBool(savedTypeSettingsFoldout, typeSettingsFoldout);
                if (typeSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawTypeSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[2])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Scale Settings
                lookAtSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(lookAtSettingsFoldout, "Look At Settings");
                EditorPrefs.SetBool(savedLookAtSettingsFoldout, lookAtSettingsFoldout);
                if (lookAtSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawLookAtSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawLookAtHelpBox();

            if (tabStates[3])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Scale Settings
                moveSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(moveSettingsFoldout, "Move Settings");
                EditorPrefs.SetBool(savedMoveSettingsFoldout, moveSettingsFoldout);
                if (moveSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawMoveSettings();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (tabStates[4])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Values
                valuesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(valuesFoldout, "Values");
                EditorPrefs.SetBool(savedValuesFoldout, valuesFoldout);
                if (valuesFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawValues();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }


            if (tabStates[5])
            {
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
            }

            if (tabStates[6])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Editor
                editorFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(editorFoldout, "Editor");
                EditorPrefs.SetBool(savedEditorFoldout, editorFoldout);
                if (editorFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawEditorProperties();

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying)
                return;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

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
            if (doShapeCircle.begin == Begin.After ||
                doShapeCircle.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doShapeCircle.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            Color handleColor = color[currentHandleColorIndex];
            Color lineColor = color[currentLineColorIndex];

            Vector3 startPosition;

            if (EditorApplication.isPlaying)
                startPosition = beginPosition;
            else if (TweenPreviewing)
                startPosition = positionBeforePreview;
            else
                startPosition = doShapeCircle.transform.position;

            Vector3 handlePosition = CalculateCenterPosition(startPosition);

            DrawCenterLineAndSphere(startPosition, handlePosition, handleColor, lineColor);
            DrawCircle(startPosition, handlePosition, Vector2.Distance(startPosition, handlePosition));

            if (doShapeCircle.lookAt != LookAtSimple.None)
            {
                DrawLookAtLine();
                DrawRotationClampCircle();
            }

            if (!EditorApplication.isPlaying && editPath)
            {
                DrawCenterHandle(handlePosition, handleColor);

                if (doShapeCircle.lookAt == LookAtSimple.Position)
                    DrawLookAtHandle();
            }
        }

        #endregion

        private Vector3 CalculateCenterPosition(Vector2 startPosition)
        {
            Vector3 handlePosition;

            if (doShapeCircle.useLocal)
            {
                if (doShapeCircle.transform.parent != null)
                {
                    handlePosition = doShapeCircle.transform.parent.TransformPoint(doShapeCircle.center);
                }
                else
                {
                    handlePosition = doShapeCircle.center;
                }
            }

            else
            {

                if (doShapeCircle.relative)
                {
                    if (firstTimeRelative)
                    {
                        doShapeCircle.center = doShapeCircle.center - (Vector2)doShapeCircle.transform.position;

                        firstTimeRelative = false;
                        EditorPrefs.SetBool(savedFirstTimeRelative, firstTimeRelative);
                    }

                    handlePosition = startPosition + doShapeCircle.center;

                    firstTimeNonRelative = true;
                    EditorPrefs.SetBool(savedFirstTimeNonRelative, firstTimeNonRelative);
                }
                else
                {
                    if (firstTimeNonRelative)
                    {
                        doShapeCircle.center = doShapeCircle.center + (Vector2)doShapeCircle.transform.position;

                        firstTimeNonRelative = false;
                        EditorPrefs.SetBool(savedFirstTimeNonRelative, firstTimeNonRelative);
                    }

                    handlePosition = doShapeCircle.center;

                    firstTimeRelative = true;
                    EditorPrefs.SetBool(savedFirstTimeRelative, firstTimeRelative);
                }

            }

            return handlePosition;
        }

        #region Scene Draw Functions

        private void DrawCenterHandle(Vector3 handlePosition, Color handleColor)
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
                Undo.RecordObject(doShapeCircle, "Center Move Handle");

                // Perform the handle move and update the serialized data
                Vector2 delta = newHandlePosition - handlePosition;
                doShapeCircle.center += delta;
            }
        }

        private void DrawCenterLineAndSphere(Vector2 startPosition, Vector2 endPosition, 
                                             Color handleColor, Color lineColor)
        {
            Handles.color = handleColor;
            Handles.SphereHandleCap(2, endPosition, Quaternion.identity, currentHandleRadius, EventType.Repaint);

            Handles.color = lineColor;
            Handles.DrawLine(startPosition, endPosition, currentLineWidth);
        }

        private void DrawCircle(Vector2 startPosition, Vector2 center, float radius)
        {
            // Calculate the direction vector from center to startPosition
            Vector2 direction = (startPosition - center).normalized;

            // Calculate the 'from' point using the calculated direction vector and radius
            Vector3 from = new Vector3(direction.x * radius + center.x, direction.y * radius + center.y);
            Handles.DrawWireArc(center, Vector3.back, from, 
                                doShapeCircle.endDegree + 90f, radius, currentLineWidth);
        }

        #endregion

        #region Inspector Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Look At", "Move", "Values", "Events", "Editor" };

            for (int i = 0; i < tabStates.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                bool toggleState = GUILayout.Toggle(tabStates[i], tabNames[i], toggleStyle);
                if (EditorGUI.EndChangeCheck())
                {
                    tabStates[i] = toggleState;
                    EditorPrefs.SetBool(savedTabStates[i], toggleState);
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawMoveSettings()
        {
            EditorGUILayout.PropertyField(useLocalProp);

            if (!doShapeCircle.useLocal)
            {
                EditorGUILayout.PropertyField(relativeProp);
            }

            EditorGUILayout.PropertyField(snappingProp);
        }

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(centerProp);
            EditorGUILayout.PropertyField(endDegreeProp);

            base.DrawValues();
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            useLocalProp = serializedObject.FindProperty("useLocal");
            relativeProp = serializedObject.FindProperty("relative");
            snappingProp = serializedObject.FindProperty("snapping");
            centerProp = serializedObject.FindProperty("center");
            endDegreeProp = serializedObject.FindProperty("endDegree");
        }

        protected override void SetupSavedVariables(DOBase doShapeCircle)
        {
            base.SetupSavedVariables(doShapeCircle);

            int instanceId = doShapeCircle.GetInstanceID();

            savedFirstTimeNonRelative = "DOMoveEditor_firstTimeNonRelative_" + instanceId;
            firstTimeNonRelative = EditorPrefs.GetBool(savedFirstTimeNonRelative, false);

            savedFirstTimeRelative = "DOMoveEditor_firstTimeRelative_" + instanceId;
            firstTimeRelative = EditorPrefs.GetBool(savedFirstTimeRelative, true);

            savedMoveSettingsFoldout = "DOShapeCircleEditor_moveSettingsFoldout_" + instanceId;
            moveSettingsFoldout = EditorPrefs.GetBool(savedMoveSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOShapeCircleEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        #endregion
    }
}

#endif