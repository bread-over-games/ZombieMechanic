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

    public override void RepairObject(int repairAmount)
    {

    }

    public override bool DamageObject(int decayAmount) // returns true when weapon is destroyed
    {
        currentDurability -= decayAmount;
        //OnBackpackDamage?.Invoke();

        if (currentDurability <= 0)
        {
            return true;
        }

        return false;
    }
}
