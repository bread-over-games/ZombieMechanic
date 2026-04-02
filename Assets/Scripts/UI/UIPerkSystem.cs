using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIPerkSystem : MonoBehaviour
{
    [SerializeField] private GameObject firstSlotSelected;
    [SerializeField] private GameObject perkSystemWindow;

    [HideInInspector] public ButtonSelectorPerks.PerkSlot currentSlotSelected;
    public static Action OnUIPerkWindowActive;
    public static Action<ButtonSelectorPerks.PerkSlot> OnCurrentPerkSlotSelected;

    private void OnEnable()
    {
        XPCounter.OnLevelUp += ShowPerkWindow;
        PlayerInteraction.OnPerkActivated += ActivatePerk;
    }

    private void OnDisable()
    {
        XPCounter.OnLevelUp -= ShowPerkWindow;
        PlayerInteraction.OnPerkActivated -= ActivatePerk;
    }

    private void ShowPerkWindow()
    {
        perkSystemWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSlotSelected);
        OnUIPerkWindowActive?.Invoke();
    }

    private void HidePerkWindow()
    {
        EventSystem.current.SetSelectedGameObject(null);
        perkSystemWindow.SetActive(false);
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
