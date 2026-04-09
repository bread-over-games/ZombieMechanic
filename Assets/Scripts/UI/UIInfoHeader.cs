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

    private void OnEnable()
    {
        ResourceController.OnAntibioticsAmountChange += UpdateAntibioticsAmount;
        ResourceController.OnSparePartsAmountChange += UpdateSparePartsAmounts;
        ResourceController.OnSparePartsLimitReached += FlashSparePartsRed;
        ZombiesController.OnZombiesKilledChanged += UpdateZombiesAmount;
    }

    private void OnDisable()
    {
        ResourceController.OnAntibioticsAmountChange -= UpdateAntibioticsAmount;
        ResourceController.OnSparePartsAmountChange -= UpdateSparePartsAmounts;
        ResourceController.OnSparePartsLimitReached -= FlashSparePartsRed;
        ZombiesController.OnZombiesKilledChanged -= UpdateZombiesAmount;
    }

    private void Start()
    {
        UpdateSparePartsAmounts();
        UpdateAntibioticsAmount();
        UpdateZombiesAmount();
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

    private void UpdateSparePartsAmounts()
    {
        sparePartsAmount.text = ResourceController.Instance.GetSparePartsAmount().ToString() + "/" + ResourceController.Instance.GetSparePartsLimit().ToString();
        sparePartsPulse.Pulse();
    }

    private void UpdateZombiesAmount()
    {
        zombiesKilledAmount.text = ZombiesController.Instance.zombiesKilledTotal.ToString() + "/" + ZombiesController.Instance.zombiesKillVictoryGoal.ToString();
        zombiesKilledPulse.Pulse();
    }
}
