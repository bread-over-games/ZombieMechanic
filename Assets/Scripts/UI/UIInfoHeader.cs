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

    private void OnEnable()
    {
        ResourceController.OnAntibioticsAmountChange += UpdateAntibioticsAmount;
        ResourceController.OnSparePartsAmountChange += UpdateAmounts;
        ZombiesController.OnZombiesKilledChanged += UpdateZombiesAmount;
    }

    private void OnDisable()
    {
        ResourceController.OnAntibioticsAmountChange -= UpdateAntibioticsAmount;
        ResourceController.OnSparePartsAmountChange -= UpdateAmounts;
        ZombiesController.OnZombiesKilledChanged -= UpdateZombiesAmount;
    }

    private void Start()
    {
        UpdateAmounts();
        UpdateAntibioticsAmount();
        UpdateZombiesAmount();
    }

    private void UpdateAntibioticsAmount()
    {
        antibioticsAmount.text = ResourceController.Instance.GetAntibioticsAmount().ToString();
        antibioticsPulse.Pulse();
    }

    private void UpdateAmounts()
    {
        sparePartsAmount.text = ResourceController.Instance.GetSparePartsAmount().ToString() + "/" + 500.ToString();
        sparePartsPulse.Pulse();
    }

    private void UpdateZombiesAmount()
    {
        zombiesKilledAmount.text = ZombiesController.Instance.zombiesKilledTotal.ToString() + "/" + ZombiesController.Instance.zombiesKillVictoryGoal.ToString();
        zombiesKilledPulse.Pulse();
    }
}
