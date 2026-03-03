using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    // attribute telling unity to display this class in editor
    [Serializable]
    public class ItemSlot
    {
        public ItemDefinition item;
        public int quantity;
    }

    // Scripts call this event whenever the inventory changes so UI can update.
    // note: Action denotes a void return with no parameter input
    // in C/C++ terms, this behaves like a list of function pointers called in
    // order when this object/file calls Changed().
    // THIS class controls when the event is called, but other scripts decide 
    // what functions should run when it happens by "subscribing".
    // To Subscribe:
    //     playerInv.Changed += SomeFunction;
    // To Unsubscribe:
    //     playerInv.Changed -= SomeFunction;
    // when we invoke Changed(), every subscribed function gets called immediately.
    public event Action Changed;


    // create a list of slots (dynamic). serialize the field so we can change it in the editor.
    [SerializeField]
    private List<ItemSlot> slots = new List<ItemSlot>();

    // getter function for our slots (read-only)
    public List<ItemSlot> GetItemSlots()
    {
        return slots;
    }

    // returns the first partially filled stack in our slot list for a 
    // particular item
    private ItemSlot FindPartialStack(ItemDefinition item)
    {
        if (item == null) return null;
        int slotCount = slots.Count;
        for (int i = 0; i < slotCount; i++)
        {
            ItemSlot s = slots[i];
            bool partialCond = (s.item == item) && 
                               (item.maxStack <= 0 || s.quantity < item.maxStack);
            if (partialCond)
            {
                return s;
            }
        }
        return null;
    }

    // returns the total number of a given item in a player's inventory across
    // all slots
    public int GetTotalItems(ItemDefinition item)
    {

        if (item == null) return 0;

        int itemTotal = 0;
        int slotCount = slots.Count;
        for (int i = 0; i < slotCount; i++)
        {   

            ItemSlot s = slots[i];
            if (s.item == item)
            {
                itemTotal += s.quantity;
            }
        }

        return itemTotal;
    }

    // returns how many slots currently hold this item (number of stacks)
    private int GetSlotCountForItem(ItemDefinition item)
    {
        if (item == null) return 0;
        int count = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item) count++;
        }
        return count;
    }

    public void ClearAll()
    {
        slots.Clear();
    }

    // signify if we can add an item. helper function for tryAdd
    // Semantics: maxStack = max per stack (per slot); maxCopies = max number of stacks (slots) of this item.
    public bool CanAdd(ItemDefinition item, int amt)
    {
        if ((item == null) || (amt <= 0)) return false;

        int maxStk = item.maxStack;
        int maxCopy = item.maxCopies;
        int slotCount = GetSlotCountForItem(item);

        // Unlimited per stack (maxStack <= 0): any amount fits in existing slot(s) or we can create a slot if under maxCopies.
        if (maxStk <= 0) return true;

        // Unlimited number of stacks (maxCopies <= 0): room in existing + we can create as many new slots as needed.
        if (maxCopy <= 0) return true;

        // Both limited: room = room in existing slots + room from new slots we're allowed to add.
        int roomInExisting = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
                roomInExisting += Math.Max(0, maxStk - slots[i].quantity);
        }
        int newSlotsAvailable = Math.Max(0, maxCopy - slotCount);
        int totalRoom = roomInExisting + newSlotsAvailable * maxStk;
        return amt <= totalRoom;
    }

    public bool TryAdd(ItemDefinition item, int amt)
    {
        if (!CanAdd(item, amt)) return false;

        int itemMaxStack = item.maxStack;

        // case 1: unstackable items (one per slot)
        if (itemMaxStack == 1)
        {
            int slotCount = GetSlotCountForItem(item);
            int maxCopy = item.maxCopies;
            int canAdd = (maxCopy <= 0) ? amt : Math.Min(amt, maxCopy - slotCount);
            if (canAdd <= 0) return false;
            for (int i = 0; i < canAdd; i++)
            {
                ItemSlot s = new ItemSlot();
                s.item = item;
                s.quantity = 1;
                slots.Add(s);
            }
            NotifyChanged();
            return canAdd == amt;
        }
        // case 2: stackable items
        else
        {
            int itemsToStack = amt;
            while (itemsToStack > 0)
            {
                ItemSlot partial = FindPartialStack(item);
                // case 1: partial slot exists, fill it
                if (partial != null)
                {
                    // for itemMaxStack = 0, there is always enough room left
                    int spaceLeft = (itemMaxStack != 0) ? (itemMaxStack - partial.quantity) : itemsToStack;
                    int numToStack = Math.Min(spaceLeft, itemsToStack);
                    partial.quantity += numToStack;

                    // decrease toStack by the # of items we store
                    itemsToStack -= numToStack;
                } 
                else
                {
                    // no partial slot — can we create a new stack? (maxCopies = max number of stacks)
                    int maxCopy = item.maxCopies;
                    if (maxCopy > 0 && GetSlotCountForItem(item) >= maxCopy)
                        return false; // at stack limit, no room in existing stacks

                    ItemSlot newSlot = new ItemSlot();
                    newSlot.item = item;

                    // stack as many items as we can into this new slot
                    int numToStack = (itemMaxStack != 0) ? Math.Min(itemsToStack, itemMaxStack): itemsToStack;
                    newSlot.quantity = numToStack;

                    slots.Add(newSlot);
                    itemsToStack -= numToStack;
                }
            }

            // notify all subscribed functions and return
            NotifyChanged();
            return true;
        }
    }

    public bool TryRemove(ItemDefinition item, int amt)
    {   
        if ((item == null) || (amt <= 0 )) return false;
        int have = GetTotalItems(item);

        // we can't remove more items than we have
        if (have < amt) return false;

        // local var representing num left to remove from player's inventory
        int toRemove = amt;

        // case 1: unstackable items
        if (item.maxStack == 1)
        {   
            // remove items at the 'end' first
            for(int i = slots.Count - 1; i >= 0 && toRemove > 0; i--)
            {
                if (slots[i].item == item)
                {
                    slots.RemoveAt(i);
                    toRemove--;
                }
            }

            NotifyChanged();
            return true;
        }
        // case 2: stackable items
        else
        {
            // remove items at the 'end' first
            for(int i = slots.Count - 1; i >= 0 && toRemove > 0; i--)
            {
                ItemSlot s = slots[i];
                // if we aren't looking at the right item, move on
                if (s.item != item) continue;

                int take = Math.Min(toRemove, s.quantity);
                s.quantity -= take;
                toRemove -= take;

                // if we've removed all items from a slot, remove the slot
                if (s.quantity <= 0)
                {
                    slots.RemoveAt(i);
                    // debug
                    if (s.quantity < 0) Debug.Log("PlayerInventory() -> TryRemove(): You've removed more items than you have!");
                }
            }
            
            NotifyChanged();
            return true;
        }
    }

    // this notifies all subscribed functions that something in the inventory
    // has changed
    private void NotifyChanged()
    {
        // Call the event only if someone is subscribed.
        if (Changed != null)
            Changed();
    }


}
