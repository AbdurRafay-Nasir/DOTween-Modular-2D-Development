namespace DOTweenModular2D
{

using DG.Tweening;
using DOTweenModular2D.Enums;
using UnityEngine;

[AddComponentMenu("DOTween Modular 2D/DO Scale", 3)]
public class DOScale : DOBase
{
    [Tooltip("If TRUE, the tween will Move duration amount in each second")]
    public bool speedBased;

    [Tooltip("If TRUE, the targetScale will be calculated as: " + "\n" +
            "targetScale = targetScale + transform.localScale")]
    public bool relative;

    [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" +
             "targetScale = targetScale + transform.localScale")]
    public Vector2 targetScale = Vector2.one;

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
        tween = transform.DOScale(targetScale, duration);
        
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

        if (lookAt == LookAtSimple.None) 
            return;

        tween.onUpdate += OnTweenUpdated;
        tween.onComplete += OnTweenCompleted;
    }

    private void OnTweenUpdated()
    {
        if (lookAt == LookAtSimple.Position)
            transform.LookAt2DSmooth(lookAtPosition, offset, smoothFactor, min, max);
        else
            transform.LookAt2DSmooth(lookAtTarget, offset, smoothFactor, min, max);
    }

    private void OnTweenCompleted()
    {
        tween.onUpdate -= OnTweenUpdated;
        tween.onComplete -= OnTweenCompleted;
    }

    protected new void OnDestroy()
    {
        base.OnDestroy();

        lookAtTarget = null;
    }
}

}