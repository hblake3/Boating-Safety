using UnityEngine;
using UnityEngine.UI;

public class StarEffectsController : MonoBehaviour
{
    [Header("Star Effect UI Images")]
    [SerializeField] private RectTransform[] starEffects;   // blueStar_Effect_1, 2, 3

    [Header("CanvasGroups")]
    [SerializeField] private CanvasGroup[] starCanvasGroups;

    [Header("Effects")]
    [SerializeField] private float gainStartScale = 0.25f;
    [SerializeField] private float gainPeakScale = 2.2f;
    [SerializeField] private float gainGrowTime = 0.45f;
    [SerializeField] private float gainSpinDegrees = 1080f;
    [SerializeField] private float gainSpinTime = 0.55f;
    [SerializeField] private float gainFadeInTime = 0.08f;
    [SerializeField] private float gainFadeOutTime = 0.32f;
    [SerializeField] private float gainFadeOutDelay = 0.18f;
    [SerializeField] private float lossStartScale = 1.15f;
    [SerializeField] private float lossEndScale = 0.35f;
    [SerializeField] private float lossShrinkTime = 0.30f;
    [SerializeField] private float lossSpinDegrees = -220f;
    [SerializeField] private float lossSpinTime = 0.30f;
    [SerializeField] private float lossFadeOutTime = 0.28f;
    [SerializeField] private float lossFadeOutDelay = 0f;

    private int currentStars = 0;

    private void Awake()
    {
        for (int i = 0; i < starEffects.Length; i++)
        {
            if (starEffects[i] == null) continue;

            starEffects[i].localScale = Vector3.one;
            starEffects[i].localEulerAngles = Vector3.zero;

            CanvasGroup cg = GetCanvasGroup(i);
            if (cg != null)
                cg.alpha = 0f;
        }
    }

    private void OnEnable()
    {
        ScoreController.onStarsChanged += HandleStarsChanged;
    }

    private void OnDisable()
    {
        ScoreController.onStarsChanged -= HandleStarsChanged;
    }

    private CanvasGroup GetCanvasGroup(int index)
    {
        if (starCanvasGroups != null &&
            index >= 0 &&
            index < starCanvasGroups.Length &&
            starCanvasGroups[index] != null)
        {
            return starCanvasGroups[index];
        }

        if (starEffects[index] == null) return null;

        CanvasGroup cg = starEffects[index].GetComponent<CanvasGroup>();
        if (cg == null)
            cg = starEffects[index].gameObject.AddComponent<CanvasGroup>();

        return cg;
    }

    private void HandleStarsChanged(int newStarCount)
    {
        if (newStarCount > currentStars)
        {
            for (int i = currentStars; i < newStarCount; i++)
            {
                PlayGain(i, (i - currentStars) * 0.08f);
            }
        }
        else if (newStarCount < currentStars)
        {
            for (int i = currentStars - 1; i >= newStarCount; i--)
            {
                PlayLoss(i, (currentStars - 1 - i) * 0.06f);
            }
        }

        currentStars = newStarCount;
    }

    private void PlayGain(int index, float delay = 0f)
    {
        if (index < 0 || index >= starEffects.Length || starEffects[index] == null) return;

        RectTransform star = starEffects[index];
        CanvasGroup cg = GetCanvasGroup(index);

        LeanTween.cancel(star.gameObject);

        star.localScale = Vector3.one * gainStartScale;
        star.localEulerAngles = Vector3.zero;

        if (cg != null)
            cg.alpha = 0f;

        // fade in
        if (cg != null)
        {
            LeanTween.value(star.gameObject, 0f, 1f, gainFadeInTime)
                .setDelay(delay)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnUpdate((float value) =>
                {
                    cg.alpha = value;
                });
        }

        // grow bigger
        LeanTween.scale(star.gameObject, Vector3.one * gainPeakScale, gainGrowTime)
            .setDelay(delay)
            .setEase(LeanTweenType.easeOutBack);

        // spin much more
        LeanTween.rotateZ(star.gameObject, gainSpinDegrees, gainSpinTime)
            .setDelay(delay)
            .setEase(LeanTweenType.easeOutCubic);

        // fade out while effect is still large
        if (cg != null)
        {
            LeanTween.value(star.gameObject, 1f, 0f, gainFadeOutTime)
                .setDelay(delay + gainFadeOutDelay)
                .setEase(LeanTweenType.easeInQuad)
                .setOnUpdate((float value) =>
                {
                    cg.alpha = value;
                })
                .setOnComplete(() =>
                {
                    star.localScale = Vector3.one;
                    star.localEulerAngles = Vector3.zero;
                });
        }
    }

    private void PlayLoss(int index, float delay = 0f)
    {
        if (index < 0 || index >= starEffects.Length || starEffects[index] == null) return;

        RectTransform star = starEffects[index];
        CanvasGroup cg = GetCanvasGroup(index);

        LeanTween.cancel(star.gameObject);

        star.localScale = Vector3.one * lossStartScale;
        star.localEulerAngles = Vector3.zero;

        if (cg != null)
            cg.alpha = 1f;

        LeanTween.scale(star.gameObject, Vector3.one * lossEndScale, lossShrinkTime)
            .setDelay(delay)
            .setEase(LeanTweenType.easeInBack);

        LeanTween.rotateZ(star.gameObject, lossSpinDegrees, lossSpinTime)
            .setDelay(delay)
            .setEase(LeanTweenType.easeInCubic);

        if (cg != null)
        {
            LeanTween.value(star.gameObject, 1f, 0f, lossFadeOutTime)
                .setDelay(delay + lossFadeOutDelay)
                .setEase(LeanTweenType.easeInQuad)
                .setOnUpdate((float value) =>
                {
                    cg.alpha = value;
                })
                .setOnComplete(() =>
                {
                    star.localScale = Vector3.one;
                    star.localEulerAngles = Vector3.zero;
                });
        }
        else
        {
            LeanTween.delayedCall(star.gameObject, delay + lossFadeOutDelay + lossFadeOutTime, () =>
            {
                star.localScale = Vector3.one;
                star.localEulerAngles = Vector3.zero;
            });
        }
    }
}