/// scriptable object for backpack

using UnityEngine;

[CreateAssetMenu(fileName = "NewBackpack", menuName = "Game/Backpack")]
public class BackpackData : ScriptableObject
{
    public Sprite backpackVisual;
    public GameObject backpackVisualPrefab;
    public int backpackSize;
    public string backpackName;
    public int maxDurability;
    public int spawnChance;
}
