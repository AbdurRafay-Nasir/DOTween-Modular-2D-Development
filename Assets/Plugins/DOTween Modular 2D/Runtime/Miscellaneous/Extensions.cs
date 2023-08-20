namespace DOTweenModular2D
{

using UnityEngine;

public static class Extensions
{
    
#region LookAt2D Functions

    /// <summary>
    /// Rotate on Z-Axis to Look At target
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
    public static void LookAt2D(this Transform transform, Transform target, float offset)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
    }

    /// <summary>
    /// Rotate on Z-Axis to Look At target, also Clamps the rotation 
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
    /// <param name="min">Minimum Rotation</param>
    /// <param name="max">Maximum Rotation</param>
    /// <remarks>If target is moved above and right of this transform then rotation will snap to 'max'</remarks>
    public static void LookAt2D(this Transform transform, Transform target, float offset, float min, float max)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);

        Vector3 localEulerAngle = transform.localEulerAngles;
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, min, max);
        transform.localEulerAngles = localEulerAngle;
    }

    /// <summary>
    /// Rotate on Z-Axis to Look At target
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
    public static void LookAt2D(this Transform transform, Vector2 target, float offset)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
    }

    /// <summary>
    /// Rotate on Z-Axis to Look At target, also Clamps the rotation 
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
    /// <param name="min">Minimum Rotation</param>
    /// <param name="max">Maximum Rotation</param>
    /// <remarks>If target is moved above and right of this transform then rotation will snap to 'max'</remarks>
    public static void LookAt2D(this Transform transform, Vector2 target, float offset, float min, float max)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);

        Vector3 localEulerAngle = transform.localEulerAngles;
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, min, max);
        transform.localEulerAngles = localEulerAngle;
    }

#endregion

#region LookAt2D Smooth Functions

    /// <summary>
    /// Rotate on Z-Axis to Look At target
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look directly At the target</param>
    /// <param name="interpolate">How smoothly the Game Object will rotate to Look At target, 1 means no smoothness</param>
    public static void LookAt2DSmooth(this Transform transform, Vector2 target, float offset, float interpolate)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotGoal = Quaternion.Euler(0f, 0f, angle + offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, interpolate);
    }

    /// <summary>
    /// Rotate on Z-Axis to Look At target
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look At the target</param>
    /// <param name="interpolate">How smoothly the Game Object will rotate to Look At target, 1 means no smoothness</param>
    /// <param name="min">Minimum Rotation</param>
    /// <param name="max">Maximum Rotation</param>
    /// <remarks>If target is moved above and right of this transform then rotation will snap to 'max'</remarks>
    public static void LookAt2DSmooth(this Transform transform, Vector2 target, float offset, float interpolate, float min, float max)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, interpolate);

        Vector3 localEulerAngle = transform.localEulerAngles;
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, min, max);
        transform.localEulerAngles = localEulerAngle;
    }

    /// <summary>
    /// Rotate on Z-Axis to Look At target
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look At the target</param>
    /// <param name="interpolate">How smoothly the Game Object will rotate to Look At target, 1 means no smoothness</param>
    public static void LookAt2DSmooth(this Transform transform, Transform target, float offset, float interpolate)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, interpolate);
    }

    /// <summary>
    /// Rotate on Z-Axis to Look At target
    /// </summary>
    /// <param name="target">The target to look at</param>
    /// <param name="offset">Offset to add in Z Rotation, -90 makes the game Object Look At the target</param>
    /// <param name="interpolate">How smoothly the Game Object will rotate to Look At target, 1 means no smoothness</param>
    /// <param name="min">Minimum Rotation</param>
    /// <param name="max">Maximum Rotation</param>
    /// <remarks>If target is moved above and right of this transform then rotation will snap to 'max'</remarks>
    public static void LookAt2DSmooth(this Transform transform, Transform target, float offset, float interpolate, float min, float max)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, interpolate);

        Vector3 localEulerAngle = transform.localEulerAngles;
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, min, max);
        transform.localEulerAngles = localEulerAngle;
    }

#endregion

#region Vector Functions

    /// <summary>
    /// Convert this array to Vector3 Array
    /// </summary>
    /// <returns>Copy of array with Z value = 0</returns>
    public static Vector3[] ToVector3Array(this Vector2[] vector2Array)
    {
        Vector3[] vector3Array = new Vector3[vector2Array.Length];
        Vector3 xyOne = new(1f, 1f, 0f);

        for (int i = 0; i < vector3Array.Length; i++)
        {
            vector3Array[i] = xyOne * vector2Array[i];
        }

        return vector3Array;
    }

    /// <summary>
    /// Convert this array to Vector2 Array
    /// </summary>
    /// <returns>Copy of array with Z axis dropped</returns>
    public static Vector2[] ToVector2Array(this Vector3[] vector3Array)
    {
        Vector2[] vector2Array = new Vector2[vector3Array.Length];

        for (int i = 0; i < vector2Array.Length; i++)
        {
            vector2Array[i] = vector3Array[i];
        }

        return vector2Array;
    }

#if UNITY_EDITOR

    /// <summary>
    /// Print elements of array. Useful for debugging, only available in Editor
    /// </summary>
    public static void Print(this Vector3[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log("Index: " + i + " = " + arr[i]);
        }
    }

    /// <summary>
    /// Print elements of array. Useful for debugging, only available in Editor
    /// </summary>
    public static void Print(this Vector2[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log("Index: " + i + " = " + arr[i]);
        }
    }

#endif

#endregion

}

}