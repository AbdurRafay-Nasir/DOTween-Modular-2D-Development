#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOScale)), CanEditMultipleObjects]
    public class DOScaleEditor : DOBaseEditor
    {

        #region Serialized Properties

        private SerializedProperty relativeProp;
        private SerializedProperty speedBasedProp;
        private SerializedProperty targetScaleProp;

        private SerializedProperty lookAtProp;
        private SerializedProperty lookAtPositionProp;
        private SerializedProperty lookAtTargetProp;
        private SerializedProperty minProp;
        private SerializedProperty maxProp;
        private SerializedProperty offsetProp;
        private SerializedProperty smoothFactorProp;

        #endregion

        private DOScale doScale;

        private bool[] tabStates = new bool[6];
        private string[] savedTabStates = new string[6];

        #region Foldout Settings

        private bool lookAtSettingsFoldout = true;
        private string savedLookAtSettingsFoldout;

        private bool scaleSettingsFoldout = true;
        private string savedScaleSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doScale = (DOScale)target;

            SetupSerializedProperties();
            SetupSavedVariables(doScale);
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
             

            if (doScale.lookAt == LookAtSimple.Transform && doScale.lookAtTarget == null)
            {
                EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
            }
            else if (doScale.lookAt != LookAtSimple.Transform && doScale.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove Look At Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                {
                    doScale.lookAtTarget = null;
                }

                EditorGUILayout.EndHorizontal();
            }


            if (tabStates[3])
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // Draw Scale Settings
                scaleSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(scaleSettingsFoldout, "Scale Settings");
                EditorPrefs.SetBool(savedScaleSettingsFoldout, scaleSettingsFoldout);
                if (scaleSettingsFoldout)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.Space();

                    DrawScaleSettings();

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

            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying)
                return;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            DrawPreviewButtons();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void OnSceneGUI()
        {
            if (doScale.begin == Begin.After ||
                doScale.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doScale.tweenObject != null)
                    DrawTweenObjectInfo();
            }

            if (doScale.lookAt == LookAtSimple.None)
                return;

            Vector2 look = doScale.transform.position;

            if (doScale.lookAt == LookAtSimple.Position)
            {
                look = doScale.lookAtPosition;

                Vector2 newLookAtPosition = Handles.PositionHandle(look, Quaternion.identity);

                if (newLookAtPosition != doScale.lookAtPosition)
                {
                    Undo.RecordObject(doScale, "Change Look At Position");
                    doScale.lookAtPosition = newLookAtPosition;
                }
            }

            else if (doScale.lookAtTarget != null)
            {
                look = doScale.lookAtTarget.position;
            }

            Handles.color = Color.green;

            DrawRotationClampCircle();
            Handles.DrawDottedLine(doScale.transform.position, look, 5f);
        }

        #endregion

        #region Draw Functions

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();

            GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
            toggleStyle.fixedHeight = 30f;

            string[] tabNames = new string[] { "Life", "Type", "Look At", "Scale", "Values", "Events" };

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

        private void DrawLookAtSettings()
        {
            EditorGUILayout.PropertyField(lookAtProp);

            if (doScale.lookAt == LookAtSimple.None)
                return;

            if (doScale.lookAt == LookAtSimple.Position)
                EditorGUILayout.PropertyField(lookAtPositionProp);
            else
                EditorGUILayout.PropertyField(lookAtTargetProp);

            EditorGUILayout.PropertyField(offsetProp);
            EditorGUILayout.PropertyField(minProp);
            EditorGUILayout.PropertyField(maxProp);
            EditorGUILayout.PropertyField(smoothFactorProp);
        }

        private void DrawRotationClampCircle()
        {
            Vector3 position = doScale.transform.position;

            // Calculate the endpoints of the arc based on the min and max angles
            float minAngle = (doScale.min + 90) * Mathf.Deg2Rad;
            float maxAngle = (doScale.max + 90) * Mathf.Deg2Rad;
            Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
            Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

            // Draw the circle representing the range
            Handles.DrawWireArc(position, Vector3.forward, minDir, doScale.max - doScale.min, 2f);

            // Draw lines from the center to the min and max angles
            Handles.DrawLine(position, position + minDir * 2f);
            Handles.DrawLine(position, position + maxDir * 2f);
        }

        private void DrawScaleSettings()
        {
            EditorGUILayout.PropertyField(speedBasedProp);
            EditorGUILayout.PropertyField(relativeProp);
        }

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(targetScaleProp);

            base.DrawValues();
        }

        #endregion

        #region Setup

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            speedBasedProp = serializedObject.FindProperty("speedBased");
            relativeProp = serializedObject.FindProperty("relative");
            targetScaleProp = serializedObject.FindProperty("targetScale");

            lookAtProp = serializedObject.FindProperty("lookAt");
            lookAtPositionProp = serializedObject.FindProperty("lookAtPosition");
            lookAtTargetProp = serializedObject.FindProperty("lookAtTarget");
            minProp = serializedObject.FindProperty("min");
            maxProp = serializedObject.FindProperty("max");
            offsetProp = serializedObject.FindProperty("offset");
            smoothFactorProp = serializedObject.FindProperty("smoothFactor");
        }

        protected override void SetupSavedVariables(DOBase dOScale)
        {
            base.SetupSavedVariables(dOScale);

            int instanceId = dOScale.GetInstanceID();

            savedLookAtSettingsFoldout = "DOScaleEditor_lookAtSettingsFoldout_" + instanceId;
            lookAtSettingsFoldout = EditorPrefs.GetBool(savedLookAtSettingsFoldout, true);

            savedScaleSettingsFoldout = "DOScaleEditor_scaleSettingsFoldout_" + instanceId;
            scaleSettingsFoldout = EditorPrefs.GetBool(savedScaleSettingsFoldout, true);

            for (int i = 0; i < savedTabStates.Length; i++)
            {
                savedTabStates[i] = "DOScaleEditor_tabStates_" + i + " " + instanceId;
                tabStates[i] = EditorPrefs.GetBool(savedTabStates[i], true);
            }
        }

        #endregion

    }

}

#endif