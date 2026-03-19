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
    }

    private void UpdateAntibioticsAmount()
    {
        antibioticsAmount.text = ResourceController.Instance.GetAntibioticsAmount().ToString();
    }

    private void UpdateAmounts()
    {
        sparePartsAmount.text = ResourceController.Instance.GetSparePartsAmount().ToString();
    }

    private void UpdateZombiesAmount()
    {
        zombiesKilledAmount.text = ZombiesController.Instance.zombiesKilledTotal.ToString() + "/" + ZombiesController.Instance.zombiesKillVictoryGoal.ToString();
    }
}
