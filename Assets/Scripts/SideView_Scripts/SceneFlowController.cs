using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

using static GameManager;

public class SceneFlowController : MonoBehaviour
{
    [Header("Scene References")]
    public RectTransform characterFullBodyDisplay;
    public Image backgroundImage;

    [Header("Sprites")]
    public Sprite zoomedBackgroundSprite;

    [Header("Zoom Settings")]
    public float moveDuration = 0.5f; 

    public SafetyInspectionManager safetyInspectionManager;
    public SelectionDialogueController dialogueController;

    private Vector2 startPos;
    private Vector3 startScale;
    private Sprite originalBackgroundSprite;

    public void InspectionTransition()
    {
        // Store original background to reset later
        originalBackgroundSprite = backgroundImage.sprite;

        // Swap background instantly
        backgroundImage.sprite = zoomedBackgroundSprite;
        StopAllCoroutines();
        StartCoroutine(MoveAndZoomCharacter());
    }

    private IEnumerator MoveAndZoomCharacter()
    {
        // Set the character's initial position and target position
        startPos = characterFullBodyDisplay.anchoredPosition;
        Vector2 targetPos = new Vector2(0, startPos.y);

        // Set the character's initial scale and target scale
        startScale = characterFullBodyDisplay.localScale;
        Vector3 endScale = Vector3.one * Instance.selectedCharacter.inspectionScale;

        float time = 0f;

        // Smoothly move and scale the character over time
        while (time < moveDuration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = time / moveDuration;

            // Smooth easing
            t = Mathf.SmoothStep(0, 1f, t);

            characterFullBodyDisplay.anchoredPosition =
                Vector2.Lerp(startPos, targetPos, t);

            characterFullBodyDisplay.localScale =
                Vector3.Lerp(startScale, endScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and scale are set
        characterFullBodyDisplay.anchoredPosition = targetPos;
        characterFullBodyDisplay.localScale = endScale;

        // NOW start inspection
        safetyInspectionManager.StartInspection();
    }

    public void ExitInspection()
    {
        // Reset background
        backgroundImage.sprite = originalBackgroundSprite;

        // Move and scale character back to original position and scale
        StopAllCoroutines();
        StartCoroutine(ResetCharacterPositionAndScale());

    }

    private IEnumerator ResetCharacterPositionAndScale()
    {
        float time = 0f;
        while (time < moveDuration)
        {
            float t = time / moveDuration;
            t = Mathf.SmoothStep(0, 1.5f, t);
            characterFullBodyDisplay.anchoredPosition =
                Vector2.Lerp(characterFullBodyDisplay.anchoredPosition, startPos, t);
            characterFullBodyDisplay.localScale =
                Vector3.Lerp(characterFullBodyDisplay.localScale, startScale, t);
            time += Time.deltaTime;
            yield return null;
        }
        // Ensure final position and scale are set
        characterFullBodyDisplay.anchoredPosition = startPos;
        characterFullBodyDisplay.localScale = startScale;

        // Start the next step, safety gear
        dialogueController.OnNextClicked2(Instance.selectedCharacter);
    }

}

