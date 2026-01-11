using UnityEngine;

public class FlamethrowerTrap : MonoBehaviour
{
    public bool isActive = false;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(isActive)
        {
            animator.Play("IdleOn",0, 1f);
        }
        else
        {
            animator.Play("IdleOff",0, 1f);
        }
    }
    private void OnEnable()
    {
        LevelManager.OnPlayerStep += Toggle;
    }
    private void OnDisable()
    {
        LevelManager.OnPlayerStep -= Toggle;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger entered by {other.name}, IsActive={isActive}");

        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player killed by flamethrower");
            LevelManager.Instance.TriggerLose();
        }
    }


}

