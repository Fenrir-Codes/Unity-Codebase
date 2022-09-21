using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class RagdollInstantiatorNew : MonoBehaviour
{
    public GameObject deadReplacement;
    public void Die()
    {
        // Replace ourselves with the dead body
        GameObject dead = null;
        if (deadReplacement)
        {
            // Create the dead body
            dead = Instantiate(deadReplacement, transform.position, transform.rotation);
            Vector3 vel = Vector3.zero;
            if (GetComponent<Rigidbody>())
            {
                vel = GetComponent<Rigidbody>().velocity;
            }
            else
            {
                CharacterController cc = GetComponent<CharacterController>();
                vel = cc.velocity;
            }
            // Copy position & rotation from the old hierarchy into the dead replacement
            CopyTransformsRecurse(transform, dead.transform, vel);
            gameObject.SetActive(false);
        }
    }

    public void CopyTransformsRecurse(Transform src, Transform dst, Vector3 velocity)
    {
        Rigidbody body = dst.GetComponent<Rigidbody>();
        if (body != null)
        {
            body.velocity = velocity;
            body.useGravity = true;
        }
        dst.position = src.position;
        dst.rotation = src.rotation;
        foreach (Transform child in dst)
        {
            // Match the transform with the same name
            Transform curSrc = src.Find(child.name);
            if (curSrc)
            {
                CopyTransformsRecurse(curSrc, child, velocity);
            }
        }
    }

}