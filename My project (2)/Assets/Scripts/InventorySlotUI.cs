using UnityEngine;
using UnityEngine.UI;
using TMPro;

// each InventoryUI object visually represents one inventory slot
public class InventorySlotUI : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    // setter: call after instantiating the slot to fill in
    public void Set(ItemDefinition item, int quantity)
    {
        // Set icon sprite if we have an iconImage reference.
        if (iconImage != null)
        {
            // if item or icon does not exist -> null
            // this is a ternary expression, its like a special if statement
            // variable = if (condition) true branch else false branch
            iconImage.sprite = (item != null) ? item.itemSprite : null;
        }
    
        // Set quantity label if we have a quantityText reference.
        if (quantityText != null)
        {
            // If quantity is 1, we usually hide the number so it looks cleaner.
            if (quantity <= 1) quantityText.text = "";
            else quantityText.text = quantity.ToString();
        }
    }
}
