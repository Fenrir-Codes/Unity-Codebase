using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
    [SerializeField][Tooltip("Destroy delay in seconds")] private float destroyDelay = 20f;

    private void OnEnable()
    {
        Destroy(gameObject, destroyDelay);
    }
}
