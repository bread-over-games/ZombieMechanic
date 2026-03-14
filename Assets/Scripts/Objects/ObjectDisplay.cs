/// Displays or destroys given object in the world

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class ObjectDisplay : MonoBehaviour
{
    [SerializeField]private int displaySlots = 1;
    private List<GameObject> currentObjects = new List<GameObject>();
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform weaponSpawnPivot;
    [SerializeField] private Transform backpackSpawnPivot;
    [SerializeField] private Transform armorSpawnPivot;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += DisplayCurrentObject;
        Inventory.OnObjectSend += DestroyCurrentObject;
        Object.OnObjectDestroyed += DestroyMyObject;
    }

    public void StartInteractionPrimary()
    {
        Inventory.OnObjectReceive -= DisplayCurrentObject;
        Inventory.OnObjectSend -= DestroyCurrentObject;
        Object.OnObjectDestroyed -= DestroyMyObject;
    }

    private void TryDisplayCurrentObjects()
    {
        if (inventory.GetObjectList().Count > 0 && currentObjects.Count == 0)
        {
            if (displaySlots == 1)
            {
                DoObjectDisplay();
            }
        }

        if (displaySlots > 1)
        {
            DoMultipleObjectsDisplay();
        }
    }

    private void DisplayCurrentObject(Inventory.InventoryOfType inventoryOfType, Object obj)
    {
        if (inventoryOfType != inventory.GetInventoryOfType())
        {
            return;
        }

        TryDisplayCurrentObjects();
    }

    private void DoMultipleObjectsDisplay()
    {
        ClearCurrentObjects();
        for (int i = 0; i < inventory.GetObjectList().Count; i++)
        {
            switch (inventory.GetObjectList()[i])
            {
                case Weapon weapon:
                    currentObjects.Add(WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot));
                    break;
                case Backpack backpack:
                    currentObjects.Add(BackpackWorld.SpawnBackpackWorld(backpackSpawnPivot.position, backpack, backpackSpawnPivot));
                    break;
                case Armor armor:
                    currentObjects.Add(ArmorWorld.SpawnArmorWorld(armorSpawnPivot.position, armor, armorSpawnPivot));
                    break;
                case Scrap scrap:
                    currentObjects.Add(ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot));
                    break;
                    /*    case Medicine medicine:
                            medicine.LoadValues(medicine);
                            break;*/
            }
        }
    }

    private void DoObjectDisplay()
    {
        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                currentObjects.Add(WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot));
                break;
            case Backpack backpack:
                currentObjects.Add(BackpackWorld.SpawnBackpackWorld(weaponSpawnPivot.position, backpack, weaponSpawnPivot));
                break;
            case Armor armor:
                currentObjects.Add(ArmorWorld.SpawnArmorWorld(weaponSpawnPivot.position, armor, weaponSpawnPivot));                
                break;
            case Scrap scrap:
                currentObjects.Add(ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot));
                break;
                /*    case Medicine medicine:
                        medicine.LoadValues(medicine);
                        break;*/
        }
    }

    private void ClearCurrentObjects()
    {
        foreach (GameObject currentObject in currentObjects)
        {
            if (currentObject != null)
            {
                Destroy(currentObject);
            }
        }

        currentObjects.Clear();
    }

    public void DestroyCurrentObject(Inventory.InventoryOfType invOfType, Object obj)
    {
        if (inventory.GetInventoryOfType() != invOfType)
        { 
            return;
        }

        ClearCurrentObjects();

        TryDisplayCurrentObjects();
    }

    private void DestroyMyObject(Inventory invOfType) // determines if it should destroy object on this interactable
    {
        if (inventory.GetInventoryOfType() == invOfType.GetInventoryOfType())
        {
            DestroyCurrentObject(invOfType.GetInventoryOfType(), null);
        }
    }
}
