using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardCollection", menuName = "UI/Project/Cards Collection")]
public class CardSpriteCollection : ScriptableObject
{
    [Header("Card Sprites")]
    public List<Sprite> cardSprites = new List<Sprite>();
}
