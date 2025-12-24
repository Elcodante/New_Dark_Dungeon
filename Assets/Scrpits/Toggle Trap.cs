using UnityEngine;

public class ToggleTrap : MonoBehaviour
{
    public bool isActive = true;
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private SpriteRenderer spriteRenderer;
    private Vector2Int trapGridPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trapGridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );
    }

    private void Start()
    {
        UpdateSprite();
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
        UpdateSprite();
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

    private void UpdateSprite()
    {
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
    }
}
