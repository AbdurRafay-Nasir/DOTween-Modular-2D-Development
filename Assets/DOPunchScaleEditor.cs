using UnityEditor;

namespace DOTweenModular2D.Editor
{

    [CustomEditor(typeof(DOPunchScale))]
    public class DOPunchScaleEditor : DOPunchBaseEditor
    {
        private SerializedProperty punchAmountProp;

        protected override void DrawValues()
        {
            EditorGUILayout.PropertyField(punchAmountProp);

            base.DrawValues();
        }

        protected override void SetupSerializedProperties()
        {
            base.SetupSerializedProperties();

            punchAmountProp = serializedObject.FindProperty("punchAmount");
        }
    }

}