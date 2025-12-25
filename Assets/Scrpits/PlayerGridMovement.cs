using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Wall")]
    public Tilemap wallTilemap;

    public static Vector2Int GridPosition { get; private set; }

    private bool isMoving;
    private Vector3 targetPosition;

    private void Start()
    {
        Vector3 start = SnapToGrid(transform.position);
        transform.position = start;
        targetPosition = start;

        GridPosition = new Vector2Int(
            Mathf.RoundToInt(start.x),
            Mathf.RoundToInt(start.y)
        );
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveVisual();
            return;
        }

        if (!LevelManager.Instance.HasMoves() || LevelManager.Instance.IsGameOver() || LevelManager.Instance.IsWin())
            return;

        Vector2Int dir = ReadInput();
        if (dir == Vector2Int.zero)
            return;

        TryMove(dir);
    }

    private void MoveVisual()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            transform.position = targetPosition;
            isMoving = false;
            LevelManager.OnPlayerMoveFinished?.Invoke();
        }
    }

    private Vector2Int ReadInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame) return Vector2Int.up;
        if (Keyboard.current.sKey.wasPressedThisFrame) return Vector2Int.down;
        if (Keyboard.current.aKey.wasPressedThisFrame) return Vector2Int.left;
        if (Keyboard.current.dKey.wasPressedThisFrame) return Vector2Int.right;
        return Vector2Int.zero;
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int targetGrid = GridPosition + direction;

        if (IsWallAtGrid(targetGrid))
        {
            Debug.Log($"[MOVE BLOCKED] Wall at {targetGrid}");
            return;
        }

        LevelManager.Instance.ConsumeMove();

        GridPosition = targetGrid;
        targetPosition = new Vector3(GridPosition.x, GridPosition.y, 0f);
        isMoving = true;
    }

    private bool IsWallAtGrid(Vector2Int gridPos)
    {
        Vector3Int cell = new Vector3Int(gridPos.x, gridPos.y, 0);
        return wallTilemap.HasTile(cell);
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(
            Mathf.Round(pos.x),
            Mathf.Round(pos.y),
            0f
        );
    }
}
