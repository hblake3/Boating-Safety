using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LifeJacketSelectionUI : MonoBehaviour
{
    public Image characterDisplayImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI detailsText;
    //public FeedbackPopupUI feedbackPopup;


    public void SelectJacket(LifeJacketData jacket)
    {
        characterDisplayImage.sprite = jacket.displaySprite;
        nameText.text = jacket.jacketName;
        detailsText.text = jacket.description;
    }
}
