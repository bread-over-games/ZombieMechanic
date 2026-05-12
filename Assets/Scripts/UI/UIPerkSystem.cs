using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;

public class UIPerkSystem : MonoBehaviour
{
    public enum PerkSlot
    {
        FirstPerkSlot,
        SecondPerkSlot
    }

    [SerializeField] private GameObject perkSystemWindow;

    [Header("First perk info")]
    [SerializeField] private TMP_Text firstPerkName;
    [SerializeField] private TMP_Text firstPerkDescription;
    [SerializeField] private Image firstPerkImage;

    [Header("Second perk info")]
    [SerializeField] private TMP_Text secondPerkName;
    [SerializeField] private TMP_Text secondPerkDescription;
    [SerializeField] private Image secondPerkImage;

    [HideInInspector] public PerkSlot currentSlotSelected;
    public static Action OnUIPerkWindowActive;
    public static Action<PerkSlot> OnCurrentPerkSlotSelected;
    [SerializeField] private Button defaultSelectedButton;

    private void OnEnable()
    {
        PerkController.OnRandomPerksGenerated += ShowPerkWindow;
        InputDeviceTracker.OnSwitchedToGamepad += SelectDefaultButton;
    }

    private void OnDisable()
    {
        PerkController.OnRandomPerksGenerated -= ShowPerkWindow;
        InputDeviceTracker.OnSwitchedToGamepad -= SelectDefaultButton;
    }

    private void ShowPerkWindow()
    {
        PlayerInteraction.OnPrimaryInteractionInterceptor = ActivatePerk;
        UIFocusStack.Push(perkSystemWindow);     
        DisplayPerks();
        OnUIPerkWindowActive?.Invoke();
    }

    private void HidePerkWindow()
    {
        EventSystem.current.SetSelectedGameObject(null);
        UIFocusStack.Pop();
        PlayerInteraction.OnPrimaryInteractionInterceptor = null;
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

    public void FirstPerkSelected()
    {
        currentSlotSelected = PerkSlot.FirstPerkSlot;
    }

    public void SecondPerkSelected()
    {
        currentSlotSelected = PerkSlot.SecondPerkSlot;
    }

    private void SelectDefaultButton()
    {
        if (perkSystemWindow.activeSelf)
            defaultSelectedButton.Select();
    }
}
