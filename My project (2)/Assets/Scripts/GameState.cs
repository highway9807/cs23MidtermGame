using UnityEngine;

// This object will act like a global struct, persistent across scene changes.
// This object stores the data for our game and acts like a reference point for
// that data. This object exists once in game. 
public class GameState : MonoBehaviour
{

    // note: {get; private set} would allow code to read gs, but not set or modify it. potential addition.
    public static GameState gs;
    public PlayerInventory playerInv;

    [Tooltip("Ice prefab for respawning one ice when mined. Assign in Inspector.")]
    [SerializeField] private GameObject icePrefab;

    private void Awake()
    {
        // ensure no game object duplicates
        if (gs && (gs != this))
        {
            Destroy(gameObject);
            return;
        }

        // assign this particular obj to be the official gamestate
        gs = this;

        // allow gs to persist across scene changes
        DontDestroyOnLoad(gameObject);

        // assign the player inventory object, or create one if it doesnt exist
        playerInv = GetComponent<PlayerInventory>();
        if(!playerInv)
        {
            playerInv = gameObject.AddComponent<PlayerInventory>();
        }
    }

    /// <summary>Spawns one ice at the given position. Used by MineIce when ice is mined.</summary>
    public void SpawnIceAt(Vector3 position)
    {
        if (icePrefab != null)
            Instantiate(icePrefab, position, Quaternion.identity);
    }
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
