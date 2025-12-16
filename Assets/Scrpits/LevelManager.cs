using UnityEngine;
using System;
using System.Collections;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int maxMoves;
    public int currentMoves;

    public float loseDelay = 1.5f;
    public static Action<int> OnMovesChanged;
    public static Action OnLose;

    private bool isGameOver = false;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;     
    }
    private void Start()
    {
        ResetMoves();
    }
    public bool HasMoves()
    {
        return currentMoves > 0 && !isGameOver;
    }
    public void ConsumeMove()
    {
        if(isGameOver)
        {
            return;
        }
        currentMoves--;
        OnMovesChanged?.Invoke(currentMoves);
        if(currentMoves <= 0)
        {
            StartCoroutine(LoseRoutine());
        }
    }
    IEnumerator LoseRoutine()
    {
        isGameOver = true;
        yield return new WaitForSeconds(loseDelay);
        OnLose?.Invoke();
    }
    public void ResetMoves()
    {
        isGameOver = false;
        currentMoves = maxMoves;
        OnMovesChanged?.Invoke(currentMoves);
    }
}
