using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup howToPlayGroup;

    void Start()
    {
        howToPlayGroup.alpha = 0;
        howToPlayGroup.gameObject.SetActive(false);
    }

    public void OnPlayPressed()
    {
        howToPlayGroup.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void OnOKPressed()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator FadeIn()
    {
        while (howToPlayGroup.alpha < 1)
        {
            howToPlayGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }
    }

    IEnumerator LoadGame()
    {
        while (howToPlayGroup.alpha > 0)
        {
            howToPlayGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        SceneManager.LoadScene(1);
    }
}