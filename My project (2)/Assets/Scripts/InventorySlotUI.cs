using UnityEngine;
using UnityEngine.UI;
using TMPro;

// each InventoryUI object visually represents one inventory slot
public class InventorySlotUI : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    const float IconSize = 72f;

    // setter: call after instantiating the slot to fill in
    public void Set(ItemDefinition item, int quantity)
    {
        if (iconImage != null)
        {
            Sprite sprite = (item != null) ? item.itemSprite : null;
            iconImage.sprite = sprite;

            // Center the icon regardless of sprite pivot (fixes dogsled, nails, salmon, leather, coin, ice saw, etc.)
            RectTransform iconRect = iconImage.rectTransform;
            if (sprite != null)
            {
                float w = sprite.rect.width;
                float h = sprite.rect.height;
                if (w > 0 && h > 0)
                {
                    float scale = Mathf.Min(IconSize / w, IconSize / h);
                    float offsetX = (w * 0.5f - sprite.pivot.x) * scale;
                    float offsetY = (h * 0.5f - sprite.pivot.y) * scale;
                    iconRect.anchoredPosition = new Vector2(offsetX, -8f + offsetY);
                }
                else
                    iconRect.anchoredPosition = new Vector2(0f, -8f);
            }
            else
                iconRect.anchoredPosition = new Vector2(0f, -8f);
        }

        if (quantityText != null)
            quantityText.text = quantity > 0 ? quantity.ToString() : "";
    }
}
