using UnityEngine;

// This object will act like a global struct, persistent across scene changes.
// This object stores the data for our game and acts like a reference point for
// that data. This object exists once in game. 
public class GameState : MonoBehaviour
{

    // note: {get; private set} would allow code to read gs, but not set or modify it. potential addition.
    public static GameState gs;
    public PlayerInventory playerInv;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
