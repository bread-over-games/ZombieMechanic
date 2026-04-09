using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Game/Armor")]
public class ArmorData : ScriptableObject
{
    public Sprite armorVisual;
    public GameObject armorVisualPrefab;
    public string armorName;    
    public int maxDurability;
    public int lootQualityBonus;
    public int spawnChance;
}
