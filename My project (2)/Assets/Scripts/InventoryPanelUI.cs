using UnityEngine;

public class InventoryPanelUI : MonoBehaviour
{
    [Header("Wiring")]

    // The object that has GridLayoutGroup
    [SerializeField] private Transform gridParent;        
    // prefab for ONE item box
    [SerializeField] private InventorySlotUI slotPrefab; 

    private PlayerInventory playerInv;

    private void OnEnable()
    {
        // Grab playerInv from GameState singleton.
        // GameState should already exist in the scene, and it persists across scene loads.
        if (GameState.gs != null)
        {
            playerInv = GameState.gs.playerInv;
        }
        else
        {
            playerInv = null;
        }

        // 'subscribe' to the event so UI updates any time playerInv changes.
        if (playerInv != null)
        {
            playerInv.Changed += Rebuild;
        }

        // build once immediately so the panel looks correct when opened.
        Rebuild();
    }

    private void OnDisable()
    {
        // 'unsubscribe' to avoid double-subscriptions.
        //example bug without this: opening playerInv twice might cause two 
        // Rebuild calls each change.
        if (playerInv != null)
        {
            playerInv.Changed -= Rebuild;
        }
    }

    public void Rebuild()
    {
        if (gridParent == null || slotPrefab == null) return;

        // clear existing UI boxes.
        // iterate backwards because childCount changes as we destroy children.
        for (int i = gridParent.childCount - 1; i >= 0; i--)
        {
            Destroy(gridParent.GetChild(i).gameObject);
        }

        if (playerInv == null) return;

        // grab the inventory slots--> inherit type (var)
        var slots = playerInv.GetItemSlots();

        // Requirement: if no items, show no boxes.
        if (slots.Count == 0) return;

        // one UI slot per playerInv slot.
        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];

            // Create a new slot UI object under the grid.
            InventorySlotUI ui = Instantiate(slotPrefab, gridParent);

            // Fill it with the slot data.
            ui.Set(s.item, s.quantity);
        }
    }
}
