using UnityEditor;

namespace DOTweenModular2D.Editor 
{
    [CustomEditor(typeof(DOPunchAnchorPos))]
    public class DOPunchAnchorPosEditor : DOPunchPositionEditor
    {
        // Empty because unity does not allow more than one 
        // CustomEditor attribute on one editor script
        // and this editor script was exactly same as DOPunchPositionEditor
    }
}
