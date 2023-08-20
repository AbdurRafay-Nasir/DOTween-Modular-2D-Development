namespace DOTweenModular2D
{

using DOTweenModular2D.Enums;
using DG.Tweening;
using UnityEngine;

[AddComponentMenu("DOTween Modular 2D/DO LookAt", 7)]
public class DOLookAt : DOBase
{
    [Tooltip("Type of Look At")]
    public LookAtSimple lookAt;

    [Tooltip("The game Object to Look At")]
    public Transform lookAtTarget;

    [Tooltip("The position to Look At")]
    public Vector2 lookAtPosition;

    [Tooltip("The offet to add to rotation, value of -90 means the game object will look directly towards lookAtPosition/lookAtTarget")]
    public float offset = -90f;

    [Tooltip("Minimum Rotation, Set min to 0 and max to 360 for no rotation clamp")]
    [Range(0f, 360f)] public float min = 90f;

    [Tooltip("Maximum Rotation, Set min to 0 and max to 360 for no rotation clamp")]
    [Range(0f, 360f)] public float max = 270f;

    [Tooltip("Smoothness of rotation, 1 means there will be no smoothness")]
    [Range(0f, 1f)] public float smoothFactor = 0.01f;

    // Just to make sure the tween runs
    private float ghostValue = 0f;

    public override void CreateTween()
    {
        tween = DOTween.To(() => ghostValue, x => ghostValue = x, 50f, duration);

        tween.SetDelay(delay);

        InvokeTweenCreated();

        if (lookAt == LookAtSimple.None) return;

        tween.onUpdate += OnTweenUpdate;
        tween.onComplete += OnTweenCompleted;
    }

    private void OnTweenUpdate()
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

    private void OnTweenCompleted()
    {
        tween.onUpdate -= OnTweenUpdate;
        tween.onComplete -= OnTweenCompleted;
    }

    protected new void OnDestroy()
    {
        base.OnDestroy();

        lookAtTarget = null;
    }
}

}