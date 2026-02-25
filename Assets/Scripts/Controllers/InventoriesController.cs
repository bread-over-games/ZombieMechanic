/// Holds reference to all inventories in the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesController : MonoBehaviour
{
    public static InventoriesController Instance { get; private set; }

    public List<Transform> storageInventories = new List<Transform>();
    public Transform armoryInventory;
    public Transform lootTableInventory;
    public Transform workbenchInventory;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
