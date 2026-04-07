using UnityEngine;
using System;

public class StorageRack : Bench, IBench
{
    [SerializeField] private int storageSpace;

    public static Action<int> OnStorageRackBuilt;    

    private void Start()
    {
        OnStorageRackBuilt?.Invoke(storageSpace);
    }    
}