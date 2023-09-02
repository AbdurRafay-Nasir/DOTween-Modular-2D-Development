#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DOTweenModular2D.Editor
{
    public class DOLookAtBaseEditor : DOBaseEditor
    {
        protected SerializedProperty lookAtProp;
        protected SerializedProperty lookAtTargetProp;
        protected SerializedProperty lookAtPositionProp;
        protected SerializedProperty offsetProp;
        protected SerializedProperty minProp;
        protected SerializedProperty maxProp;
        protected SerializedProperty smoothFactorProp;

        #region Foldout Bool

        protected bool lookAtSettingsFoldout = true;
        protected string savedLookAtSettingsFoldout;

        #endregion

        private DOLookAt dOLookAt;

        #region Inspector Draw Functions

        protected void DrawLookAtSettings()
        {
            EditorGUILayout.PropertyField(lookAtProp);

            if (dOLookAt.lookAt == Enums.LookAtSimple.None)
                return;

            switch (dOLookAt.lookAt)
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

        protected void DrawLookAtHelpBox()
        {
            if (dOLookAt.lookAt == Enums.LookAtSimple.Transform && dOLookAt.lookAtTarget == null)
            {
                EditorGUILayout.HelpBox("Look At Target not Assigned", MessageType.Error);
            }
            else if (dOLookAt.lookAt != Enums.LookAtSimple.Transform && dOLookAt.lookAtTarget != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.HelpBox("Look At Target is still Assigned, it Should be removed", MessageType.Warning);

                GUIContent trashButton = EditorGUIUtility.IconContent("TreeEditor.Trash");
                trashButton.tooltip = "Remove Look At Target";

                if (GUILayout.Button(trashButton, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize * 2f)))
                {
                    dOLookAt.lookAtTarget = null;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        #endregion

        #region Scene View Draw Functions

        protected void DrawLookAtHandle()
        {
            Vector2 newLookAtPosition = Handles.PositionHandle(dOLookAt.lookAtPosition, Quaternion.identity);

            if (newLookAtPosition != dOLookAt.lookAtPosition)
            {
                Undo.RecordObject(dOLookAt, "Change Look At Position_DOLookAt");
                dOLookAt.lookAtPosition = newLookAtPosition;
            }
        }

        protected void DrawRotationClampCircle()
        {
            Vector3 position = dOLookAt.transform.position;

            // Calculate the endpoints of the arc based on the min and max angles
            float minAngle = (dOLookAt.min + 90) * Mathf.Deg2Rad;
            float maxAngle = (dOLookAt.max + 90) * Mathf.Deg2Rad;
            Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
            Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

            // Draw the circle representing the range
            Handles.DrawWireArc(position, Vector3.forward, minDir, dOLookAt.max - dOLookAt.min, 2f);

            // Draw lines from the center to the min and max angles
            Handles.DrawLine(position, position + minDir * 2f);
            Handles.DrawLine(position, position + maxDir * 2f);
        }

        protected void DrawLookAtLine()
        {
            if (dOLookAt.lookAt == Enums.LookAtSimple.Position)
            {
                Handles.DrawDottedLine(dOLookAt.transform.position, dOLookAt.lookAtPosition, 5f);
            }
            else if (dOLookAt.lookAtTarget != null)
            {
                Handles.DrawDottedLine(dOLookAt.transform.position, dOLookAt.lookAtTarget.position, 5f);
            }
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
            dOLookAt = (DOLookAt)doLookAt;
            int instanceId = doLookAt.GetInstanceID();

            savedLookAtSettingsFoldout = "DOLookAtBaseEditor_lookAtSettingsFoldout_" + instanceId;
            lookAtSettingsFoldout = EditorPrefs.GetBool(savedLookAtSettingsFoldout, true);
        }

        #endregion
    }
}

#endif