using UnityEngine;
using System;

public class StorageRack : Bench, IInteractable
{
    [SerializeField] private int storageSpace;

    public static Action<int> OnStorageRackBuilt;    

    private void Start()
    {
        OnStorageRackBuilt?.Invoke(storageSpace);
    }    
}