using UnityEngine;

[CreateAssetMenu(menuName = "Boating/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int age;
    public float weight;
    public Sprite fullBodySprite;
    public Sprite portraitSprite;
    //public LifeJacketType requiredJacket;
}

