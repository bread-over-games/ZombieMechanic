using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;

public class UIPerkSystem : MonoBehaviour
{
    [SerializeField] private GameObject firstSlotSelected;
    [SerializeField] private GameObject perkSystemWindow;

    [Header("First perk info")]
    [SerializeField] private TMP_Text firstPerkName;
    [SerializeField] private TMP_Text firstPerkDescription;
    [SerializeField] private Image firstPerkImage;

    [Header("Second perk info")]
    [SerializeField] private TMP_Text secondPerkName;
    [SerializeField] private TMP_Text secondPerkDescription;
    [SerializeField] private Image secondPerkImage;

    [HideInInspector] public ButtonSelectorPerks.PerkSlot currentSlotSelected;
    public static Action OnUIPerkWindowActive;
    public static Action<ButtonSelectorPerks.PerkSlot> OnCurrentPerkSlotSelected;

    private void OnEnable()
    {
        PerkController.OnRandomPerksGenerated += ShowPerkWindow;
        PlayerInteraction.OnPerkActivated += ActivatePerk;
    }

    private void OnDisable()
    {
        PerkController.OnRandomPerksGenerated -= ShowPerkWindow;
        PlayerInteraction.OnPerkActivated -= ActivatePerk;
    }

    private void ShowPerkWindow()
    {
        perkSystemWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSlotSelected);
        DisplayPerks();
        OnUIPerkWindowActive?.Invoke();
        UIFocusStack.Push(perkSystemWindow);
    }

    private void HidePerkWindow()
    {
        EventSystem.current.SetSelectedGameObject(null);
        perkSystemWindow.SetActive(false);
        UIFocusStack.Pop();
    }

    public void DisplayPerks()
    {
        PerkData firstPerk = PerkController.Instance.firstPerkPick;
        PerkData secondPerk = PerkController.Instance.secondPerkPick;

        firstPerkName.text = firstPerk.perkName;
        firstPerkDescription.text = firstPerk.perkDescription;
        firstPerkImage.sprite = firstPerk.perkVisual;

        secondPerkName.text = secondPerk.perkName;
        secondPerkDescription.text = secondPerk.perkDescription;
        secondPerkImage.sprite = secondPerk.perkVisual;
    }

    public virtual void ActivatePerk()
    {
        OnCurrentPerkSlotSelected?.Invoke(currentSlotSelected);
        HidePerkWindow();
    }

    public void OnButtonSelected(ButtonSelectorPerks.PerkSlot perkSlot)
    {
        currentSlotSelected = perkSlot;
    }

    public void OnButtonDeselected(ButtonSelectorPerks.PerkSlot perkSlot)
    {

    }
}
