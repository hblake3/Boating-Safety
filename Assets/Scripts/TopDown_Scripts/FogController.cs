using System.Collections;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fog1;
    [SerializeField] private SpriteRenderer fog2;
    [SerializeField] private SpriteRenderer fog3;

    [SerializeField] private float timeToFade = 1.5f;

    private void Start()
    {
        SetFogAlpha(0f);

        fog1.gameObject.SetActive(false);
        fog2.gameObject.SetActive(false);
        fog3.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameController.onStartFog += StartFog;
    }

    private void OnDisable()
    {
        GameController.onStartFog -= StartFog;
    }

    private void StartFog()
    {
        fog1.gameObject.SetActive(true);
        fog2.gameObject.SetActive(true);
        fog3.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(FadeInFog());
    }

    private IEnumerator FadeInFog()
    {
        float elapsed = 0f;

        while (elapsed < timeToFade)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / timeToFade);
            SetFogAlpha(alpha);

            yield return null;
        }

        SetFogAlpha(1f);
    }

    private void SetFogAlpha(float alpha)
    {
        fog1.color = new Color(fog1.color.r, fog1.color.g, fog1.color.b, alpha);
        fog2.color = new Color(fog2.color.r, fog2.color.g, fog2.color.b, alpha);
        fog3.color = new Color(fog3.color.r, fog3.color.g, fog3.color.b, alpha);
    }
}