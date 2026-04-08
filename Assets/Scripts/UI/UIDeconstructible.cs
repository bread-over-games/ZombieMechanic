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
        ObjectDeconstruction.OnDeconstructionTick += UpdateDeconstructibleInfo;
        PlayerInteraction.OnInteractableApproached += ShowDeconstructibleWindow;
        PlayerInteraction.OnInteractableLeft += HideDeconstructibleWindow;
    }

    private void OnDisable()
    {
        ObjectDeconstruction.OnDeconstructionTick -= UpdateDeconstructibleInfo;
        PlayerInteraction.OnInteractableApproached -= ShowDeconstructibleWindow;
        PlayerInteraction.OnInteractableLeft -= HideDeconstructibleWindow;
    }

    private void ShowDeconstructibleWindow(IInteractable currentInteractable)
    {
        if (currentInteractable is IDeconstructible deconstructible)
        {
            deconstructibleWindow.SetActive(true);
            currentDeconstructible = deconstructible;
            UpdateDeconstructibleInfo();            
        }
    }

    private void HideDeconstructibleWindow(IInteractable currentInteractable)
    {
        if (currentInteractable is IDeconstructible deconstructible)
        {
            deconstructibleWindow.SetActive(false);
            currentDeconstructible = null;
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
