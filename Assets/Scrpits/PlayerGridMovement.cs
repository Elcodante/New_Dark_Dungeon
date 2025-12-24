using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveDistance = 1f;
    public float moveSpeed = 10f;

    [Header("Collision")]
    public LayerMask wallLayer;

    public static Vector2Int GridPosition { get; private set; }

    private bool isMoving;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = SnapToGrid(transform.position);
        transform.position = targetPosition;
        UpdateGridPosition();
    }

    private void Update()
    {
        if (isMoving)
        {
            UpdateMovement();
            return;
        }

        if (!LevelManager.Instance.HasMoves())
            return;

        Vector3 direction = ReadInput();
        if (direction == Vector3.zero)
            return;

        TryMove(direction);
    }

    private void UpdateMovement()
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

            UpdateGridPosition();
            LevelManager.OnPlayerMoveFinished?.Invoke();
        }
    }

    private Vector3 ReadInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame) return Vector3.up;
        if (Keyboard.current.sKey.wasPressedThisFrame) return Vector3.down;
        if (Keyboard.current.aKey.wasPressedThisFrame) return Vector3.left;
        if (Keyboard.current.dKey.wasPressedThisFrame) return Vector3.right;
        return Vector3.zero;
    }

    private void TryMove(Vector3 direction)
    {
        if (IsWallInDirection(direction))
            return;

        LevelManager.Instance.ConsumeMove();

        targetPosition = SnapToGrid(targetPosition + direction * moveDistance);
        isMoving = true;
    }

    private void UpdateGridPosition()
    {
        GridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            0f
        );
    }

    private bool IsWallInDirection(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            Vector2.one,
            0f,
            direction,
            moveDistance,
            wallLayer
        );

        return hit.collider != null;
    }
}
