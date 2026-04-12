using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData selectedCharacter;
    public LifeJacketType requiredJacket;
    public LifeJacketData selectedJacket;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


    }

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // Reset inactivity timer on scene load
    //    InactivityManager inactivityManager = FindAnyObjectByType<InactivityManager>();
    //    if (inactivityManager != null)
    //    {
    //        inactivityManager.ResetTimer();
    //    }
    //}

}
