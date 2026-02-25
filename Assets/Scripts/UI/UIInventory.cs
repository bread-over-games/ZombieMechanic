using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform weaponSlotContainer;
    [SerializeField] private Transform weaponSlotTemplate;

    private void OnEnable()
    {
        Inventory.OnWeaponReceive += RefreshWeaponItems;
        Inventory.OnWeaponSend += RefreshWeaponItems;
    }

    private void OnDisable()
    {
        Inventory.OnWeaponReceive -= RefreshWeaponItems;
        Inventory.OnWeaponSend -= RefreshWeaponItems;
    }

    private void RefreshWeaponItems()
    {
        foreach (Transform child in weaponSlotContainer)
        {
            if (child == weaponSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        if (inventory.GetWeaponList().Count == 0)
        {
            return;
        }

        RectTransform weaponSlotRectTransform = Instantiate(weaponSlotTemplate, weaponSlotContainer).GetComponent<RectTransform>();
        weaponSlotRectTransform.gameObject.SetActive(true);

        Image image = weaponSlotRectTransform.Find("Image").GetComponent<Image>();
        TMP_Text baseDamageText = weaponSlotRectTransform.Find("Damage").GetComponent<TMP_Text>();
        TMP_Text durabilityText = weaponSlotRectTransform.Find("Durability").GetComponent<TMP_Text>();
        image.sprite = inventory.GetWeaponList()[0].GetWeaponSprite();
        baseDamageText.text = inventory.GetWeaponList()[0].baseDamage.ToString() + "+" + inventory.GetWeaponList()[0].bonusDamage.ToString();
        durabilityText.text = inventory.GetWeaponList()[0].currentDurability.ToString() + "/" + inventory.GetWeaponList()[0].maxDurability.ToString();
    }
}