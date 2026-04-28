using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDeconstructible : MonoBehaviour
{
    private IDeconstructible currentDeconstructible;

    [SerializeField] private GameObject deconstructibleWindow;    
    [SerializeField] private TMP_Text deconstructibleDescription;
    [SerializeField] private TMP_Text deconstructibleName;
    [SerializeField] private TMP_Text sparePartsLeft;
    [SerializeField] private Image durabilityBar;

    private void OnEnable()
    {
        ObjectDeconstruction.OnDeconstructionTick += _ => UpdateDeconstructibleInfo();
        PlayerInteraction.OnInteractableApproached += ShowDeconstructibleWindow;
        PlayerInteraction.OnInteractableLeft += HideDeconstructibleWindow;
    }

    private void OnDisable()
    {
        ObjectDeconstruction.OnDeconstructionTick -= _ => UpdateDeconstructibleInfo();
        PlayerInteraction.OnInteractableApproached -= ShowDeconstructibleWindow;
        PlayerInteraction.OnInteractableLeft -= HideDeconstructibleWindow;
    }

    private void ShowDeconstructibleWindow(IInteractable currentInteractable)
    {
        if (currentInteractable is IDeconstructible deconstructible)
        {
            UIFocusStack.Push(deconstructibleWindow);            
            currentDeconstructible = deconstructible;
            UpdateDeconstructibleInfo();            
        }
    }

    private void HideDeconstructibleWindow(IInteractable currentInteractable)
    {
        if (currentInteractable is IDeconstructible deconstructible)
        {            
            currentDeconstructible = null;
            UIFocusStack.Pop();
        }
    }

    private void UpdateDeconstructibleInfo()
    {
        sparePartsLeft.text = currentDeconstructible.GetSparePartsLeft().ToString();
        durabilityBar.fillAmount = currentDeconstructible.GetCurrentDurabilityLevel();
        deconstructibleDescription.text = currentDeconstructible.GetDescription();
        deconstructibleName.text = currentDeconstructible.GetName();
    }
}
