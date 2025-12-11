using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 10f;
    private bool isMoving = false;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if(transform.position == targetPosition)
            {
                isMoving = false;
                return;
            }
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
        targetPosition = transform.position + direction * moveDistance;
        isMoving = true;
    }
}
