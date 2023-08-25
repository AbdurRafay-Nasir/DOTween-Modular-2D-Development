using DG.Tweening;

namespace DOTweenModular2D
{ 
    public class DOShakeScale : DOShakeBase
    {
        public UnityEngine.Vector2 strength;

        protected override void InitializeTween()
        {
            tween = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut, randomnessMode);
        }
    }
}
