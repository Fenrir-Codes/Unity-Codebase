using UnityEngine;
using System.Collections;

public class RotatetWheel : MonoBehaviour
{
    public float maxSteeringAngle;
    public float steering;
    public CarController carcontroller;

    void Update()
    {
        steering = carcontroller.steering;
        transform.localRotation = Quaternion.Euler(-maxSteeringAngle * steering, 0, 0);
    }

}