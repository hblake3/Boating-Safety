using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GearItem : MonoBehaviour
{
    public string itemName;
    public bool isCorrect;

    [HideInInspector] public bool alreadyChosen = false;

    private GearSelectionManager manager;

    // Visual / UI members
    private Button button;
    private bool lockedSelected = false;

    void Start()
    {
        manager = Object.FindFirstObjectByType<GearSelectionManager>();

    }

    public void OnItemClicked()
    {
        if (alreadyChosen) return;

        manager.SelectItem(this);
    }

    // Called by manager to set or clear the persistent selection visual.
    public void SetSelected(bool selected)
    {
        lockedSelected = selected;

        // Keep Unity's navigation selection for keyboard/gamepad by selecting the button.
        if (selected && button != null)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    // Called by manager to clear any visuals when the item is permanently marked/disabled.
    public void ResetVisuals()
    {
        lockedSelected = false;
    }

    void OnDisable()
    {
        // Ensure visuals reset when the object is disabled
        ResetVisuals();
    }
}