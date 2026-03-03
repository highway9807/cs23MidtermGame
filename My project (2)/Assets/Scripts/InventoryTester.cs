using UnityEngine;

/// <summary>
/// Runtime tester for PlayerInventory. Attach to any GameObject (e.g. same as GameState).
/// Press T in Play mode to run all tests. Assign at least one ItemDefinition to test with.
/// </summary>
public class InventoryTester : MonoBehaviour
{
    [Header("Items to use in tests (assign in Inspector)")]
    [SerializeField] private ItemDefinition testItemA;
    [SerializeField] private ItemDefinition testItemB;

    [Header("Hotkey")]
    [SerializeField] private KeyCode runTestsKey = KeyCode.T;

    private void Update()
    {
        if (Input.GetKeyDown(runTestsKey))
            RunAllTests();
    }

    public void RunAllTests()
    {
        if (GameState.gs == null)
        {
            Debug.LogError("[InventoryTester] GameState.gs is null. Is GameState in the scene?");
            return;
        }

        PlayerInventory inv = GameState.gs.playerInv;
        if (inv == null)
        {
            Debug.LogError("[InventoryTester] GameState.playerInv is null.");
            return;
        }

        if (testItemA == null)
        {
            Debug.LogWarning("[InventoryTester] testItemA is not assigned. Assign an ItemDefinition to run tests.");
            return;
        }

        int passed = 0;
        int failed = 0;

        inv.ClearAll();

        // --- Test 1: Add item and verify count ---
        bool addOk = inv.TryAdd(testItemA, 3);
        int total = inv.GetTotalItems(testItemA);
        if (addOk && total == 3)
        {
            Debug.Log("[InventoryTester] PASS: TryAdd + GetTotalItems (added 3, got " + total + ")");
            passed++;
        }
        else
        {
            Debug.LogError("[InventoryTester] FAIL: TryAdd(3) returned " + addOk + ", GetTotalItems = " + total);
            failed++;
        }

        // --- Test 2: Add more (stacking) ---
        inv.TryAdd(testItemA, 2);
        total = inv.GetTotalItems(testItemA);
        if (total == 5)
        {
            Debug.Log("[InventoryTester] PASS: Stacking (total = " + total + ")");
            passed++;
        }
        else
        {
            Debug.LogError("[InventoryTester] FAIL: After adding 2 more, total = " + total + " (expected 5)");
            failed++;
        }

        // --- Test 3: Remove some ---
        bool removeOk = inv.TryRemove(testItemA, 2);
        total = inv.GetTotalItems(testItemA);
        if (removeOk && total == 3)
        {
            Debug.Log("[InventoryTester] PASS: TryRemove (removed 2, total = " + total + ")");
            passed++;
        }
        else
        {
            Debug.LogError("[InventoryTester] FAIL: TryRemove(2) returned " + removeOk + ", total = " + total + " (expected 3)");
            failed++;
        }

        // --- Test 4: Remove more than we have (should fail) ---
        bool removeFail = inv.TryRemove(testItemA, 10);
        total = inv.GetTotalItems(testItemA);
        if (!removeFail && total == 3)
        {
            Debug.Log("[InventoryTester] PASS: TryRemove(10) correctly refused, total still " + total);
            passed++;
        }
        else
        {
            Debug.LogError("[InventoryTester] FAIL: TryRemove(10) should have failed. removeOk=" + removeFail + ", total=" + total);
            failed++;
        }

        // --- Test 5: ClearAll ---
        inv.ClearAll();
        total = inv.GetTotalItems(testItemA);
        int slotCount = inv.GetItemSlots().Count;
        if (total == 0 && slotCount == 0)
        {
            Debug.Log("[InventoryTester] PASS: ClearAll (total=0, slots=" + slotCount + ")");
            passed++;
        }
        else
        {
            Debug.LogError("[InventoryTester] FAIL: ClearAll — total=" + total + ", slotCount=" + slotCount);
            failed++;
        }

        // --- Test 6 (optional): Second item ---
        if (testItemB != null)
        {
            inv.ClearAll();
            inv.TryAdd(testItemA, 1);
            inv.TryAdd(testItemB, 1);
            int slots = inv.GetItemSlots().Count;
            int aTotal = inv.GetTotalItems(testItemA);
            int bTotal = inv.GetTotalItems(testItemB);
            if (slots >= 1 && aTotal == 1 && bTotal == 1)
            {
                Debug.Log("[InventoryTester] PASS: Two item types (slots=" + slots + ", A=" + aTotal + ", B=" + bTotal + ")");
                passed++;
            }
            else
            {
                Debug.LogError("[InventoryTester] FAIL: Two items — slots=" + slots + ", A=" + aTotal + ", B=" + bTotal);
                failed++;
            }
        }
        else
        {
            Debug.Log("[InventoryTester] Skip: testItemB not assigned (optional)");
        }

        Debug.Log("[InventoryTester] Done. Passed: " + passed + ", Failed: " + failed);
    }
}
