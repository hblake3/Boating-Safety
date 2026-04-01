using UnityEngine;
using UnityEngine.UI;

public class InspectionHotspot : MonoBehaviour
{
    public bool isCorrect;
    public GameObject redXIcon;

    private Button button;
    private SafetyInspectionManager manager;

    void Awake()
    {
        button = GetComponent<Button>();
        manager = FindObjectOfType<SafetyInspectionManager>();
    }

    public void OnClicked()
    {
        if (isCorrect)
        {
            manager.CorrectSelected();
        }
        else
        {
            manager.WrongSelected(this);
        }
    }

    public void DisableHotspot()
    {
        button.interactable = false;
    }

    public void ShowRedX()
    {
        redXIcon.SetActive(true);
    }
}