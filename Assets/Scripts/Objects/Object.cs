using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public abstract Sprite GetObjectSprite();
    public abstract GameObject GetObjectGameObject();
    public abstract void SetValues(); //when creating new object
    public abstract void LoadValues(Object existingObject); // when object already exists
    public abstract void DestroyObject(Inventory currentlyInInventory);


}
