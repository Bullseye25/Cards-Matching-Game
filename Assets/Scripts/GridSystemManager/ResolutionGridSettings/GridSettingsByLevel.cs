using UnityEngine;

[CreateAssetMenu(
    fileName = "GridSettingsByLevel",
    menuName = "UI/Grid/Grid Settings By Level",
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

    public GridSettingsByLevel(int difficultyLevel, Vector2 cellSize, Vector2 spacing, int constraintColumnCount, int constraintRowCount)
    {
        this.difficultyLevel = difficultyLevel;
        this.cellSize = cellSize;
        this.spacing = spacing;
        this.constraintColumnCount = constraintColumnCount;
        this.constraintRowCount = constraintRowCount;
    }
}
