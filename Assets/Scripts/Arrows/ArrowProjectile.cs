using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class ArrowProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float collisionDelay = 0.1f;
    private Vector2 direction;
    private Collider2D arrowCollider;
    private void Awake()
    {
        arrowCollider = GetComponent<Collider2D>();
        arrowCollider.enabled = false;
    }
    public void Init(Vector2 dir)
    {
        direction = dir.normalized;

        Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            LevelManager.Instance.TriggerLose();
            Destroy(gameObject);
            return;
        }
        StartCoroutine(EnableCollisionDelayed());
    }
    private IEnumerator EnableCollisionDelayed()
    {
        yield return new WaitForSeconds(collisionDelay);
        arrowCollider.enabled = true;
    }
    public void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!arrowCollider.enabled)return;
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("Arrow hit wall and is destroyed.");
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Player"))
        {
            Debug.Log("Arrow hit player. Game Over.");
            LevelManager.Instance.TriggerLose();
            Destroy(gameObject);
        }
    }
}
