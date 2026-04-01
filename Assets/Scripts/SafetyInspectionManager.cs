using UnityEngine;
using UnityEngine.UI;

public class SafetyInspectionManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText;
    [SerializeField] private Button nextArrowButton;

    void Start()
    {
        nextArrowButton.gameObject.SetActive(false);
        dialogueText.text =
        "First, I need to make sure all straps are tightened. Where can I find the life jacket's straps?";
    }

    public void WrongSelected(InspectionHotspot hotspot)
    {
        dialogueText.text =
        "Uh oh, that doesn't look right. Try again.";

        hotspot.ShowRedX();
        hotspot.DisableHotspot();
    }

    public void CorrectSelected()
    {
        dialogueText.text =
        "Great job! Always make sure all straps on the lifejacket are tightened and snug!";

        nextArrowButton.gameObject.SetActive(true);
    }

    public void OnNextPressed()
    {
        Debug.Log("Move to next inspection step");
        // trigger the next inspection question here
    }
}