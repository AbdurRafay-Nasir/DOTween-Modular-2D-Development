using DG.Tweening;
using UnityEngine;
using UnityEditor;
using DOTweenModular2D;

public class Tester : MonoBehaviour
{
    public LayerMask layerMask;
    // [SerializeField] private int layer = 10;
    // private int layerAsLayerMask;

    private void Update()
    {
        // layerAsLayerMask = 1 << layer;
        
        Debug.Log(LayerMask.LayerToName(layerMask));
    }
}


// [CustomEditor(typeof(Tester))]
// public class TesterEditor : Editor
// {
//     Tester t;
//     private void OnEnable()
//     {
//         t = (Tester) target;
//     }
//     private void OnSceneGUI()
//     {
//         Handles.color = Color.green;
//         Vector3 position = t.transform.position;

//         // Calculate the endpoints of the arc based on the min and max angles
//         float minAngle = (t.min + 90) * Mathf.Deg2Rad;
//         float maxAngle = (t.max + 90) * Mathf.Deg2Rad;
//         Vector3 minDir = new Vector3(Mathf.Cos(minAngle), Mathf.Sin(minAngle), 0);
//         Vector3 maxDir = new Vector3(Mathf.Cos(maxAngle), Mathf.Sin(maxAngle), 0);

//         // Draw the circle representing the range
//         Handles.DrawWireArc(position, Vector3.forward, minDir, t.max - t.min, t.radius);

//         // Draw lines from the center to the min and max angles
//         Handles.DrawLine(position, position + minDir * t.radius);
//         Handles.DrawLine(position, position + maxDir * t.radius);
//     }

// }
