using System;
using UnityEngine;

[System.Serializable]
public abstract class Object 
{
    protected Inventory inInventory; // in which inventory the object is
    public int maxDurability;
    public int currentDurability;
    public string objectName;

    public static Action OnObjectRepair; // called when weapon is repaired
    public static Action OnObjectDamage;
    public static Action<Inventory> OnObjectDestroyed;

    public void AssignOwnerInventory(Inventory ownerInventory)
    {
        inInventory = ownerInventory;
    }

    public void ClearOwnerInventory()
    {
        inInventory = null;
    }

    public abstract Sprite GetObjectSprite();
    public abstract GameObject GetObjectGameObject();
    public abstract void SetValues(); //when creating new object
    public abstract void LoadValues(Object existingObject); // when object already exists
    public abstract void DestroyObject();
    public abstract void RepairObject(int repairAmount);
    public abstract bool DamageObject(int decayAmount); // returns true when weapon is destroyed


}
