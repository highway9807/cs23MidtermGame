using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    [Header("Wiring")]
    
    // TODO: drag InventoryRoot here
    [SerializeField] private GameObject inventoryRoot; 

    [Header("Startup")]
    [SerializeField] private bool startClosed = true;

    [Header("Pause Behavior")]
    [SerializeField] private bool pauseGameplayWhenOpen = true;

    // the reason we would pause from this file (always inventory pause)
    private const PauseReason InvPause = PauseReason.Inventory;

    // Start runs once when the object first becomes active.
    private void Start()
    {
        if (inventoryRoot != null && startClosed)
        {
            inventoryRoot.SetActive(!startClosed);
        }

        // in case there was some gameplay pause, then this avoids us getting "stuck"
        if (pauseGameplayWhenOpen)
        {
            PauseManager.ReleasePause(InvPause);
        }
    }

    private void Update()
    {
        if (inventoryRoot == null) return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = inventoryRoot.activeSelf;
            inventoryRoot.SetActive(!isOpen);
            if (pauseGameplayWhenOpen)
            {
                if (!isOpen) PauseManager.RequestPause(InvPause);
                else PauseManager.ReleasePause(InvPause);
            }
        }
    }

    private void OnDestroy()
    {
        // if theres some scene change or something else happens, do NOT leave
        // the game pasued
        if (pauseGameplayWhenOpen)
        {
            PauseManager.ReleasePause(InvPause);
        }
    }
}
