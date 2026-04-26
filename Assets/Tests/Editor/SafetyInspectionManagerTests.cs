using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class SafetyInspectionManagerTests
{
    private GameObject managerObject;
    private SafetyInspectionManager manager;
    private GameObject gameManagerObject;

    private GameObject speechBubble;
    private TextMeshProUGUI dialogueText;
    private Button nextArrowButton;

    private GameObject buddyStrapStep;
    private GameObject buddyEarStep;
    private GameObject barryStrapStep;
    private GameObject barryEarStep;

    [SetUp]
    public void SetUp()
    {
        gameManagerObject = new GameObject("GameManager");
        GameManager gameManager = gameManagerObject.AddComponent<GameManager>();
        GameManager.Instance = gameManager;

        CharacterData character = ScriptableObject.CreateInstance<CharacterData>();
        character.characterName = "Buddy the Beaver";
        gameManager.selectedCharacter = character;

        managerObject = new GameObject("SafetyInspectionManager");
        manager = managerObject.AddComponent<SafetyInspectionManager>();

        speechBubble = CreateUIObject("SpeechBubble");
        dialogueText = CreateText("DialogueText");
        nextArrowButton = CreateButton("NextArrowButton");

        buddyStrapStep = new GameObject("BuddyStrapStep");
        buddyEarStep = new GameObject("BuddyEarStep");
        barryStrapStep = new GameObject("BarryStrapStep");
        barryEarStep = new GameObject("BarryEarStep");

        buddyStrapStep.SetActive(false);
        buddyEarStep.SetActive(false);
        barryStrapStep.SetActive(false);
        barryEarStep.SetActive(false);

        SetPrivateField("speechBubble", speechBubble);
        SetPrivateField("dialogueText", dialogueText);
        SetPrivateField("nextArrowButton", nextArrowButton);
        SetPrivateField("buddyStrapStep", buddyStrapStep);
        SetPrivateField("buddyEarStep", buddyEarStep);
        SetPrivateField("barryStrapStep", barryStrapStep);
        SetPrivateField("barryEarStep", barryEarStep);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(managerObject);
        Object.DestroyImmediate(gameManagerObject);
    }

    [Test]
    public void CanCreateSafetyInspectionManager()
    {
        // Tests that the SafetyInspectionManager component can be successfully created.
        Assert.IsNotNull(manager);
    }

    [Test]
    public void StartInspection_ActivatesSpeechBubbleAndBuddyStrapStep()
    {
        // Tests that starting inspection activates the speech bubble and Buddy strap step.
        manager.StartInspection();

        Assert.IsTrue(managerObject.activeSelf);
        Assert.IsTrue(speechBubble.activeSelf);
        Assert.IsTrue(buddyStrapStep.activeSelf);
        Assert.IsFalse(barryStrapStep.activeSelf);
        Assert.IsNotEmpty(dialogueText.text);
    }

    [Test]
    public void CorrectSelected_StrapsStep_ShowsCorrectFeedbackAndEnablesNextButton()
    {
        // Tests that selecting the correct straps hotspot shows success feedback and enables next button.
        InspectionHotspot hotspot = CreateHotspot();

        manager.currentStep = SafetyInspectionManager.InspectionStep.Straps;
        manager.CorrectSelected(hotspot);

        Assert.AreEqual("Great job! Always make sure all straps on the lifejacket are tight and snug!", dialogueText.text);
        Assert.IsTrue(nextArrowButton.gameObject.activeSelf);
        Assert.IsFalse(hotspot.GetComponent<Button>().interactable);
    }

    private GameObject CreateUIObject(string name)
    {
        GameObject obj = new GameObject(name);
        obj.AddComponent<RectTransform>();
        return obj;
    }

    private TextMeshProUGUI CreateText(string name)
    {
        GameObject obj = CreateUIObject(name);
        return obj.AddComponent<TextMeshProUGUI>();
    }

    private Button CreateButton(string name)
    {
        GameObject obj = CreateUIObject(name);
        obj.AddComponent<Image>();
        return obj.AddComponent<Button>();
    }

    private InspectionHotspot CreateHotspot()
    {
        GameObject obj = new GameObject("InspectionHotspot");

        Image image = obj.AddComponent<Image>();
        Button button = obj.AddComponent<Button>();

        InspectionHotspot hotspot = obj.AddComponent<InspectionHotspot>();

        SetHotspotPrivateField(hotspot, "image", image);
        SetHotspotPrivateField(hotspot, "button", button);
        SetHotspotPrivateField(hotspot, "manager", manager);

        return hotspot;
    }

    private void SetPrivateField(string fieldName, object value)
    {
        FieldInfo field = typeof(SafetyInspectionManager).GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(manager, value);
    }

    private void SetHotspotPrivateField(InspectionHotspot hotspot, string fieldName, object value)
    {
        FieldInfo field = typeof(InspectionHotspot).GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(hotspot, value);
    }
}