using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID; // Unique identifier for the key
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collected key: {keyID}");
            // Notify the player that they have collected the key
            KeyManager.Instance.CollectKey(keyID);
            Destroy(gameObject); // Remove the key from the scene
        }
    }
}