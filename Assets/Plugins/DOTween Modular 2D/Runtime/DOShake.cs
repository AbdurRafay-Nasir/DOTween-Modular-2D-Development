namespace DOTweenModular2D
{

using DG.Tweening;
using DOTweenModular2D.Enums;
using UnityEngine;

[AddComponentMenu("DOTween Modular 2D/DO Shake", 5)]
public class DOShake : DOBase
{
    [Tooltip("At what, the Shake will apply to")]
    public ApplyTo applyTo;

    [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
    public bool snapping;

    [Tooltip("If TRUE, tween will smoothly reach back to start Position/Rotation/Scale")]
    public bool fadeOut = true;

    [Tooltip("Indicates how much will the shake vibrate")]
    public int vibrato = 10;

    [Tooltip("Indicates how much the shake will be random (values higher than 90 kind of suck, so beware) " + 
             "Setting it to 0 will shake along a single direction.")]
    [Range(0f, 180f)]
    public float randomness = 90f;

    [Tooltip("Full - Full randomness " + "\n" + 
             "Harmonic - Creates a more balanced randomness that looks more harmonic")]
    public ShakeRandomnessMode randomnessMode;

    [Tooltip("Strength Amount")]
    public Vector2 strength;

    [Tooltip("Strength Amount")]
    public float strengthAmount;

    public override void CreateTween()
    {
        switch (applyTo)
        {
            case ApplyTo.Position:
                tween = transform.DOShakePosition(duration, strength, vibrato, randomness, snapping,                                          
                                                   fadeOut, randomnessMode);
            break;

            case ApplyTo.Rotation:
                tween = transform.DOShakeRotation(duration, Vector3.forward * strengthAmount, vibrato, randomness, 
                                                   fadeOut, randomnessMode); 
            break;

            case ApplyTo.Scale:
                tween = transform.DOShakeScale(duration, strength, vibrato, randomness,                                        
                                                fadeOut, randomnessMode);

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