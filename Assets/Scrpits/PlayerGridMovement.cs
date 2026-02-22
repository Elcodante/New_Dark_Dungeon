using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Wall")]
    public Tilemap wallTilemap;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    public static Vector2Int GridPosition { get; private set; }

    private bool isMoving;
    private Vector3 targetPosition;

    private void Start()
    {
        Vector3 start = SnapToGrid(transform.position);
        transform.position = start;
        targetPosition = start;

        Vector3Int cell = wallTilemap.WorldToCell(start);
        GridPosition = new Vector2Int(cell.x, cell.y);

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
        UpdateFacing(targetPosition - transform.position);
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

        if (DoorManager.Instance.IsBlocked(targetGrid))
        {
            Door door = DoorManager.Instance.GetDoor(targetGrid);
            door.TryOpen();
            return;
        }

        LevelManager.Instance.ConsumeMove();

        GridPosition = targetGrid;
        Vector3Int cell = new Vector3Int(targetGrid.x, targetGrid.y, 0);
        targetPosition = wallTilemap.GetCellCenterWorld(cell);
        isMoving = true;
    }

    private bool IsWallAtGrid(Vector2Int gridPos)
    {
        Vector3Int cell = new Vector3Int(gridPos.x, gridPos.y, 0);
        return wallTilemap.HasTile(cell);
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        Vector3Int cell = wallTilemap.WorldToCell(pos);
        return wallTilemap.GetCellCenterWorld(cell);
    }
    private void UpdateFacing(Vector2 moveDir)
    {
        if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnEnable()
    {
        LevelManager.OnLoseStarted += PlayDeath;
    }
    private void OnDisable()
    {
        LevelManager.OnLoseStarted -= PlayDeath;
    }
    private void PlayDeath()
    {
        animator.SetTrigger("Die");
    }
    public void OnDeathAnimationFinished()
    {
        LevelManager.Instance.NotifyDeathAnimationFinished();
    }
}
