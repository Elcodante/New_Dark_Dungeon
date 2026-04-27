using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    [Header("Door Config")]
    public string doorID;
    public Tilemap referenceTilemap; // untuk konversi world → grid

    private Animator animator;
    private Vector2Int gridPosition;

    private enum DoorState { Locked, Opening, Open }
    private DoorState currentState = DoorState.Locked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Vector3Int cell = referenceTilemap.WorldToCell(transform.position);
        gridPosition = new Vector2Int(cell.x, cell.y);

        DoorManager.Instance.RegisterDoor(gridPosition, this);
    }

    public bool IsBlocking()
    {
        return currentState != DoorState.Open;
    }

    public void TryOpen()
    {
        if (currentState != DoorState.Locked)
            return;

        if (!KeyManager.Instance.HasKey(doorID))
            return;

        currentState = DoorState.Opening;
        animator.SetTrigger("Open");
    }

    // Dipanggil via Animation Event di akhir animasi
    public void OnDoorOpened()
    {
        currentState = DoorState.Open;

        DoorManager.Instance.UnRegisterDoor(gridPosition);
    }
}