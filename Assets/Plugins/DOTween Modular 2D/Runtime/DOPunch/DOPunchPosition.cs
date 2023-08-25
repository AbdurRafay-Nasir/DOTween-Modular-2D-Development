namespace DOTweenModular2D
{
    using DG.Tweening;
    using UnityEngine;

    [AddComponentMenu("DOTween Modular 2D/Transform/DO Punch/DO Punch Position")]
    public class DOPunchPosition : DOPunchBase
    {
        public Vector2 punchAmount;
        public bool snapping;

        protected override void InitializeTween()
        {
            tween = transform.DOPunchPosition(punchAmount, duration, vibrato, elasticity, snapping);
        }
    }
}
