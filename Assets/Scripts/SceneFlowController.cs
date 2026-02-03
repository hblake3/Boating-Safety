using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowController : MonoBehaviour
{
    public void GoToLifeJacketScene()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.selectedCharacter == null)
        {
            Debug.LogWarning("No character selected!");
            return;
        }

        SceneManager.LoadScene("LifeJacketSelection");
    }
}
