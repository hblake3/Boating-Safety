using UnityEngine;

public class MenuHUDController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject menuPanel;
    public GameObject dimBackground;
    public GameObject menuButton;

    private Animator menuAnimator;
    private Animator dimAnimator;

    private bool isOpen = false;

    void Start()
    {
        menuButton.SetActive(true); 
        menuAnimator = menuPanel.GetComponent<Animator>();
        dimAnimator = dimBackground.GetComponent<Animator>();
    }

    public void ToggleMenu()
    {
        menuPanel.transform.position = menuButton.transform.position;
        // Toggle the menu state
        isOpen = !isOpen;

        menuAnimator.Play("Menu_Open");
        dimAnimator.Play("Dim_FadeIn");

        menuButton.SetActive(false);
        // Pause or resume the game based on the menu state
        // Time.timeScale = isOpen ? 0f : 1f;
    }

    public void Resume()
    {
        isOpen = false;
        menuAnimator.Play("Menu_Close");
        dimAnimator.Play("Dim_FadeOut");

        // Time.timeScale = 1f;
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