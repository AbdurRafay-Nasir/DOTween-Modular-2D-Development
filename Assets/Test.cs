using DG.Tweening;
using DOTweenModular2D.Miscellaneous;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SpriteRenderer sr;

    private void Start()
    {
        // Assuming you want to tween the width from its current value to a new width over 2 seconds
        float targetWidth = 5f; // Replace with your desired target width
        float duration = 2f;    // Replace with your desired tween duration

        sr.DOWidth(targetWidth, duration).SetEase(Ease.InOutBounce).Play();
    }
}
