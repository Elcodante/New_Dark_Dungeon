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

        
    }

    private void OnEnable()
    {
        LevelManager.OnPlayerStep += Toggle;
        LevelManager.OnPlayerMoveFinished += CheckKill;
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerStep -= Toggle;
        LevelManager.OnPlayerMoveFinished -= CheckKill;
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
        if (!isActive)
            return;
        if (trapGridPosition == PlayerGridMovement.GridPosition)
        {           
            LevelManager.Instance.TriggerLose();
        }
    }
}
