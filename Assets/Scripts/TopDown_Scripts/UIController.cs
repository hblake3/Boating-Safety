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

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateSpeedUIText;
        ScoreController.onScoreChanged += UpdateScoreUIText;
        ScoreController.onStarsChanged += UpdateStarUI;
        BoatCollision.onBoatHit += RunDecrementAnimation;
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateSpeedUIText;
        ScoreController.onScoreChanged -= UpdateScoreUIText;
        ScoreController.onStarsChanged -= UpdateStarUI;
        BoatCollision.onBoatHit -= RunDecrementAnimation;
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

    private void RunDecrementAnimation()
    {
        if (scoreDecrementText == null) return;

        CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 1f;
        scoreDecrementText.gameObject.SetActive(true);
        scoreDecrementText.text = ($"-{scoreController.scoreDecrementAmount.ToString()}");

        Vector3 startPos = scoreDecrementText.rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0f, -40f, 0f);

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
}