using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    [Header("Wiring")]
    
    // TODO: drag InventoryRoot here
    [SerializeField] private GameObject inventoryRoot; 

    [Header("Startup")]
    [SerializeField] private bool startClosed = true;

    // Start runs once when the object first becomes active.
    private void Start()
    {
        if (inventoryRoot != null)
        {
            inventoryRoot.SetActive(!startClosed);
        }
    }

    private void Update()
    {
        if (inventoryRoot == null) return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = inventoryRoot.activeSelf;
            inventoryRoot.SetActive(!isOpen);
        }
    }
}
