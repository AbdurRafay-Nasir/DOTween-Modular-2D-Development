namespace DOTweenModular2D
{
using DOTweenModular2D.Enums;
using DG.Tweening;
using UnityEngine;

[AddComponentMenu("DOTween Modular 2D/DO Punch", 4)]
public class DOPunch : DOBase
{
    [Tooltip("At what the Punch will apply to")]
    public ApplyTo applyTo;

    [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
    public bool snapping;

    [Tooltip("Indicates how much will the punch vibrate")]
    public int vibrato = 10;

    [Tooltip("Represents how much the vector will go beyond the starting position when bouncing" + "\n" +
             "backwards. 1 creates a full oscillation between the punch direction " + "\n" +
             "and the opposite direction, while 0 oscillates only between the punch and the start position")]
    [Range(0f, 1f)]
    public float elasticity = 1;

    [Tooltip("Punch Amount")]
    public Vector2 punch;

    [Tooltip("Punch Amount")]
    public float punchAmount;

    public override void CreateTween()
    {
        switch (applyTo)
        {
            case ApplyTo.Position:
                tween = transform.DOPunchPosition(punch, duration, vibrato, elasticity, snapping);
            break;

            case ApplyTo.Rotation:
                tween = transform.DOPunchRotation(Vector3.forward * punchAmount, duration,vibrato, elasticity);
            break;

            case ApplyTo.Scale:
                tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);;
            break;
        }

        if (easeType == Ease.INTERNAL_Custom)
        {
            tween.SetEase(curve);
        }
        else
        {
            tween.SetEase(easeType);
        }

        if (tweenType == Enums.TweenType.Looped)
            tween.SetLoops(loops, loopType);

        tween.SetDelay(delay);

        InvokeTweenCreated();
    }
}

}