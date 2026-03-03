using UnityEngine;

/// <summary>
/// Place this on empty GameObjects in the ice mine scene where ice can respawn.
/// Do not put it on the entrance/exit. MineIce picks a random spawn point when respawning.
/// </summary>
public class IceSpawnPoint : MonoBehaviour
{
    // No fields needed - position comes from transform.position
}
