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
            if ((s.item == item) && s.quantity < item.maxStack)
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

    // signify if we can add an item. helper function for tryAdd
    public bool CanAdd(ItemDefinition item, int amt)
    {
        if ((item == null) || (amt < 0)) return false;

        // is maxcopies <=0, treat as infinite
        if (item.maxCopies <= 0) return true;
        int currItemsInInv = GetTotalItems(item);
        return (currItemsInInv + amt) <= (item.maxCopies * item.maxStack); // TODO: make sure this is right (testing)
    }

    public bool TryAdd(ItemDefinition item, int amt)
    {
        if (!CanAdd(item, amt)) return false;

        int itemMaxStack = item.maxStack;

        // case 1: unstackable items
        if (itemMaxStack == 1)
        {
            for (int i = 0; i < amt; i++)
            {
                ItemSlot s = new ItemSlot();
                s.item = item;
                s.quantity = 1;
                slots.Add(s);
            }
            NotifyChanged();
            return true;
        }
        // case 2: stackable items
        else
        {
            int itemsToStack = amt;
            while (itemsToStack > 0)
            {
                ItemSlot partial = FindPartialStack(item);
                if (partial != null)
                {   
                    // fill the partial slot
                    int spaceLeft = itemMaxStack - partial.quantity;
                    int numToStack = Math.Min(spaceLeft, itemsToStack);
                    partial.quantity += numToStack;
                    itemsToStack -= numToStack;
                } 
                else
                {
                    // no partial slot, create a new one
                    ItemSlot newSlot = new ItemSlot();
                    newSlot.item = item;

                    // stack as many items as we can into this new slot
                    int numToStack = Math.Min(itemsToStack, itemMaxStack);
                    newSlot.quantity = numToStack;
                    
                    // add the new slot to our list
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
        if ((item == null) || (amt <=0 )) return false;
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
