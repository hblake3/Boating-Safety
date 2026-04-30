using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHUDController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject menuPanel;
    public GameObject dimBackground;
    public GameObject menuButton;

    private CanvasGroup dimCanvasGroup;

    private Animator menuAnimator;
    private Animator dimAnimator;

    private bool isOpen = false;

    void Start()
    {
        menuButton.SetActive(true);
        menuAnimator = menuPanel.GetComponent<Animator>();
        dimAnimator = dimBackground.GetComponent<Animator>();
        dimCanvasGroup = dimBackground.GetComponent<CanvasGroup>();

        // Let UI animations keep running while the game is paused
        if (menuAnimator != null)
            menuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        if (dimAnimator != null)
            dimAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isOpen) CloseMenu();
            else OpenMenu();
        }
    }

    public void ToggleMenu()
    {
        if (isOpen)
            Resume();
        else
            OpenMenu();
    }

    public void Resume()
    {
        isOpen = false;

        menuAnimator.Play("Menu_Close");
        dimAnimator.Play("Dim_FadeOut");

        dimCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 1f;
        menuButton.SetActive(true);
    }

    public void OpenMenu()
    {
        isOpen = true;

        menuPanel.transform.position = menuButton.transform.position;

        menuAnimator.Play("Menu_Open");
        dimAnimator.Play("Dim_FadeIn");

        dimCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 0f;
        menuButton.SetActive(false);
    }

    public void CloseMenu()
    {
        isOpen = false;

        menuAnimator.Play("Menu_Close");
        dimAnimator.Play("Dim_FadeOut");

        dimCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 1f;
        menuButton.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}