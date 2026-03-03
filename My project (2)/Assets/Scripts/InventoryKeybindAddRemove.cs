using UnityEngine;
using System.Collections.Generic;

public class InventoryKeybindAddRemove : MonoBehaviour
{
    [Header("Items (populate via Tools > Refresh Item List From Project)")]
    [SerializeField] private List<ItemDefinition> items = new List<ItemDefinition>();

    [Header("Hotkeys")]
    [SerializeField] private KeyCode addItemKey = KeyCode.K;
    [SerializeField] private KeyCode removeItemKey = KeyCode.L;

    void Update()
    {
        if (Input.GetKeyDown(addItemKey))
            addItems();
        if (Input.GetKeyDown(removeItemKey))
            removeItems();
    }

    private void addItems()
    {
        PlayerInventory inv = getInventory();
        if (inv == null) return;
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("[InventoryKeybindAddRemove] No items in list. Use Tools > Refresh Item List From Project (with this GameObject selected).");
            return;
        }
        foreach (ItemDefinition item in items)
        {
            if (item == null) continue;
            bool ok = inv.TryAdd(item, 1);
            Debug.Log("Add " + item.itemName + " x1 -> " + ok);
        }
    }

    private void removeItems()
    {
        PlayerInventory inv = getInventory();
        if (inv == null) return;
        if (items == null || items.Count == 0) return;
        foreach (ItemDefinition item in items)
        {
            if (item == null) continue;
            bool ok = inv.TryRemove(item, 1);
            Debug.Log("Remove " + item.itemName + " x1 -> " + ok);
        }
    }

    private PlayerInventory getInventory()
    {
        if (GameState.gs == null)
        {
            Debug.LogError("[InventoryKeybindAddRemove] GameState.gs is null. Is GameState in the scene?");
            return null;
        }
        return GameState.gs.playerInv;
    }
}
