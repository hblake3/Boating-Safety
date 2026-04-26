using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class QuizQuestionData
{
    [TextArea(2, 4)]
    public string question;

    public string[] answers = new string[3];

    [Range(0, 2)]
    public int correctAnswerIndex;

    [TextArea(2, 3)]
    public string correctFeedback = "That's correct!";

    [TextArea(2, 3)]
    public string incorrectFeedback = "Not quite. Try again!";
}

public class QuizController : MonoBehaviour
{
    [Header("Quiz Root")]
    [SerializeField] private GameObject quizRoot;

    [Header("Question UI")]
    [SerializeField] private TextMeshProUGUI questionText;

    [Header("Answer Buttons")]
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;

    [Header("Feedback Bubble")]
    [SerializeField] private GameObject feedbackBubble;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Timing")]
    [SerializeField] private float correctPauseSeconds = 2f;
    [SerializeField] private float incorrectPauseSeconds = 1.25f;

    private QuizQuestionData currentQuestion;
    private bool acceptingInput;

    // [ DELEGATES ]
    public delegate void OnQuizPassed();
    public static OnQuizPassed onQuizPassed;

    private void Start()
    {
        quizRoot.SetActive(false);
        feedbackBubble.SetActive(false);

        // each answer button has its own capture index
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int capturedIndex = i;
            answerButtons[i].onClick.AddListener(() => SubmitAnswer(capturedIndex));
        }
    }

    public void ShowQuiz(QuizQuestionData questionData)
    {
        currentQuestion = questionData;
        acceptingInput = true;

        questionText.text = currentQuestion.question;

        // populate the answer text on to each button
        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = currentQuestion.answers[i];
        }

        SetButtonsInteractable(true);

        feedbackBubble.SetActive(false);
        quizRoot.SetActive(true);
    }

    private void SubmitAnswer(int answerIndex)
    {
        // ignore extra clicks while feedback is displayedx
        if (!acceptingInput || currentQuestion == null)
            return;

        StartCoroutine(HandleAnswer(answerIndex));
    }

    private IEnumerator HandleAnswer(int answerIndex)
    {
        acceptingInput = false;
        SetButtonsInteractable(false);

        bool isCorrect = answerIndex == currentQuestion.correctAnswerIndex;

        // Show the correct or incorrect response text
        feedbackText.text = isCorrect
            ? currentQuestion.correctFeedback
            : currentQuestion.incorrectFeedback;

        feedbackBubble.SetActive(true);

        yield return new WaitForSeconds(isCorrect ? correctPauseSeconds : incorrectPauseSeconds);

        feedbackBubble.SetActive(false);

        if (isCorrect)
        {
            quizRoot.SetActive(false);
            onQuizPassed?.Invoke();
        }
        else
        {
            // allow player to try again after a wrong answer
            acceptingInput = true;
            SetButtonsInteractable(true);
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        // toggle all answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = interactable;
        }
    }
}