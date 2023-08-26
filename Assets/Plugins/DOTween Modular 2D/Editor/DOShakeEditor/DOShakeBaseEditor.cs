#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace DOTweenModular2D.Editor
{
    [CustomEditor(typeof(DOShakeBase))]
    [CanEditMultipleObjects]
    public class DOShakeBaseEditor : DOBaseEditor
    {
        #region Serialized Properties

        private SerializedProperty fadeOutProp;
        private SerializedProperty vibratoProp;
        private SerializedProperty randomnessProp;
        private SerializedProperty randomnessModeProp;

        #endregion

        private DOShakeBase doShake;

        #region Foldout Bool

        private bool shakeSettingsFoldout = true;
        private string savedShakeSettingsFoldout;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            doShake = (DOShakeBase)target;

            SetupSerializedProperties();
            SetupSavedVariables(doShake);
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
            shakeSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(shakeSettingsFoldout, "Shake Settings");
            EditorPrefs.SetBool(savedShakeSettingsFoldout, shakeSettingsFoldout);
            if (shakeSettingsFoldout)
            {
                EditorGUI.indentLevel++;

                DrawShakeSettings();

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

        protected void OnSceneGUI()
        {
            if (doShake.begin == Enums.Begin.After ||
                doShake.begin == Enums.Begin.With)
            {
                Handles.color = Color.white;

                if (doShake.tweenObject != null)
                    DrawLineToTweenObject();
            }
        }

        #endregion

        #region Draw Functions

        private void DrawShakeSettings()
        {
            EditorGUILayout.PropertyField(fadeOutProp);
            EditorGUILayout.PropertyField(vibratoProp);
            EditorGUILayout.PropertyField(randomnessProp);
            EditorGUILayout.PropertyField(randomnessModeProp);
        }

        #endregion

        #region Setup Functions

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            fadeOutProp = serializedObject.FindProperty("fadeOut");
            vibratoProp = serializedObject.FindProperty("vibrato");
            randomnessProp = serializedObject.FindProperty("randomness");
            randomnessModeProp = serializedObject.FindProperty("randomnessMode");
        }

        protected override void SetupSavedVariables(DOBase dOShake)
        {
            base.SetupSavedVariables(dOShake);

            savedShakeSettingsFoldout = "DOShakeEditor_shakeSettingsFoldout_" + dOShake.GetInstanceID();
            shakeSettingsFoldout = EditorPrefs.GetBool(savedShakeSettingsFoldout, true);
        }

        #endregion

    }

}

#endif