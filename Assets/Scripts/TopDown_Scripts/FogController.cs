using System.Collections;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fog1;
    [SerializeField] private SpriteRenderer fog2;
    [SerializeField] private SpriteRenderer fog3;
    [SerializeField] private float timeToFade = 1.5f;

    public delegate void OnFogCleared();
    public static event OnFogCleared onFogCleared;

    private void Start()
    {
        // start with the fog hidden (alpha = 0)
        SetFogAlpha(0f);

        fog1.gameObject.SetActive(false);
        fog2.gameObject.SetActive(false);
        fog3.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameController.onStartFog += StartFog;
        GameController.onClearFog += ClearFog;
    }

    private void OnDisable()
    {
        GameController.onStartFog -= StartFog;
        GameController.onClearFog -= ClearFog;
    }

    private void StartFog()
    {
        fog1.gameObject.SetActive(true);
        fog2.gameObject.SetActive(true);
        fog3.gameObject.SetActive(true);

        // reset any current fade before starting the fog
        StopAllCoroutines();
        StartCoroutine(FadeInFog());
    }

    private void ClearFog()
    {
        // switch to fading out even if fog is still fading in
        StopAllCoroutines();
        StartCoroutine(FadeOutFog());
    }

    private IEnumerator FadeInFog()
    {
        float elapsed = 0f;
        float targetAlpha = 0.50f;

        while (elapsed < timeToFade)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / timeToFade) * targetAlpha;
            SetFogAlpha(alpha);
            yield return null;
        }

        SetFogAlpha(targetAlpha);
    }

    private IEnumerator FadeOutFog()
    {
        float elapsed = 0f;

        while (elapsed < timeToFade)
        {
            elapsed += Time.deltaTime;
            float alpha = (1f - Mathf.Clamp01(elapsed / timeToFade)) * 0.50f;
            SetFogAlpha(alpha);
            yield return null;
        }

        SetFogAlpha(0f);

        fog1.gameObject.SetActive(false);
        fog2.gameObject.SetActive(false);
        fog3.gameObject.SetActive(false);

        // let the game controller know the screen is clear again
        onFogCleared?.Invoke();
    }

    private void SetFogAlpha(float alpha)
    {
        // Apply the same alpha to each fog layer
        fog1.color = new Color(fog1.color.r, fog1.color.g, fog1.color.b, alpha);
        fog2.color = new Color(fog2.color.r, fog2.color.g, fog2.color.b, alpha);
        fog3.color = new Color(fog3.color.r, fog3.color.g, fog3.color.b, alpha);
    }
}