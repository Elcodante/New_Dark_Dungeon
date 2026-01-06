using UnityEngine;

public class ToggleTrap : MonoBehaviour
{
    public bool isActive = true;

    private Animator animator;
    private Vector2Int trapGridPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(isActive)
        {
            animator.Play("IdleOn", 0, 1f);
        }
        else
        {
            animator.Play("IdleOff", 0, 1f);
        }
        trapGridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );

        Debug.Log($"[Trap:{name}] Awake | GridPos={trapGridPosition}");
    }

    private void Start()
    {
        Debug.Log($"[Trap:{name}] Start | Initial IsActive={isActive}");
    }

    private void OnEnable()
    {
        LevelManager.OnPlayerStep += Toggle;
        LevelManager.OnPlayerMoveFinished += CheckKill;

        Debug.Log($"[Trap:{name}] Subscribed to events");
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerStep -= Toggle;
        LevelManager.OnPlayerMoveFinished -= CheckKill;

        Debug.Log($"[Trap:{name}] Unsubscribed from events");
    }

    private void Toggle()
    {
        isActive = !isActive;

        if (isActive)
        {
            animator.ResetTrigger("ToOff");
            animator.SetTrigger("ToOn");
        }
        else
        {
            animator.ResetTrigger("ToOn");
            animator.SetTrigger("ToOff");
        }
    }


    private void CheckKill()
    {
        Debug.Log(
            $"[Trap:{name}] CheckKill | IsActive={isActive} | " +
            $"Trap={trapGridPosition} | Player={PlayerGridMovement.GridPosition}"
        );

        if (!isActive)
            return;

        if (trapGridPosition == PlayerGridMovement.GridPosition)
        {
            Debug.Log($"[Trap:{name}] PLAYER KILLED");
            LevelManager.Instance.TriggerLose();
        }
    }
}
