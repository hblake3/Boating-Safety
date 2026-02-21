using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] BoatController boatController;
    [SerializeField] ScoreController scoreController;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreDecrementText;

    private void OnEnable()
    {
        BoatController.onSpeedChanged += UpdateSpeedUIText;
        ScoreController.onScoreChanged += UpdateScoreUIText;
        BoatCollision.onBoatHit += RunDecrementAnimation;
    }

    private void OnDisable()
    {
        BoatController.onSpeedChanged -= UpdateSpeedUIText;
        ScoreController.onScoreChanged -= UpdateScoreUIText;
        BoatCollision.onBoatHit -= RunDecrementAnimation;
    }

    private void Start()
    {
        if (scoreDecrementText == null) return;

        // ensure hidden at game start
        scoreDecrementText.gameObject.SetActive(false);

        // ensure alpha is reset in case editor saved it faded
        CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 1f;
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
            displaySpeed = speed.ToString(); // fallback just in case

        speedText.text = $"{displaySpeed}\nMPH";
    }

    public void UpdateScoreUIText(int score)
    {
        scoreText.text = score.ToString("N0"); ;
    }

    private void RunDecrementAnimation()
    {
        if (scoreDecrementText == null) return;

        CanvasGroup cg = scoreDecrementText.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = scoreDecrementText.gameObject.AddComponent<CanvasGroup>();

        // reset starting state
        cg.alpha = 1f;
        scoreDecrementText.gameObject.SetActive(true);
        scoreDecrementText.text = ($"-{scoreController.scoreDecrementAmount.ToString()}");

        Vector3 startPos = scoreDecrementText.rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0f, -40f, 0f); // move slightly downward

        // small delay before movement begins
        LeanTween.delayedCall(scoreDecrementText.gameObject, 0.15f, () =>
        {
            // move downward
            LeanTween.move(scoreDecrementText.rectTransform, endPos, 0.6f)
                     .setEaseOutQuad();

            // fade out at the same time
            LeanTween.alphaCanvas(cg, 0f, 0.6f)
                     .setEaseOutQuad()
                     .setOnComplete(() =>
                     {
                         // reset for next use
                         scoreDecrementText.rectTransform.anchoredPosition = startPos;
                         scoreDecrementText.gameObject.SetActive(false);
                     });
        });
    }

}
