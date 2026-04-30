using UnityEngine;


public enum LifeJacketType
{
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

    [Header("Speech Bubble Settings")]
    public Vector2 speechBubbleAnchoredPosition;

    [Header("Inspection Settings")]
    public float inspectionScale = 1.8f;
}

