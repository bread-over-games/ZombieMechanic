using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    public UIInventory interactableInvSingleItem;
    public UIArmory armoryUI;

    public static UIController Instance {  get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
