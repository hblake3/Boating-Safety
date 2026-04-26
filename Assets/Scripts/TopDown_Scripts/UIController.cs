using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] BoatController boatController;
    [SerializeField] ScoreController scoreController;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreIncrementText;
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
        ScoreController.onScoreIncremented += RunIncrementAnimation;
        ScoreController.onNoWakeViolationChanged += UpdateNoWakeWarning;
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateSpeedUIText;
        ScoreController.onScoreChanged -= UpdateScoreUIText;
        ScoreController.onStarsChanged -= UpdateStarUI;
        ScoreController.onScoreDecremented -= RunDecrementAnimation;
        ScoreController.onScoreIncremented -= RunIncrementAnimation;
        ScoreController.onNoWakeViolationChanged -= UpdateNoWakeWarning;
    }

    private void Start()
    {
        // prepare decrement text so it is ready when a penalty happens
        if (scoreDecrementText != null)
        {
            scoreDecrementText.gameObject.SetActive(false);

            CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 1f;
        }

        // prep reward text the same as penalty text
        if (scoreIncrementText != null)
        {
            scoreIncrementText.gameObject.SetActive(false);

            CanvasGroup cg = scoreIncrementText.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = scoreIncrementText.gameObject.AddComponent<CanvasGroup>();

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

        // map internal values to more user-friendly MPH numbers
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

        // swap the scoreboard image based on the current star count
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

    private void RunIncrementAnimation(int amount)
    {
        if (scoreIncrementText == null) return;

        CanvasGroup cg = scoreIncrementText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = scoreIncrementText.gameObject.AddComponent<CanvasGroup>();

        // clear any old reward animation before starting a new one
        LeanTween.cancel(scoreIncrementText.gameObject);
        LeanTween.cancel(scoreIncrementText.rectTransform);

        cg.alpha = 1f;
        scoreIncrementText.gameObject.SetActive(true);
        scoreIncrementText.text = $"+{amount}";

        Vector3 startPos = scoreIncrementText.rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0f, -40f, 0f);

        scoreIncrementText.rectTransform.anchoredPosition = startPos;

        LeanTween.delayedCall(scoreIncrementText.gameObject, 0.15f, () =>
        {
            LeanTween.move(scoreIncrementText.rectTransform, endPos, 0.6f)
                     .setEaseOutQuad();

            LeanTween.alphaCanvas(cg, 0f, 0.6f)
                     .setEaseOutQuad()
                     .setOnComplete(() =>
                     {
                         scoreIncrementText.rectTransform.anchoredPosition = startPos;
                         scoreIncrementText.gameObject.SetActive(false);
                     });
        });
    }

    private void RunDecrementAnimation(int amount)
    {
        if (scoreDecrementText == null) return;

        CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

        // Stop the old penalty animation so the text does not overlap itself
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

            // ***
            // pulse the warning message while the player is going too fast
            // ***
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