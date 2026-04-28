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
        isOpen = !isOpen;

        if (isOpen)
        {
            // OPEN
            menuPanel.transform.position = menuButton.transform.position;

            menuAnimator.Play("Menu_Open");
            dimAnimator.Play("Dim_FadeIn");

            dimCanvasGroup.blocksRaycasts = true;
            menuButton.SetActive(false);
        }
        else
        {
            // CLOSE
            Resume();
        }
    }
    public void Resume()
    {
        isOpen = false;
        menuAnimator.Play("Menu_Close");
        dimAnimator.Play("Dim_FadeOut");

        dimCanvasGroup.blocksRaycasts = false;

        // Time.timeScale = 1f;
        menuButton.SetActive(true);
    }

    public void OpenMenu()
    {
        isOpen = true;

        menuPanel.transform.position = menuButton.transform.position;

        menuAnimator.Play("Menu_Open");
        dimAnimator.Play("Dim_FadeIn");

        dimCanvasGroup.blocksRaycasts = true;
        menuButton.SetActive(false);
    }

    public void CloseMenu()
    {
        isOpen = false;

        menuAnimator.Play("Menu_Close");
        dimAnimator.Play("Dim_FadeOut");

        dimCanvasGroup.blocksRaycasts = false;
        menuButton.SetActive(true);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}