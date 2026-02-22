using UnityEngine;
using System.Collections.Generic;
public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance { get; private set; }

    private Dictionary<Vector2Int, Door> doors = new Dictionary<Vector2Int, Door>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterDoor(Vector2Int position, Door door)
    {
        if (!doors.ContainsKey(position))
        {
            doors.Add(position, door);
        }
    }

    public void UnRegisterDoor(Vector2Int position)
    {
        if (doors.ContainsKey(position))
        {
            doors.Remove(position);
        }
    }

    public bool IsBlocked(Vector2Int position)
    {
        if (doors.TryGetValue(position, out Door door))
        {
            return door.IsBlocking();
        }
        return false;
    }
    public Door GetDoor(Vector2Int position)
    {
        if (doors.TryGetValue(position, out Door door))
        {
            return door;
        }
        return null;
    }
}
