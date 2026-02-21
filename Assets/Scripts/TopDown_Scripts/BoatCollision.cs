using System.Collections;
using UnityEngine;

public class BoatCollision : MonoBehaviour
{
    public delegate void OnBoatHit();
    public static OnBoatHit onBoatHit;

    [SerializeField] float invulnDuration = 1.2f;
    [SerializeField] float blinkInterval = 0.1f;

    bool isInvulnerable;
    Coroutine invulnRoutine;
    Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvulnerable)
            return;

        Debug.Log($"Boat hit {other.name}!");

        onBoatHit?.Invoke();

        CameraShake cam = Camera.main.GetComponent<CameraShake>();
        if (cam != null)
            cam.Shake();

        if (invulnRoutine != null)
            StopCoroutine(invulnRoutine);

        invulnRoutine = StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float timer = 0f;
        bool visible = true;

        while (timer < invulnDuration)
        {
            visible = !visible;

            foreach (var r in renderers)
                r.enabled = visible;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        foreach (var r in renderers)
            r.enabled = true;

        isInvulnerable = false;
        invulnRoutine = null;
    }


}