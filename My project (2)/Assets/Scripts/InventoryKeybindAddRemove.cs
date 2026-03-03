using UnityEngine;

public class InventoryKeybindAddRemove : MonoBehaviour
{
    [Header("Items to use in tests (assign in Inspector)")]
    [SerializeField] private ItemDefinition testItemA;
    [SerializeField] private ItemDefinition testItemB;

    [Header("Hotkeys")]
    [SerializeField] private KeyCode addItemKey = KeyCode.K;
    [SerializeField] private KeyCode removeItemKey = KeyCode.L;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(addItemKey))
        {
            addItems();
        }
        if(Input.GetKeyDown(removeItemKey))
        {
            removeItems();
        }
    }

    private void addItems() {
        PlayerInventory inv = getInventory();
        if (inv == null) {
            Debug.LogError("[InventoryTester] GameState.playerInv is null.");
            return;
        }
        Debug.Log("Adding 1 " + testItemA.itemName + " was " + (inv.TryAdd(testItemA, 1) ? "successful\n" : "unsuccessful\n"));
        Debug.Log("Adding 1 " + testItemB.itemName + " was " + (inv.TryAdd(testItemB, 1) ? "successful\n" : "unsuccessful\n"));
    }
    
    private void removeItems() {
        PlayerInventory inv = getInventory();
        if (inv == null) {
            Debug.LogError("[InventoryTester] GameState.playerInv is null.");
            return;
        }
        Debug.Log("Removing 1 " + testItemA.itemName + " was " + (inv.TryRemove(testItemA, 1) ? "successful\n" : "unsuccessful\n"));
        Debug.Log("Removing 1 " + testItemB.itemName + " was " + (inv.TryRemove(testItemB, 1) ? "successful\n" : "unsuccessful\n"));
    }

    private PlayerInventory getInventory()
    {
        if (GameState.gs == null)
        {
            Debug.LogError("[InventoryTester] GameState.gs is null. Is GameState in the scene?");
            return null;
        }
    
        return GameState.gs.playerInv;;

    }
    private bool conditionsMet()
    {
        return (testItemA != null && testItemB != null);
    }
}
