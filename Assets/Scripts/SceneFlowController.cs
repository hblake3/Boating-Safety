using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using static GameManager;

public class SceneFlowController : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject speechBubble;
    public RectTransform characterFullBodyDisplay;
    public Image backgroundImage;

    [Header("Sprites")]
    public Sprite zoomedBackgroundSprite;

    [Header("Zoom Settings")]
    public float moveDuration = 0.5f;
    
public void InspectionTransition()
    {

        // Swap background instantly
        backgroundImage.sprite = zoomedBackgroundSprite;
        StopAllCoroutines();
        StartCoroutine(MoveAndZoomCharacter());
    }

    private IEnumerator MoveAndZoomCharacter()
    {
        
        Vector2 startPos = characterFullBodyDisplay.anchoredPosition;
        Vector2 targetPos = new Vector2(0, startPos.y);

        Vector3 startScale = characterFullBodyDisplay.localScale;
        Vector3 endScale = Vector3.one * Instance.selectedCharacter.inspectionScale;

        float time = 0f;

        while (time < moveDuration)
        {
            float t = time / moveDuration;

            // Smooth easing
            t = Mathf.SmoothStep(0, 1, t);

            characterFullBodyDisplay.anchoredPosition =
                Vector2.Lerp(startPos, targetPos, t);

            characterFullBodyDisplay.localScale =
                Vector3.Lerp(startScale, endScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        characterFullBodyDisplay.anchoredPosition = targetPos;
        characterFullBodyDisplay.localScale = endScale;
    }
}

