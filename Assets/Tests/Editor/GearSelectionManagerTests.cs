using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectionManagerTests
{
    private GameObject managerObject;
    private GearSelectionManager manager;

    [SetUp]
    public void SetUp()
    {
        managerObject = new GameObject("GearSelectionManager");
        manager = managerObject.AddComponent<GearSelectionManager>();

        manager.selectedItemTitle = CreateText("Title");
        manager.selectedItemDetails = CreateText("Details");
        manager.feedbackText = CreateText("Feedback");

        manager.confirmButton = CreateButton("ConfirmButton");
        manager.gearPanel = CreatePanel("GearPanel");
        manager.descriptionPanel = CreatePanel("DescriptionPanel");

        manager.ghostPanel = new GameObject("GhostPanel");
        manager.ghostPanel2 = new GameObject("GhostPanel2");
        manager.buddyPortrait = new GameObject("BuddyPortrait");
        manager.barryPortrait = new GameObject("BarryPortrait");

        manager.speechBubble = new GameObject("SpeechBubble");
        manager.bubbleText = CreateText("BubbleText");
        manager.continueButton = CreateButton("ContinueButton");

        manager.speechBubble.SetActive(false);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(managerObject);
    }

    [Test]
    public void CanCreateGearSelectionManager()
    {
        // Tests that the GearSelectionManager component can be successfully created.
        Assert.IsNotNull(manager);
    }

    [Test]
    public void SelectItem_UpdatesTitleAndDetails()
    {
        // Tests that selecting an item updates the UI text and enables the confirm button.
        GearItem item = CreateGearItem("Life Jacket", true);

        manager.SelectItem(item);

        Assert.AreEqual("Life Jacket", manager.selectedItemTitle.text);
        Assert.AreEqual("An extra life jacket, just incase.", manager.selectedItemDetails.text);
        Assert.IsTrue(manager.confirmButton.interactable);
    }

    [Test]
    public void ConfirmSelection_CorrectItem_ShowsPositiveFeedbackAndMarksChosen()
    {
        // Tests that confirming a correct item gives positive feedback and disables reuse.
        GearItem item = CreateGearItem("Life Jacket", true);

        manager.SelectItem(item);
        manager.ConfirmSelection();

        Assert.AreEqual("Correct! Life jackets help keep everyone safe in the water.", manager.feedbackText.text);
        Assert.IsTrue(item.alreadyChosen);
        Assert.IsFalse(item.GetComponent<Button>().interactable);
    }

    private TextMeshProUGUI CreateText(string name)
    {
        GameObject obj = new GameObject(name);
        return obj.AddComponent<TextMeshProUGUI>();
    }

    private Button CreateButton(string name)
    {
        GameObject obj = new GameObject(name);
        obj.AddComponent<Image>();
        return obj.AddComponent<Button>();
    }

    private GameObject CreatePanel(string name)
    {
        GameObject obj = new GameObject(name);
        obj.AddComponent<Image>();
        return obj;
    }

    private GearItem CreateGearItem(string itemName, bool isCorrect)
    {
        GameObject obj = new GameObject(itemName);
        obj.transform.SetParent(manager.gearPanel.transform);

        obj.AddComponent<Image>();
        obj.AddComponent<Button>();

        GearItem item = obj.AddComponent<GearItem>();
        item.itemName = itemName;
        item.isCorrect = isCorrect;
        item.alreadyChosen = false;

        GameObject resultIcon = new GameObject("ResultIcon");
        resultIcon.transform.SetParent(obj.transform);
        resultIcon.AddComponent<Image>();
        resultIcon.SetActive(false);

        return item;
    }
}