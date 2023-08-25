using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{

    [AddComponentMenu("DOTween Modular 2D/DOPunch/DO Punch Anchor Pos")]
    public class DOPunchAnchorPos : DOPunchBase
    {
        public Vector2 punchAmount;
        public bool snapping;

        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;
            tween = rectTransform.DOPunchAnchorPos(punchAmount, duration, vibrato, elasticity, snapping);
        }
    }
}