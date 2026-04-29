using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIInfoHeader : MonoBehaviour
{
    [SerializeField] private TMP_Text antibioticsAmount;
    [SerializeField] private TMP_Text sparePartsAmount;
    [SerializeField] private TMP_Text zombiesKilledAmount;

    [SerializeField] private ScalePulse antibioticsPulse;
    [SerializeField] private ScalePulse sparePartsPulse;
    [SerializeField] private ScalePulse zombiesKilledPulse;

    [SerializeField] private TextFlasher sparePartsTextFlasher;

    [SerializeField] private Image zombieKilledBar;

    private void OnEnable()
    {
        ResourceController.OnSparePartsLimitReached += FlashSparePartsRed;
        UIFlyoutVisual.OnFlyoutReachedDestination += UpdateAmounts;
        ResourceController.OnAntibioticsAmountChange += UpdateAntibioticsAmount;
    }

    private void OnDisable()
    {
        ResourceController.OnSparePartsLimitReached -= FlashSparePartsRed;
        UIFlyoutVisual.OnFlyoutReachedDestination -= UpdateAmounts;
        ResourceController.OnAntibioticsAmountChange -= UpdateAntibioticsAmount;
    }

    private void Start()
    {
        antibioticsAmount.text = ResourceController.Instance.GetAntibioticsAmount().ToString();
        zombiesKilledAmount.text = ZombiesController.Instance.zombiesKilledTotal.ToString();
        sparePartsAmount.text = ResourceController.Instance.GetSparePartsAmount().ToString() + "/" + ResourceController.Instance.GetSparePartsLimit().ToString();
    }

    private void UpdateAmounts(UIFlyoutVisual.FlyoutTypes type)
    {
        switch (type)
        {
            case UIFlyoutVisual.FlyoutTypes.Zombies:
                zombiesKilledAmount.text = ZombiesController.Instance.zombiesKilledTotal.ToString();
                zombieKilledBar.fillAmount = (float)ZombiesController.Instance.zombiesKilledTotal / ZombiesController.Instance.zombiesKillVictoryGoal;
                zombiesKilledPulse.Pulse();
                break;
            case UIFlyoutVisual.FlyoutTypes.SpareParts:
                sparePartsAmount.text = ResourceController.Instance.GetSparePartsAmount().ToString() + "/" + ResourceController.Instance.GetSparePartsLimit().ToString();
                sparePartsPulse.Pulse();
                break;
            case UIFlyoutVisual.FlyoutTypes.Antibiotics:
                antibioticsAmount.text = ResourceController.Instance.GetAntibioticsAmount().ToString();
                antibioticsPulse.Pulse();
                break;
        }
    }

    private void FlashSparePartsRed()
    {
        sparePartsTextFlasher.Flash();
    }

    private void UpdateAntibioticsAmount()
    {
        antibioticsAmount.text = ResourceController.Instance.GetAntibioticsAmount().ToString();
        antibioticsPulse.Pulse();
    }
}
