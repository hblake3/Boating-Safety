using UnityEngine;

[CreateAssetMenu(menuName = "Boating/Life Jacket")]
public class LifeJacketData : ScriptableObject
{
    public string jacketName;
    public LifeJacketType jacketType;
    public Sprite displaySprite;
    public string description;
    public Sprite fullbodysprite;
}
