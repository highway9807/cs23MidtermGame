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
    public int maxStack;
    // potentially use -1 as a "no limit" value
    public int maxCopies;
}
