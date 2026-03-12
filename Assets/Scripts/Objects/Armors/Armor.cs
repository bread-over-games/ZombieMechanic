using UnityEngine;

public class Armor : Object
{
    public override Sprite GetObjectSprite()
    {
        return null;
    }
    public override GameObject GetObjectGameObject()
    {
        return null; 
    }

    public override void SetValues() //when creating new object
    {

    }
    public override void LoadValues(Object existingObject) // when object already exists
    {

    }
    public override void DestroyObject()
    {

    }
}
