using DG.Tweening;

namespace DOTweenModular2D
{
    public class DOShakePosition : DOShakeBase
    {
        public UnityEngine.Vector2 strength;
        public bool snapping;

        protected override void InitializeTween()
        {
            tween = transform.DOShakePosition(duration, strength, vibrato, randomness, 
                                              snapping, fadeOut, randomnessMode);
        }
    }
}

