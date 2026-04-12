using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class InactivityManager : MonoBehaviour
{
    [Header("Timers")]
    public float warningTime = 60f;
    public float timeoutTime = 120f;

    private float timer = 0f;

    [Header("UI")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    private bool warningShown = false;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;

        timer += Time.deltaTime;

        DetectInput();

        // Show warning at 60 seconds
        if (timer >= warningTime && !warningShown)
        {
            ShowWarning();
        }

        if (warningShown)
        {
            UpdateCountdownText();
        }

        // Return to main menu at 120 seconds
        if (timer >= timeoutTime)
        {
            GoToMainMenu();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTimer();

        // Only look for UI in gameplay scene
        if (scene.name != "MainMenu")
        {
            Debug.Log("Scene loaded: " + scene.name + ", looking for UI references...");
            ReassignUIReferences();
        }
    }

    void ReassignUIReferences()
    {
        warningPanel = GameObject.Find("WarningPanel");

        if (warningPanel != null)
        {
            warningText = warningPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            warningPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Warning Panel not found in scene!");
        }
    }

    public void SetUIReferences(GameObject panel, TextMeshProUGUI text)
    {
        warningPanel = panel;
        warningText = text;

        warningPanel.SetActive(false);
    }

    void UpdateCountdownText()
    {

        if (warningText == null) return; // prevent crash

        float remainingTime = timeoutTime - timer;

        // Clamp so it never goes negative
        remainingTime = Mathf.Clamp(remainingTime, 0, timeoutTime);

        int seconds = Mathf.FloorToInt(remainingTime);

        if (remainingTime <= 10f)
        {
            // Show seconds in red when 10 seconds or less remain
            warningText.text = "Are you still there?\nReturning to main menu in <color=red>" + seconds + "</color> seconds...";

        }
        else
        {
            warningText.text = "Are you still there?\nReturning to main menu in " + seconds + " seconds...";
        }
    }

    // Old input detection using legacy Input system - errors due to Unity Input System mismatch.
    // We switched to the new Input System, but code is still using the old UnityEngine.Input API
    // Can be fixed if Edit > Project Settings > Player > Other Settings > Configuration > Active Input Handling is set to "Both"
    //void DetectInput()
    //{
    //    if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
    //    {
    //        ResetTimer();
    //    }
    //}

    void DetectInput()
    {
        if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame))
        {
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = 0f;

        if (warningShown)
        {
            HideWarning();
        }
    }

    void ShowWarning()
    {
        warningShown = true;
        warningPanel.SetActive(true);

        // play warning sound here

        UpdateCountdownText(); // Update text immediately when showing the warning
    }

    void HideWarning()
    {
        warningShown = false;
        warningPanel.SetActive(false);
    }

    void GoToMainMenu()
    {
        timer = 0f;
        warningShown = false;
        SceneManager.LoadScene("MainMenu"); // replace with your scene name
    }
}