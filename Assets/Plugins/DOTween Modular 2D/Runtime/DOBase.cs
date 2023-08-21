namespace DOTweenModular2D
{

using DG.Tweening;
using DOTweenModular2D.Enums;
using UnityEngine;
using UnityEngine.Events;

public abstract class DOBase : MonoBehaviour
{
    [Tooltip("When this tween should start")]
    public Begin begin;

    [Tooltip("The DO component After/With this tween will start")]
    public DOBase tweenObject;

    [Tooltip("When To kill this tween")]
    public Kill kill;

    [Tooltip("If TRUE, destroys the component when tween is killed")]
    public bool destroyComponent;

    [Tooltip("If TRUE, destroys the Game Object when tween is killed")]
    public bool destroyGameObject;

    [Tooltip("Time after which this tween will play")]
    public float delay;

    public Enums.TweenType tweenType;

    [Tooltip("Restart - Start again from start Position/Rotation/Scale " + "\n" +
             "Yoyo - Start from Target Position/Rotation/Scale")]
    public LoopType loopType;

    [Tooltip("Ease to apply, for custom ease select INTERNAL_Custom. Do not assign INTERNAL_Zero")]
    public Ease easeType;
    public AnimationCurve curve;

    [Tooltip("Number of loops, -1 for infinite loops " + "\n" +
             "For Yoyo Loop Type the backward movement will also be counted")]
    [Min(-1)] public int loops = -1;

    [Tooltip("How long this tween will play")]
    [Min(0)] public float duration = 1;

    // Events
    /// <summary>
    /// Called when this tween is created
    /// </summary>
    public UnityEvent<DOBase> onTweenCreated;
    
    /// <summary>
    /// Called when this tween completes, in-case of infinite loops this will not invoke
    /// </summary>
    public UnityEvent<DOBase> onTweenCompleted;

    /// <summary>
    /// Called when this tween is Killed, in-case of infinite loops this will not invoke
    /// </summary>
    public UnityEvent<DOBase> onTweenKilled;

    /// <summary>
    /// Must assign this to custom tween that you create 
    /// </summary>
    protected Tween tween;
    public Tween Tween { get { return tween; }}

    private bool visible;
    private bool enteredTrigger;

    private void Awake() 
    {
        if (begin == Begin.Manual) return;

        if (begin == Begin.After)
            tweenObject.onTweenCompleted.AddListener(OnStartAfterTweenCompleted);

        else if (begin == Begin.With)
            tweenObject.onTweenCreated.AddListener(OnStartWithTweenCreated);     
    }

    private void Start() 
    {
        if (begin == Begin.Manual) return;
        if (begin == Begin.After) return;
        if (begin == Begin.With) return;

        CreateTween();

        if (begin != Begin.OnSceneStart) return;

        tween.Play();
    }

    private void OnBecameVisible() 
    {
        if (begin != Begin.OnVisible) return;
        if (visible) return;

        visible = true;
        tween.Play();
    }

    private void OnBecameInvisible()
    {
        if (kill != Kill.OnInvisible) return;
        if (tween == null) return;
        if (!tween.playedOnce) return;

        onTweenKilled?.Invoke(this);
        ClearTweenCallbacks();
        tween.Kill();
        tween = null;

        if (destroyComponent) Destroy(this);
        if (destroyGameObject) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (begin != Begin.OnTrigger) return;
        if (enteredTrigger) return;

        enteredTrigger = true;
        tween.Play();
    }

    protected void OnDestroy()
    {
        if (tween != null)
        {
            onTweenKilled?.Invoke(this);
            ClearTweenCallbacks();
            tween.Kill();
            tween = null;
        }

        tweenObject = null;
        
        onTweenCreated?.RemoveAllListeners();
        onTweenCompleted?.RemoveAllListeners();
    }

    /// <summary>
    /// Implement this method for your custom tween  
    /// </summary>
    public abstract void CreateTween();

    /// <summary>
    /// Must call this after creating custom tween
    /// </summary>
    protected void InvokeTweenCreated()
    {
        onTweenCreated.AddListener(OnTweenCreated);
        onTweenCreated?.Invoke(this);
    }

    private void OnTweenCreated(DOBase doBase)
    {
        tween.onComplete += OnTweenCompleted;
        onTweenCreated.RemoveListener(OnTweenCreated);
    }

    private void OnTweenCompleted()
    {
        onTweenCompleted?.Invoke(this);
        tween.onComplete -= OnTweenCompleted;

        if (kill == Kill.OnTweenComplete)
        {
            onTweenKilled?.Invoke(this);
            ClearTweenCallbacks();
            tween.Kill();
            tween = null;

            if (destroyComponent) Destroy(this);
            if (destroyGameObject) Destroy(gameObject);
        }
    }

    private void OnStartWithTweenCreated(DOBase doBase)
    {
        CreateTween();
        tween.Play();

        tweenObject.onTweenCreated.RemoveListener(OnStartWithTweenCreated);
    }

    private void OnStartAfterTweenCompleted(DOBase doBase)
    {
        CreateTween();
        tween.Play();

        tweenObject.onTweenCompleted.RemoveListener(OnStartAfterTweenCompleted);
    }

    private void ClearTweenCallbacks()
    {
        Tween.OnComplete(null);
        Tween.OnKill(null);
        Tween.OnPause(null);
        Tween.OnPlay(null);
        Tween.OnRewind(null);
        Tween.OnStart(null);
        Tween.OnStepComplete(null);
        Tween.OnUpdate(null);
        Tween.OnWaypointChange(null);
    }

}

}