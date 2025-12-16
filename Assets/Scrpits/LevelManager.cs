using UnityEngine;
using System;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int maxMoves;
    public int currentMoves;

    public static Action<int> OnMovesChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        
    }
    public bool HasMoves()
    {
        return currentMoves > 0;
    }
    public void ConsumeMove()
    {
        if(currentMoves <= 0) return;
        currentMoves--;
        OnMovesChanged?.Invoke(currentMoves);
    }
    public void ResetMoves()
    {
        currentMoves = maxMoves;
        OnMovesChanged?.Invoke(currentMoves);
    }
}
