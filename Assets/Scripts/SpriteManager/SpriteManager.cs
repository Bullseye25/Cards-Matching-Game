using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Space]
    [Header("Card Content")]
    [SerializeField] private CardSpriteCollection cardsCollection;   // All available sprites to choose from

    [Space]
    [SerializeField] private List<Sprite> selectedSprites = new List<Sprite>();

    // Tracks how many times each sprite has been given out
    private Dictionary<Sprite, int> spriteUsageCount = new Dictionary<Sprite, int>();

    private void Awake()
    {
        SelectCardSprites();
    }

    /// <summary>
    /// Returns a random allowed sprite from selectedSprites,
    /// ensuring that each sprite can only be returned twice.
    /// Returns (Sprite sprite, int index)
    /// If no sprite available -> returns (null, -1)
    /// </summary>
    public (Sprite sprite, int index) GetData()
    {
        if (selectedSprites == null || selectedSprites.Count == 0)
        {
            Debug.LogWarning("SpriteManager: No selected sprites available.");
            return (null, -1);
        }

        // Build a list of valid sprites and track their indexes
        List<int> validIndexes = new List<int>();

        for (int i = 0; i < selectedSprites.Count; i++)
        {
            Sprite sprite = selectedSprites[i];
            int currentCount = spriteUsageCount.ContainsKey(sprite) ? spriteUsageCount[sprite] : 0;

            if (currentCount < 2)
                validIndexes.Add(i);
        }

        // If nothing left to give
        if (validIndexes.Count == 0)
        {
            return (null, -1);
        }

        // Pick a random valid index
        int chosenIndex = validIndexes[Random.Range(0, validIndexes.Count)];
        Sprite chosen = selectedSprites[chosenIndex];

        // Increment usage tracker
        if (!spriteUsageCount.ContainsKey(chosen))
            spriteUsageCount[chosen] = 1;
        else
            spriteUsageCount[chosen]++;

        return (chosen, chosenIndex);
    }

    /// <summary>
    /// Calculates how many cards fit based on the grid settings
    /// and selects a random subset of sprites for the matching pairs.
    /// </summary>
    public void SelectCardSprites()
    {
        // Reset previous data
        selectedSprites.Clear();
        spriteUsageCount.Clear();

        List<Sprite> spritePool = new List<Sprite>(cardsCollection.cardSprites);

        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, spritePool.Count);

            Sprite chosen = spritePool[index];
            selectedSprites.Add(chosen);

            spritePool.RemoveAt(index);
        }
    }

    /// <summary>
    /// Returns the sprites selected for the current grid.
    /// </summary>
    public List<Sprite> GetSelectedSprites()
    {
        return selectedSprites;
    }

    public void PassData(GameObject card)
    {
        var cardManager = card.GetComponent<CardManager>();
        var data = GetData();

        cardManager.CardID = data.index;
        cardManager.Card.sprite = data.sprite;
        cardManager.Card.preserveAspect = true;
    }
}