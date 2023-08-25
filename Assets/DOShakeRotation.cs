using DG.Tweening;

namespace DOTweenModular2D
{
    public class DOShakeRotation : DOShakeBase
    {
        public float strength;

        protected override void InitializeTween()
        {
            tween = transform.DOShakeRotation(duration, UnityEngine.Vector3.forward * strength, vibrato, 
                                              randomness, fadeOut, randomnessMode);
        }
    }
}
