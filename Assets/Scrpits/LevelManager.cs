using UnityEngine;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Moves")]
    public int maxMoves;
    public int currentMoves;

    [Header("Lose")]
    public float loseDelay = 1.5f;

    public static Action<int> OnMovesChanged;
    public static Action OnLose;
    public static Action OnPlayerStep;
    public static Action OnPlayerMoveFinished;

    private bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
        if (isGameOver)
            return;

        currentMoves--;
        OnMovesChanged?.Invoke(currentMoves);
        OnPlayerStep?.Invoke();

        if (currentMoves <= 0)
        {
            TriggerLose();
        }
    }

    public void TriggerLose()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        StartCoroutine(LoseRoutine());
    }

    private IEnumerator LoseRoutine()
    {
        yield return new WaitForSeconds(loseDelay);
        OnLose?.Invoke();
    }

    public void ResetMoves()
    {
        isGameOver = false;
        currentMoves = maxMoves;
        OnMovesChanged?.Invoke(currentMoves);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
