using DG.Tweening;
using UnityEngine;
using DOTweenModular2D;

public class Test : MonoBehaviour
{
    [SerializeField] Vector2 center;
    [SerializeField] Ease ease;
    [SerializeField] float degree;
    [SerializeField] float duration;

    // Start is called before the first frame update
    void Start()
    {
        // RectTransform rectTransform = (RectTransform)transform;

        // rectTransform.DOShapeCircle(center, degree, duration).Play();
        transform.DOShapeCircle(center, degree, duration).Play();
        // transform.DOShapeCircleLocal(center, degree, duration, true).SetEase(ease).Play();
    }

}
