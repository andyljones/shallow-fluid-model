using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    /* 
     * Adaptation of the MouseOrbit.js script into C#. 
     * Should be minimally sufficient for orbiting and zooming parent
     */

    public float xSpeed = 0.1F;
    public float ySpeed = 0.1F;
    public float rSpeed = 20.0F;

    private float azi;
    private float pol;
    private float r;

    void Start()
    {
        var position = transform.position;
        r = position.magnitude;
        pol = Mathf.Acos(position.z / position.magnitude);
        azi = Mathf.Atan2(position.x, position.y);
    }

    void LateUpdate()
    {
        if (Input.GetButton("Fire2"))
        {
            azi = azi + Input.GetAxis("Mouse X") * xSpeed;
            pol = pol + Input.GetAxis("Mouse Y") * ySpeed;
        }

        r += Input.GetAxis("Mouse ScrollWheel") * rSpeed;

        var x = r * Mathf.Sin(azi)*Mathf.Sin(pol);
        var y = r*Mathf.Cos(azi)*Mathf.Sin(pol);
        var z = r*Mathf.Cos(pol);

        var position = new Vector3(x, y, z);
        var localEast = Vector3.Cross(position, new Vector3(0, 0, 1));
        var localNorth = Vector3.Cross(localEast, position);

        transform.rotation = Quaternion.LookRotation(-position, localNorth);
        transform.position = position;
    }
}