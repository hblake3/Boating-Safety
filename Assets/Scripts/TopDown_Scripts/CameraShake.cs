using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float defaultDuration = 0.3f;
    [SerializeField] float defaultMagnitude = 0.25f;
    [SerializeField] float frequency = 25f;   // how fast it oscillates

    Vector3 originalPos;
    Coroutine shakeRoutine;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        Shake(defaultDuration, defaultMagnitude);
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float percent = elapsed / duration;

            // fade out the shake over time
            float damper = 1f - percent;

            // smooth oscillation instead of random jitter
            float x = Mathf.Sin(elapsed * frequency) * magnitude * damper;
            float y = Mathf.Cos(elapsed * frequency * 0.8f) * magnitude * damper;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeRoutine = null;
    }
}