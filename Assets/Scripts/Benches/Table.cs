using UnityEngine;

public class Table :  Bench, IInteractable
{
    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Scrap));
    }
}
