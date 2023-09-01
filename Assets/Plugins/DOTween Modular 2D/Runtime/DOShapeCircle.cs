using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    public class DOShapeCircle : DOLookAt
    {

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        [Tooltip("If TRUE, the center will be calculated as: " + "\n" +
          "center = center + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("Center of Circle")]
        public Vector2 center;

        [Tooltip("The degree at which object will stop, 360 means mid of 1st & 2nd Quadrant")]
        public float endDegree;

        public override void CreateTween()
        {
            if (useLocal)
                tween = transform.DOLocalShapeCircle(center, endDegree, duration, snapping);
            else
                tween = transform.DOShapeCircle(center, endDegree, duration, relative, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetDelay(delay);

            InvokeTweenCreated();

            SetupLookAt();
        }
    }
}
