using DG.Tweening;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SpriteRenderer sr;

    private void Start()
    {
        // Assuming you want to tween the width from its current value to a new width over 2 seconds
        float targetWidth = 5f; // Replace with your desired target width
        float duration = 2f;    // Replace with your desired tween duration

        // Use DOTween.To to tween the width property
        DOTween.To(() => sr.size.x, x => sr.size = new Vector2(x, sr.size.y), targetWidth, duration)
               .SetEase(Ease.InOutElastic) 
               .Play();
    }
}
