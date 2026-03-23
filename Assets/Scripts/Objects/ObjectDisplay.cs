/// Displays or destroys given object in the world

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static Inventory;
using DG.Tweening;
using UnityEngine.UIElements;


public class ObjectDisplay : MonoBehaviour
{
    [SerializeField] private int displaySlots = 1;
    [SerializeReference] private List<GameObject> currentObjects = new List<GameObject>();
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

    private void DisplayCurrentObject(Object obj, Inventory myInventory)
    {
        if (myInventory != inventory)
        {
            return;
        }

        TryDisplayCurrentObjects();
    }

    private void TryDisplayCurrentObjects()
    {
        {
            if (inventory.GetObjectList().Count == 0)
            {
                return;
            }

            if (displaySlots == 1 && currentObjects.Count == 0)
            {
                DoObjectDisplay();
            }
            else if (displaySlots > 1)
            {
                DoMultipleObjectsDisplay();
            }
        }
    }

    private void ObjectSpawnEffect(GameObject spawnedObject, Vector3 spawnPosition)
    {
        if (spawnedObject == null)
        {
            return; 
        }

        if (inventory.GetInventoryOfType() == Inventory.InventoryOfType.Player)
        {
            return;
        }

        // Start slightly above the target position
        Vector3 dropFrom = spawnPosition + Vector3.up * 0.35f;
        spawnedObject.transform.position = dropFrom;

        Sequence spawnSequence = DOTween.Sequence();

        // Drop down to the final position
        spawnSequence.Append(
            spawnedObject.transform.DOMove(spawnPosition, 0.2f)

        );

        // Shake in place after landing
        spawnSequence.Append(
            spawnedObject.transform.DOShakePosition(0.1f, strength: 0.02f, vibrato: 15, randomness: 45)
        );
    }

    private void DoMultipleObjectsDisplay()
    {
        ClearCurrentObjects();

        GameObject spawnedObject = null;
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < inventory.GetObjectList().Count; i++)
        {
            switch (inventory.GetObjectList()[i])
            {
                case Weapon weapon:
                    spawnedObject = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot);
                    spawnPosition = weaponSpawnPivot.position;
                    currentObjects.Add(spawnedObject);
                    break;
                case Backpack backpack:
                    spawnedObject = BackpackWorld.SpawnBackpackWorld(backpackSpawnPivot.position, backpack, backpackSpawnPivot);
                    spawnPosition = backpackSpawnPivot.position;
                    currentObjects.Add(spawnedObject);
                    break;
                case Armor armor:
                    spawnedObject = ArmorWorld.SpawnArmorWorld(armorSpawnPivot.position, armor, armorSpawnPivot);
                    spawnPosition = armorSpawnPivot.position;
                    currentObjects.Add(spawnedObject);
                    break;
                case Scrap scrap:
                    spawnedObject = ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot);
                    spawnPosition = weaponSpawnPivot.position;
                    currentObjects.Add(spawnedObject);
                    break;
                case Antibiotics antibiotics:
                    spawnedObject = AntibioticsWorld.SpawnAntibioticsWorld(weaponSpawnPivot.position, antibiotics, weaponSpawnPivot);
                    spawnPosition = weaponSpawnPivot.position;
                    currentObjects.Add(spawnedObject);
                    break;
            }
        }

        ObjectSpawnEffect(spawnedObject, spawnPosition);
    }

    private void DoObjectDisplay()
    {
        GameObject spawnedObject = null;
        Vector3 spawnPosition = weaponSpawnPivot.position;

        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                spawnedObject = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot);
                currentObjects.Add(spawnedObject);
                break;
            case Backpack backpack:
                spawnedObject = BackpackWorld.SpawnBackpackWorld(weaponSpawnPivot.position, backpack, weaponSpawnPivot);
                currentObjects.Add(spawnedObject);
                break;
            case Armor armor:
                spawnedObject = ArmorWorld.SpawnArmorWorld(weaponSpawnPivot.position, armor, weaponSpawnPivot);
                currentObjects.Add(spawnedObject);                
                break;
            case Scrap scrap:
                spawnedObject = ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot);
                currentObjects.Add(spawnedObject);
                break;
            case Antibiotics antibiotics:
                spawnedObject = AntibioticsWorld.SpawnAntibioticsWorld(weaponSpawnPivot.position, antibiotics, weaponSpawnPivot);
                currentObjects.Add(spawnedObject);
                break;
        }

        ObjectSpawnEffect(spawnedObject, spawnPosition);
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

    public void DestroyCurrentObject(Object obj, Inventory myInventory)
    {       
        if (inventory != myInventory)
        { 
            return;
        }        

        ClearCurrentObjects();

        TryDisplayCurrentObjects();
    }

    private void DestroyMyObject(Inventory myInventory) // determines if it should destroy object on this interactable
    {
        if (inventory == myInventory)
        {
            DestroyCurrentObject(null, myInventory);
        }
    }
}
