using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform center;

    public Vector3 axis;
    public float speed;

    private void Update()
    {
        transform.RotateAround(center.position, axis, Time.deltaTime * speed);

        //you might not need the - in the beginning depending how your scene is setup
        var dir = -(center.position - transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //here you need to use the same axis you use in the rotate around method.
        //If you use .forward use it here as well.
        transform.rotation = Quaternion.AngleAxis(angle, axis);
    }

}
