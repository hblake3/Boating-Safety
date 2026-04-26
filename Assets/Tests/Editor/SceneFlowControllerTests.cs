using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class SceneFlowControllerTests
{
    private GameObject controllerObject;
    private SceneFlowController controller;

    private GameObject gameManagerObject;

    private Image backgroundImage;
    private Sprite originalSprite;
    private Sprite zoomedSprite;

    [SetUp]
    public void SetUp()
    {
        gameManagerObject = new GameObject("GameManager");
        GameManager gameManager = gameManagerObject.AddComponent<GameManager>();
        GameManager.Instance = gameManager;

        CharacterData character = ScriptableObject.CreateInstance<CharacterData>();
        character.inspectionScale = 1.5f;
        gameManager.selectedCharacter = character;

        controllerObject = new GameObject("SceneFlowController");
        controller = controllerObject.AddComponent<SceneFlowController>();

        GameObject characterObject = new GameObject("CharacterDisplay");
        controller.characterFullBodyDisplay = characterObject.AddComponent<RectTransform>();

        GameObject backgroundObject = new GameObject("BackgroundImage");
        backgroundImage = backgroundObject.AddComponent<Image>();
        controller.backgroundImage = backgroundImage;

        originalSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        zoomedSprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero);

        backgroundImage.sprite = originalSprite;
        controller.zoomedBackgroundSprite = zoomedSprite;

        GameObject inspectionObject = new GameObject("SafetyInspectionManager");
        controller.safetyInspectionManager = inspectionObject.AddComponent<SafetyInspectionManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerObject);
        Object.DestroyImmediate(gameManagerObject);
    }

    [Test]
    public void CanCreateSceneFlowController()
    {
        // Tests that the SceneFlowController component can be successfully created.
        Assert.IsNotNull(controller);
    }

    [Test]
    public void InspectionTransition_ChangesBackgroundSprite()
    {
        // Tests that starting the inspection transition swaps to the zoomed background sprite.
        controller.InspectionTransition();

        Assert.AreEqual(zoomedSprite, backgroundImage.sprite);
    }
}