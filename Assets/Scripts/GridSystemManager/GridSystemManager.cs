using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSystemManager : ObjectPoolingSystem
{
    public static GridSystemManager Instance { get; private set; }

    [Header("Grid Settings")]
    public GridLayoutGroup gridLayoutGroup;

    [Header("Difficulty Presets")]
    [SerializeField] private GridSettingsByLevel defaultSettings;
    [SerializeField] private List<GridSettingsByLevel> levels = new List<GridSettingsByLevel>();

    // ========================================
    // Override Pool Parent (Grid Layout)
    // ========================================
    protected override Transform PoolParentTransform => gridLayoutGroup.transform;

    // ========================================
    // Awake for Singleton
    // ========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ========================================
    // Lifecycle
    // ========================================
    protected override void Start()
    {
        base.Start();
        ApplyGridSettings();
    }

    [ContextMenu("Apply Grid Settings")]
    public void EditorApplySettings()
    {
        Start();
    }

    // ========================================
    // Apply Grid Settings
    // ========================================
    private void ApplyGridSettings()
    {
        if (gridLayoutGroup == null || levels.Count == 0)
            return;

        foreach (var setting in levels)
        {
            if (setting.difficultyLevel == 2) // Get the current game level from game manager later.
            {
                defaultSettings = setting;
                break;
            }
        }

        // Apply size and spacing
        gridLayoutGroup.cellSize = defaultSettings.cellSize;
        gridLayoutGroup.spacing = defaultSettings.spacing;

        int rowCount = 0;
        int colCount = 0;

        if (defaultSettings.constraintColumnCount > 0)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = defaultSettings.constraintColumnCount;

            colCount = defaultSettings.constraintColumnCount;
            rowCount = (defaultSettings.constraintRowCount > 0)
                ? defaultSettings.constraintRowCount
                : 1;
        }
        else if (defaultSettings.constraintRowCount > 0)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = defaultSettings.constraintRowCount;

            rowCount = defaultSettings.constraintRowCount;
            colCount = (defaultSettings.constraintColumnCount > 0)
                ? defaultSettings.constraintColumnCount
                : 1;
        }
        else
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
            return;
        }

        int total = Mathf.Max(1, colCount * rowCount);
        EnforceGridObjectLimit(total);
    }

    // ========================================
    // Enforce Grid Limit
    // ========================================
    private void EnforceGridObjectLimit(int maxCount)
    {
        // Expand pool if needed
        for (int i = pool.Count; i < maxCount; i++)
            pool.Add(CreateNewObject());

        // Activate required
        for (int i = 0; i < maxCount; i++)
            pool[i].SetActive(true);

        // Deactivate extra
        for (int i = maxCount; i < pool.Count; i++)
            pool[i].SetActive(false);
    }

    // ========================================
    // Spawn
    // ========================================
    public GameObject SpawnGridCell()
    {
        GameObject obj = GetPooledObject();
        obj.SetActive(true);

        RectTransform rt = obj.transform as RectTransform;
        rt.SetAsLastSibling();
        rt.localScale = Vector3.one;

        return obj;
    }

    // ========================================
    // Clear / Destroy
    // ========================================
    [ContextMenu("Clear Grid")]
    public override void ClearPool()
    {
        base.ClearPool();
    }

    [ContextMenu("Destroy All Cells")]
    public override void DestroyAllPooled()
    {
        base.DestroyAllPooled();
    }

    // ========================================
    // Get Active Settings
    // ========================================
    public GridSettingsByLevel GetActiveSettings()
    {
        return defaultSettings;
    }
}
