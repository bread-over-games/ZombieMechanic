/// Holds reference to all inventories in the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesController : MonoBehaviour
{
    public static InventoriesController Instance { get; private set; }

    public List<Inventory> storageInventories = new List<Inventory>();
    public Inventory armoryInventory;
    public Inventory lootTableInventory;
    public Inventory workbenchInventory;
    public Inventory outsideInventory;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
