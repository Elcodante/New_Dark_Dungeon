using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTrap : MonoBehaviour
{
    public bool isActive = false;
    public ShootDirection direction;
    public GameObject arrowPrefab;
    public Transform firePoint;

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
            FireArrow();
        }
    }
    private void FireArrow()
    {
        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        ArrowProjectile arrow = arrowObj.GetComponent<ArrowProjectile>();
        arrow.Init(GetDirectionVector());
    }
    private Vector2 GetDirectionVector()
    {
        switch(direction)
        {
            case ShootDirection.Up : return Vector2.up;
            case ShootDirection.Down: return Vector2.down;
            case ShootDirection.Left: return Vector2.left;
            case ShootDirection.Right: return Vector2.right;
            default: return Vector2.right;
        }
    }
}
