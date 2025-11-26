using UnityEngine;

public class SaveAndLoadSystem : MonoBehaviour
{
    protected void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    protected int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    protected bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
