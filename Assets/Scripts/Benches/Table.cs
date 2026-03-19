using UnityEngine;

public class Table :  Bench, IInteractable
{
    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Backpack));
        acceptedTypes.Add(typeof(Armor));
        acceptedTypes.Add(typeof(Scrap));
        acceptedTypes.Add(typeof(Antibiotics));
    }
}
