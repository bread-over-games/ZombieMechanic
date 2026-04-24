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

    private IEnumerator ObjectSpawnEffect(GameObject spawnedObject, Transform spawnTransform)
    {
        if (spawnedObject == null) yield break;
        if (inventory.GetInventoryOfType() == Inventory.InventoryOfType.Player) yield break;

        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 startPos = spawnTransform.position + Vector3.up * 0.35f;
        spawnedObject.transform.position = startPos;

        // Drop phase — target follows the animated transform each frame
        while (elapsed < duration)
        {
            if (spawnedObject == null) yield break;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            spawnedObject.transform.position = Vector3.Lerp(startPos, spawnTransform.position, t);
            yield return null;
        }

        if (spawnedObject == null) yield break;

        // Land shake — DOTween is fine here since position is now settled
        spawnedObject.transform
            .DOShakePosition(0.1f, strength: 0.02f, vibrato: 15, randomness: 45)
            .SetLink(spawnedObject);
    }

    private void DoMultipleObjectsDisplay()
    {
        ClearCurrentObjects();

        GameObject spawnedObject = null;
        Transform spawnPosition = null;

        for (int i = 0; i < inventory.GetObjectList().Count; i++)
        {
            switch (inventory.GetObjectList()[i])
            {
                case Weapon weapon:
                    spawnedObject = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot);
                    spawnPosition = weaponSpawnPivot;
                    currentObjects.Add(spawnedObject);
                    break;
                case Backpack backpack:
                    spawnedObject = BackpackWorld.SpawnBackpackWorld(backpackSpawnPivot.position, backpack, backpackSpawnPivot);
                    spawnPosition = backpackSpawnPivot;
                    currentObjects.Add(spawnedObject);
                    break;
                case Armor armor:
                    spawnedObject = ArmorWorld.SpawnArmorWorld(armorSpawnPivot.position, armor, armorSpawnPivot);
                    spawnPosition = armorSpawnPivot;
                    currentObjects.Add(spawnedObject);
                    break;
                case Scrap scrap:
                    spawnedObject = ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot);
                    spawnPosition = weaponSpawnPivot;
                    currentObjects.Add(spawnedObject);
                    break;
                case Antibiotics antibiotics:
                    spawnedObject = AntibioticsWorld.SpawnAntibioticsWorld(weaponSpawnPivot.position, antibiotics, weaponSpawnPivot);
                    spawnPosition = weaponSpawnPivot;
                    currentObjects.Add(spawnedObject);
                    break;
            }

            StartCoroutine(ObjectSpawnEffect(spawnedObject, spawnPosition));
        }


    }

    private void DoObjectDisplay()
    {
        GameObject spawnedObject = null;
        Transform spawnPosition = weaponSpawnPivot;

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

        StartCoroutine(ObjectSpawnEffect(spawnedObject, spawnPosition));
    }

    private void ClearCurrentObjects()
    {        
        foreach (GameObject currentObject in currentObjects)
        {
            if (currentObject != null)
            {
                currentObject.transform.DOKill();
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
