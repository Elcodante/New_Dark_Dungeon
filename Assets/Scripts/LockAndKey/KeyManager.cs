using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }
    private HashSet<string> collectedKeys = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    public void CollectKey(string keyID)
    {
        if (!collectedKeys.Contains(keyID))
        {
            collectedKeys.Add(keyID);
            Debug.Log($"Key collected: {keyID}");
        }
    }
    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Optionally, you can clear collected keys when a new scene is loaded
        collectedKeys.Clear();
    }
}