using UnityEngine;

[CreateAssetMenu(
    fileName = "GridSettingsByLevel",
    menuName = "UI/Project/Grid Settings By Level",
    order = 1)]

public class GridSettingsByLevel : ScriptableObject
{
    [Header("Difficulty Level")]
    public int difficultyLevel;

    [Header("Grid Cell Setup")]
    public Vector2 cellSize = new Vector2();
    public Vector2 spacing = Vector2.zero;

    [Header("Grid Constraints")]
    public int constraintColumnCount = 0; // 0 = ignore
    public int constraintRowCount = 0;    // 0 = ignore

    public int GetTotalPairs()
    {
        int cols = constraintColumnCount;
        int rows = constraintRowCount;

        // Total number of grid cells
        int totalCells = cols * rows;

        // Unique sprite pairs needed
        int pairsNeeded = totalCells / 2;

        return pairsNeeded;
    }
}