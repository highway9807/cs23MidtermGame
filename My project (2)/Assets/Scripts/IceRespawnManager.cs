using UnityEngine;

/// <summary>
/// Place on a GameObject in each ice mine scene. Assign the Ice prefab and a single
/// spawn point Transform (e.g. an empty GameObject). Ice will respawn at that spawn point's position.
/// </summary>
public class IceRespawnManager : MonoBehaviour
{
    public static IceRespawnManager Instance { get; private set; }

    [Tooltip("The Ice prefab to spawn when ice is mined. Assign in Inspector.")]
    [SerializeField] private GameObject icePrefab;

    [Tooltip("Drag the Transform (e.g. an empty GameObject) where you want ice to appear. Ice will spawn at this position.")]
    [SerializeField] private Transform spawnPoint;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    /// <summary>Spawns one ice at the manager's assigned spawn point. Returns the spawned GameObject or null.</summary>
    public GameObject SpawnIce()
    {
        if (icePrefab == null)
        {
            Debug.LogWarning("IceRespawnManager: Ice Prefab is not assigned.");
            return null;
        }
        if (spawnPoint == null)
        {
            Debug.LogWarning("IceRespawnManager: Spawn Point is not assigned. Drag an empty GameObject (or any Transform) into the Spawn Point field.");
            return null;
        }
        return Instantiate(icePrefab, spawnPoint.position, Quaternion.identity);
    }
}
