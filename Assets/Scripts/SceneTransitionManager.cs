using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 0.5f;
    
    public static SceneTransitionManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this method to transition to a new scene with fade effect: SceneTransitionManager.Instance.TransitionToScene("SceneName");
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        // Enable blocking BEFORE fade out
        fadeCanvas.blocksRaycasts = true;
        fadeCanvas.interactable = true;

        // Fade OUT
        yield return StartCoroutine(Fade(0f, 1f));

        SceneManager.LoadScene(sceneName);

        yield return null;

        // Fade IN
        yield return StartCoroutine(Fade(1f, 0f));

        // Disable blocking AFTER fade in
        fadeCanvas.blocksRaycasts = false;
        fadeCanvas.interactable = false;
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            fadeCanvas.alpha = Mathf.Lerp(start, end, t);

            time += Time.deltaTime;
            yield return null;
        }

        fadeCanvas.alpha = end;
    }
}