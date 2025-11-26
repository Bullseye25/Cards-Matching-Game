using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    #region Public Methods

    /// <summary>
    /// Load a scene by name instantly.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Load a scene by index instantly.
    /// </summary>
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Load a scene asynchronously with optional callback.
    /// </summary>
    public void LoadSceneAsync(string sceneName, Action onComplete = null)
    {
        StartCoroutine(LoadAsyncCoroutine(sceneName, onComplete));
    }

    #endregion

    #region Private Methods

    private System.Collections.IEnumerator LoadAsyncCoroutine(string sceneName, Action onComplete)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }

    #endregion
}