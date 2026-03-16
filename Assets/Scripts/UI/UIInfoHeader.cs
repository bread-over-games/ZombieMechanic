using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIInfoHeader : MonoBehaviour
{
    [SerializeField] private TMP_Text sparePartsAmount;
    [SerializeField] private TMP_Text zombiesKilledAmount;

    private void OnEnable()
    {
        ResourceController.OnSparePartsAmountChange += UpdateAmounts;
    }

    private void OnDisable()
    {
        ResourceController.OnSparePartsAmountChange -= UpdateAmounts;
    }

    private void Start()
    {
        UpdateAmounts();
    }

    private void UpdateAmounts()
    {
        sparePartsAmount.text = ResourceController.Instance.GetSparePartsAmount().ToString();
    }
}
