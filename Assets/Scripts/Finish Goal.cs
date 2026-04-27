using UnityEngine;

public class FinishGoal : MonoBehaviour
{
    private Vector2Int goalPosition;
    private void Start()
    {
        goalPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
    }

    private void OnEnable()
    {
        LevelManager.OnPlayerMoveFinished += CheckWinCondition;
    }
    private void OnDisable()
    {
        LevelManager.OnPlayerMoveFinished -= CheckWinCondition;
    }

    private void CheckWinCondition()
    {
        if (LevelManager.Instance.IsWin())
        {
            return;
        }
        if(PlayerGridMovement.GridPosition == goalPosition)
        {
            LevelManager.Instance.TriggerWin();
        }
    }
}
