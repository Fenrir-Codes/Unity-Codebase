using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{

    public IEnumerator Shake(float magnitude, float duration)
    {
        Vector3 originalPosition = transform.localPosition;

        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
