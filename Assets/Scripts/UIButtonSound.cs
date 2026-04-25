using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
            UIAudioManager.Instance.PlayClick();
    }
}