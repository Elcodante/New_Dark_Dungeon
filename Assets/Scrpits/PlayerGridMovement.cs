using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 10f;
    public LayerMask wallLayer;
    private bool isMoving = false;
    private Vector3 targetPosition;

    void Start()
    {
        transform.position = SnapToGrid(transform.position);
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
            return;
        }
        if (!LevelManager.Instance.HasMoves())
        {
            return;
        }
        Vector3 direction = Vector3.zero;
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            direction = Vector3.up;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            direction = Vector3.down;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            direction = Vector3.left;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            direction = Vector3.right;
        }
        if(direction == Vector3.zero)
        {
            return;
        }
        Vector3 newTarget = targetPosition + direction * moveDistance;
        newTarget = SnapToGrid(newTarget);

        if (IsWallAtPosition(direction))
        {
            Debug.Log("Wall detected at position: " + newTarget);
            return;
        }

        LevelManager.Instance.ConsumeMove();

        targetPosition = SnapToGrid(newTarget);
        isMoving = true;
    }
    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
    }
    private bool IsWallAtPosition(Vector3 dir)
    {
        float distance = moveDistance;
        Vector2 size = new Vector2(1f, 1f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, size, 0f, dir.normalized, distance, wallLayer);
        return hit.collider != null;
    }
}
