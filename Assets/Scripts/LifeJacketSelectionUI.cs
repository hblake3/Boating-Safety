using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LifeJacketSelectionUI : MonoBehaviour
{
    public Image itemDisplayImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI detailsText;

    public void SelectJacket(LifeJacketData jacket)
    {
        itemDisplayImage.sprite = jacket.displaySprite;
        nameText.text = jacket.jacketName;
        detailsText.text = jacket.description;
    }
}
