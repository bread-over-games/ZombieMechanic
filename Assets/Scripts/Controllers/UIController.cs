using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    public UIInventory interactableInvSingleItem;

    public static UIController Instance {  get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
