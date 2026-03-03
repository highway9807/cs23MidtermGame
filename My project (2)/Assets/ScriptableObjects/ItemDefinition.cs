using UnityEngine;


// recall: scriptable objects are like shared program data written once into 
//         memory for efficient sharing (by reference).
[CreateAssetMenu(fileName = "ItemDefinition", menuName = "Scriptable Objects/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    [Header("Identity")] // called 'attribtues,' these ones only affect the editor visually
    public int id;
    public string itemName;

    [Header("UI")]
    public Sprite itemSprite;

    [Header("Stacking")]
    [Tooltip("Max of this item per stack (per slot). 0 = unlimited per slot.")]
    public int maxStack;
    [Tooltip("Max number of stacks (slots) of this item allowed. 0 = unlimited stacks.")]
    public int maxCopies;
}
