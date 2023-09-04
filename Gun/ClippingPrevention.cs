using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingPrevention : MonoBehaviour
{
    [SerializeField] private Transform clipProjector;
    [SerializeField] private float checkDistance;
    public Vector3 newDirection;

    RaycastHit hit;
    float lerpPos;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(clipProjector.position, clipProjector.forward, Color.red);
        if (Physics.Raycast(clipProjector.position, clipProjector.forward, out hit, checkDistance))
        {
            //get a percentage from 0 to max distance
            lerpPos = 1 - (hit.distance / checkDistance);
            
        }
        else
        {
            //if we are not hitting anything, set to 0
            lerpPos = 0;
        }

        Mathf.Clamp01(lerpPos);
        transform.localRotation =
            Quaternion.Lerp(
            Quaternion.Euler(Vector3.zero),
            Quaternion.Euler(newDirection),
            lerpPos);
    }
}
