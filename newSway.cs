using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newSway : MonoBehaviour
{
    public float amount = 0.02f;
    public float maxAmount = 0.03f;
    public float smooth = 3;
    public float smoothRotation = 2;
    public float tiltAngle = 25;
    private Vector3 dif;

    // Use this for initialization
    void Start()
    {
        dif = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

   void  TiltSway() 
    {
        float tiltAroundZ = Input.GetAxis("Mouse X") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Mouse Y") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smoothRotation);
    }
}
