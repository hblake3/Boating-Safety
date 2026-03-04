using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public float hoverScale = 1.1f;
    public float scaleSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void OnEnable()
    {
        // Ensure the object starts at its original scale when re-enabled
        transform.localScale = originalScale;
        targetScale = originalScale;
    }

    void OnDisable()
    {
        // Reset scale immediately when disabled so it won't reappear scaled
        transform.localScale = originalScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly interpolate to the target scale
        transform.localScale =
            Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
