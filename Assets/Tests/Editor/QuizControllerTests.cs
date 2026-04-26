using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class QuizControllerTests
{
    private GameObject controllerObject;
    private QuizController controller;

    private GameObject quizRoot;
    private TextMeshProUGUI questionText;
    private Button[] answerButtons;
    private TextMeshProUGUI[] answerTexts;
    private GameObject feedbackBubble;
    private TextMeshProUGUI feedbackText;

    [SetUp]
    public void SetUp()
    {
        controllerObject = new GameObject("QuizController");
        controller = controllerObject.AddComponent<QuizController>();

        quizRoot = new GameObject("QuizRoot");
        questionText = CreateText("QuestionText");

        answerButtons = new Button[3];
        answerTexts = new TextMeshProUGUI[3];

        for (int i = 0; i < 3; i++)
        {
            answerButtons[i] = CreateButton("AnswerButton" + i);
            answerTexts[i] = CreateText("AnswerText" + i);
        }

        feedbackBubble = new GameObject("FeedbackBubble");
        feedbackText = CreateText("FeedbackText");

        SetPrivateField("quizRoot", quizRoot);
        SetPrivateField("questionText", questionText);
        SetPrivateField("answerButtons", answerButtons);
        SetPrivateField("answerTexts", answerTexts);
        SetPrivateField("feedbackBubble", feedbackBubble);
        SetPrivateField("feedbackText", feedbackText);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerObject);
    }

    [Test]
    public void CanCreateQuizController()
    {
        // Tests that the QuizController component can be successfully created.
        Assert.IsNotNull(controller);
    }

    [Test]
    public void ShowQuiz_LoadsQuestionAndAnswers()
    {
        // Tests that ShowQuiz loads the question and all answer choices into the UI.
        QuizQuestionData question = CreateQuestion();

        controller.ShowQuiz(question);

        Assert.AreEqual("What should you wear on a boat?", questionText.text);
        Assert.AreEqual("Life Jacket", answerTexts[0].text);
        Assert.AreEqual("Winter Coat", answerTexts[1].text);
        Assert.AreEqual("Backpack", answerTexts[2].text);
    }

    [Test]
    public void ShowQuiz_EnablesButtonsAndShowsQuiz()
    {
        // Tests that ShowQuiz displays the quiz, enables answer buttons, and hides feedback.
        QuizQuestionData question = CreateQuestion();

        controller.ShowQuiz(question);

        Assert.IsTrue(quizRoot.activeSelf);
        Assert.IsFalse(feedbackBubble.activeSelf);

        foreach (Button button in answerButtons)
        {
            Assert.IsTrue(button.interactable);
        }
    }

    private QuizQuestionData CreateQuestion()
    {
        return new QuizQuestionData
        {
            question = "What should you wear on a boat?",
            answers = new string[] { "Life Jacket", "Winter Coat", "Backpack" },
            correctAnswerIndex = 0,
            correctFeedback = "That's correct!",
            incorrectFeedback = "Not quite. Try again!"
        };
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

    private void SetPrivateField(string fieldName, object value)
    {
        FieldInfo field = typeof(QuizController).GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(controller, value);
    }
}