using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] BoatController boatController;
    [SerializeField] ScoreController scoreController;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreDecrementText;

    [Header("Star Scoreboard")]
    [SerializeField] private Image starScoreboardImage;
    [SerializeField] private Sprite zeroStarSprite;
    [SerializeField] private Sprite oneStarSprite;
    [SerializeField] private Sprite twoStarSprite;
    [SerializeField] private Sprite threeStarSprite;

    [Header("No Wake Warning")]
    [SerializeField] private TextMeshProUGUI noWakeZoneSlowDownText;

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateSpeedUIText;
        ScoreController.onScoreChanged += UpdateScoreUIText;
        ScoreController.onStarsChanged += UpdateStarUI;
        ScoreController.onScoreDecremented += RunDecrementAnimation;
        ScoreController.onNoWakeViolationChanged += UpdateNoWakeWarning;
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateSpeedUIText;
        ScoreController.onScoreChanged -= UpdateScoreUIText;
        ScoreController.onStarsChanged -= UpdateStarUI;
        ScoreController.onScoreDecremented -= RunDecrementAnimation;
        ScoreController.onNoWakeViolationChanged -= UpdateNoWakeWarning;
    }

    private void Start()
    {
        if (scoreDecrementText != null)
        {
            scoreDecrementText.gameObject.SetActive(false);

            CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 1f;
        }

        if (noWakeZoneSlowDownText != null)
        {
            noWakeZoneSlowDownText.gameObject.SetActive(false);
            noWakeZoneSlowDownText.rectTransform.localScale = Vector3.one;
        }

        UpdateStarUI(0);
    }

    public void UpdateSpeedUIText(float speed)
    {
        string displaySpeed;

        if (speed == boatController.GetMoveSpeeds()[0])
            displaySpeed = "10";
        else if (speed == boatController.GetMoveSpeeds()[1])
            displaySpeed = "25";
        else if (speed == boatController.GetMoveSpeeds()[2])
            displaySpeed = "40";
        else
            displaySpeed = speed.ToString();

        speedText.text = $"{displaySpeed}\nMPH";
    }

    public void UpdateScoreUIText(int score)
    {
        scoreText.text = score.ToString("N0");
    }

    public void UpdateStarUI(int starCount)
    {
        if (starScoreboardImage == null) return;

        switch (starCount)
        {
            case 0:
                starScoreboardImage.sprite = zeroStarSprite;
                break;
            case 1:
                starScoreboardImage.sprite = oneStarSprite;
                break;
            case 2:
                starScoreboardImage.sprite = twoStarSprite;
                break;
            case 3:
                starScoreboardImage.sprite = threeStarSprite;
                break;
        }
    }

    private void RunDecrementAnimation(int amount)
    {
        if (scoreDecrementText == null) return;

        CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

        LeanTween.cancel(scoreDecrementText.gameObject);
        LeanTween.cancel(scoreDecrementText.rectTransform);

        cg.alpha = 1f;
        scoreDecrementText.gameObject.SetActive(true);
        scoreDecrementText.text = $"-{amount}";

        Vector3 startPos = scoreDecrementText.rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0f, -40f, 0f);

        scoreDecrementText.rectTransform.anchoredPosition = startPos;

        LeanTween.delayedCall(scoreDecrementText.gameObject, 0.15f, () =>
        {
            LeanTween.move(scoreDecrementText.rectTransform, endPos, 0.6f)
                     .setEaseOutQuad();

            LeanTween.alphaCanvas(cg, 0f, 0.6f)
                     .setEaseOutQuad()
                     .setOnComplete(() =>
                     {
                         scoreDecrementText.rectTransform.anchoredPosition = startPos;
                         scoreDecrementText.gameObject.SetActive(false);
                     });
        });
    }

    private void UpdateNoWakeWarning(bool shouldShow)
    {
        if (noWakeZoneSlowDownText == null)
            return;

        RectTransform textRect = noWakeZoneSlowDownText.rectTransform;

        LeanTween.cancel(noWakeZoneSlowDownText.gameObject);
        LeanTween.cancel(textRect);

        if (shouldShow)
        {
            noWakeZoneSlowDownText.gameObject.SetActive(true);
            textRect.localScale = Vector3.one;

            LeanTween.scale(textRect, Vector3.one * 1.15f, 0.7f)
                     .setEaseInOutSine()
                     .setLoopPingPong();
        }
        else
        {
            textRect.localScale = Vector3.one;
            noWakeZoneSlowDownText.gameObject.SetActive(false);
        }
    }
}