using DG.Tweening;
using DOTweenModular2D.Miscellaneous;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SpriteRenderer sr;

    private void Start()
    {
        // Assuming you want to tween the width from its current value to a new width over 2 seconds
        Vector2 targetWidth = Vector2.one * 5f; // Replace with your desired target width
        float duration = 0.5f;    // Replace with your desired tween duration

        // sr.DOWidth(targetWidth, duration).SetEase(Ease.InOutBounce).Play();
        sr.DOSize(targetWidth, duration).SetEase(Ease.InOutElastic).SetSpeedBased(true).Play();
    }
}
