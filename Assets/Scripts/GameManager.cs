using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData selectedCharacter;
    //public LifeJacketType requiredJacket;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
}
