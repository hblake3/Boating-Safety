using UnityEngine;


public enum LifeJacketType
{
    Youth,
    Adult,
    Child
}


[CreateAssetMenu(menuName = "Boating/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int age;
    public float weight;
    public LifeJacketType requiredJacket;
    
    public Sprite fullBodySprite;
    public Sprite portraitSprite;

    [Header("Display Settings")]
    public Vector2 displaySize; // Width, Height
    public Vector2 displayPosition; // UI anchored position
}

