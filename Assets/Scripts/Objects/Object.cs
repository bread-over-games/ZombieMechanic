using UnityEngine;

public abstract class Object : MonoBehaviour
{
    protected Inventory inInventory; // in which inventory the object is

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


}
