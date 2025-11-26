using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SpriteManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridSystemManager gridSystemManager;

    [Space]
    [Header("Card Content")]
    [SerializeField] private CardSpriteCollection cardsCollection;   // All available sprites to choose from

    [Space]
    [SerializeField] private List<Sprite> selectedSprites = new List<Sprite>();

    // Tracks how many times each sprite has been given out
    private Dictionary<Sprite, int> spriteUsageCount = new Dictionary<Sprite, int>();

    /// <summary>
    /// Returns a random allowed sprite from selectedSprites,
    /// ensuring that each sprite can only be returned twice.
    /// Returns (Sprite sprite, int index)
    /// If no sprite available -> returns (null, -1)
    /// </summary>
    private (Sprite sprite, int index) GetData()
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
    public void SelectCardSprites(Action onComplete)
    {
        if (gridSystemManager == null || cardsCollection == null || cardsCollection.cardSprites.Count == 0)
        {
            Debug.LogError("SpriteManager: Missing references or sprites.");
            return;
        }

        var settings = gridSystemManager.GetActiveSettings();

        if (settings == null)
        {
            Debug.LogError("SpriteManager: No active grid settings found.");
            return;
        }

        int cols = settings.constraintColumnCount;
        int rows = settings.constraintRowCount;

        // Total number of grid cells
        int totalCells = cols * rows;

        // Unique sprite pairs needed
        int pairsNeeded = totalCells / 2;

        var count = cardsCollection.cardSprites.Count;

        if (pairsNeeded > count)
        {
            Debug.LogError($"Not enough sprites! Need {pairsNeeded}, but only have {count}.");
            return;
        }

        // Reset previous data
        selectedSprites.Clear();
        spriteUsageCount.Clear();

        List<Sprite> spritePool = new List<Sprite>(cardsCollection.cardSprites);

        for (int i = 0; i < pairsNeeded; i++)
        {
            int index = Random.Range(0, spritePool.Count);

            Sprite chosen = spritePool[index];
            selectedSprites.Add(chosen);

            spritePool.RemoveAt(index);
        }

        onComplete?.Invoke();
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