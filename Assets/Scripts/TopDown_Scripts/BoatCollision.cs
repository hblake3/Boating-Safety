using System.Collections;
using UnityEngine;

public class BoatCollision : MonoBehaviour
{
    public delegate void OnBoatHit();
    public static OnBoatHit onBoatHit;

    [SerializeField] float invulnDuration = 1.2f;
    [SerializeField] float blinkInterval = 0.1f;
    [SerializeField] string hurtTag = "CanHurtBoat";

    bool isInvulnerable;
    Coroutine invulnRoutine;
    Renderer[] renderers;

    void Awake()
    {
        // holds all renderers of the boat, so the blink effect can toggle the whole boat.
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvulnerable)
            return;

        // Only tagged hazards should cause damage.
        if (!other.CompareTag(hurtTag))
            return;

        Debug.Log($"Boat hit {other.name}!");

        onBoatHit?.Invoke();

        CameraShake cam = Camera.main.GetComponent<CameraShake>();
        if (cam != null)
        {
            cam.Shake();
            // play collision sound
            UIAudioManager.Instance.PlayBoatHit();
        }

        if (invulnRoutine != null) // if the boat was already in the process of blinking from a previous hit, stop that and start a new one.
            StopCoroutine(invulnRoutine);

        invulnRoutine = StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float timer = 0f;
        bool visible = true;

        // briefly blink the boat so the player knows they were hit and can't be damaged again immediately.
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