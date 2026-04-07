using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIConstructible : MonoBehaviour
{
    private IConstructible currentConstructible;

    [SerializeField] private GameObject constructibleWindow;
    [SerializeField] private GameObject controlTip;
    [SerializeField] private TMP_Text constructibleDescription;
    [SerializeField] private TMP_Text constructibleName;
    [SerializeField] private TMP_Text sparePartsRequired;
    [SerializeField] private Image builtBar;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += ShowConstructibleWindow;
        PlayerInteraction.OnInteractableLeft += HideConstructibleWindow;
        BenchConstruction.OnConstructionTick += UpdateConstructibleInfo;
        ResourceController.OnSparePartsAmountChange += ToggleControlTip;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= ShowConstructibleWindow;
        PlayerInteraction.OnInteractableLeft -= HideConstructibleWindow;
        BenchConstruction.OnConstructionTick -= UpdateConstructibleInfo;
        ResourceController.OnSparePartsAmountChange -= ToggleControlTip;
    }

    private void ToggleControlTip()
    {
        if (currentConstructible == null) return;
        if (currentConstructible.GetSparePartsTickCost() <= ResourceController.Instance.GetSparePartsAmount())
        {
            controlTip.SetActive(true);
        } else
        {
            controlTip.SetActive(false);
        }
    }

    private void ShowConstructibleWindow(IInteractable currentInteractable)
    {
        if (currentInteractable is IConstructible constructible)
        {
            constructibleWindow.SetActive(true);
            currentConstructible = constructible;
            UpdateConstructibleInfo();
            ToggleControlTip();
        }
    }

    private void HideConstructibleWindow(IInteractable currentInteractable)
    {
        if (currentInteractable is IConstructible constructible)
        {
            constructibleWindow.SetActive(false);
            currentConstructible = null;
        }
    }

    private void UpdateConstructibleInfo()
    {
        sparePartsRequired.text = currentConstructible.GetSparePartsRequired().ToString();
        builtBar.fillAmount = currentConstructible.GetCurrentConstructionLevel();
        constructibleDescription.text = currentConstructible.GetDescription();
        constructibleName.text = currentConstructible.GetName();
    }
}
