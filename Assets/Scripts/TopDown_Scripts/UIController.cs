using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;

    private void OnEnable()
    {
        // Subscribe to onSpeedChanged from BoatController (awaits for command that speed has been changed)
        BoatController.onSpeedChanged += UpdateSpeedUIText;
    }

    private void OnDisable()
    {
        // Unubscribe to onSpeedChanged from BoatController
        BoatController.onSpeedChanged -= UpdateSpeedUIText;
    }


    public void UpdateSpeedUIText(float speed)
    {
        string displaySpeed;

        if (speed == 2.5f)
            displaySpeed = "10";
        else if (speed == 5.0f)
            displaySpeed = "25";
        else if (speed == 7.5f)
            displaySpeed = "40";
        else
            displaySpeed = speed.ToString(); // fallback just in case

        speedText.text = $"{displaySpeed}\nMPH";
    }

}
