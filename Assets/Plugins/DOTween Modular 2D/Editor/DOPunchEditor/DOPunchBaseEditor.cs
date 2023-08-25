#if UNITY_EDITOR

using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOPunchBase))]
    [CanEditMultipleObjects]
    public class DOPunchBaseEditor : DOBaseEditor
    {

        #region Serialized Properties

        private SerializedProperty vibratoProp;
        private SerializedProperty elasticityProp;

        #endregion

        private DOPunchBase doPunch;

        #region Foldout Bool

        private bool punchSettingsFoldout = true;
        private string savedpunchSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doPunch = (DOPunchBase)target;

            SetupSerializedProperties();
            SetupSavedVariables(doPunch);
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
            punchSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(punchSettingsFoldout, "Punch Settings");
            EditorPrefs.SetBool(savedpunchSettingsFoldout, punchSettingsFoldout);
            if (punchSettingsFoldout)
            {
                EditorGUI.indentLevel++;

                DrawPunchSettings();

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
            if (doPunch.begin == Begin.After ||
                doPunch.begin == Begin.With)
            {
                Handles.color = Color.white;

                if (doPunch.tweenObject != null)
                    DrawLineToTweenObject();
            }
        }

        #endregion

        #region Draw Functions

        private void DrawPunchSettings()
        {
            EditorGUILayout.PropertyField(vibratoProp);
            EditorGUILayout.PropertyField(elasticityProp);
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            vibratoProp = serializedObject.FindProperty("vibrato");
            elasticityProp = serializedObject.FindProperty("elasticity");
        }

        protected override void SetupSavedVariables(DOBase dOScale)
        {
            base.SetupSavedVariables(dOScale);

            savedpunchSettingsFoldout = "DOPunchEditor_punchSettingsFoldout_" + dOScale.GetInstanceID();
            punchSettingsFoldout = EditorPrefs.GetBool(savedpunchSettingsFoldout, true);
        }

        #endregion

    }

}

#endif