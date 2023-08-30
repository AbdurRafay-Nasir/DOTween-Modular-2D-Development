using DOTweenModular2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private DOBase obj;

    private void Awake()
    {
        obj = GetComponent<DOBase>();
        obj.onTweenCreated.AddListener(Created);
        obj.onTweenStarted.AddListener(Started);
        obj.onTweenCompleted.AddListener(Completed);
        obj.onTweenKilled.AddListener(Killed);
    }
    
    private void Created()
    {
        Debug.Log(obj.GetInstanceID() + " Created");
    }
    private void Started()
    {
        Debug.Log(obj.GetInstanceID() + " Started");
    }
    private void Completed()
    {
        Debug.Log(obj.GetInstanceID() + " Completed");
    }
    private void Killed()
    {
        Debug.Log(obj.GetInstanceID() + " Killed");
    }
}
