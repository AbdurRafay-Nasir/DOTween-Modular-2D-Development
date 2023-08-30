using DG.Tweening;
using DOTweenModular2D.Enums;
using UnityEngine;

namespace DOTweenModular2D
{   
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Move", 50)]
    public class DOMove : DOBase
    {
        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                  "targetPosition = targetPosition + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" + 
                 "targetPosition = targetPosition + transform.position")]
        public Vector2 targetPosition;

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        [Tooltip("Type of Look At")]
        public LookAtSimple lookAt;

        [Tooltip("The game Object to Look At")]
        public Transform lookAtTarget;

        [Tooltip("The position to Look At")]
        public Vector2 lookAtPosition;

        [Tooltip("The offet to add to rotation, value of -90 means the game object will look directly towards lookAtPosition/lookAtTarget")]
        public float offset = -90f;

        [Tooltip("Minimum Rotation, Set min to 0 and max to 360 for no rotation clamp")]
        [Range(0f, 360f)] public float min = 0f;

        [Tooltip("Maximum Rotation, Set min to 0 and max to 360 for no rotation clamp")]
        [Range(0f, 360f)] public float max = 360f;

        [Tooltip("Smoothness of rotation, 1 means there will be no smoothness")]
        [Range(0f, 1f)] public float smoothFactor = 0.01f;

        public override void CreateTween()
        {
            if (useLocal)
                tween = transform.DOLocalMove(targetPosition, duration, snapping);
          
            else
                tween = transform.DOMove(targetPosition, duration, snapping);
        
            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetSpeedBased(speedBased);
            tween.SetRelative(relative);
            tween.SetDelay(delay);

            InvokeTweenCreated();

            if (lookAt != LookAtSimple.None)
            {
                tween.onUpdate += OnTweenUpdated;
            }

        }

        private void OnTweenUpdated()
        {
            if (lookAt == LookAtSimple.Position)
            {
                transform.LookAt2DSmooth(lookAtPosition, offset, smoothFactor, min, max);
            }
            else
            {
                transform.LookAt2DSmooth(lookAtTarget, offset, smoothFactor, min, max);
            }
        }

        protected new void OnDestroy()
        {
            base.OnDestroy();

            lookAtTarget = null;
        }
    }
}