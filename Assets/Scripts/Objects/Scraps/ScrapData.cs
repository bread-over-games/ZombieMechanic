/// scriptable object for weapons

using UnityEngine;

[CreateAssetMenu(fileName = "NewScrap", menuName = "Game/Scrap")]
public class ScrapData : ScriptableObject
{
    public string scrapName;
    public int maxDurability;
    public GameObject scrapVisualPrefab;
    public Sprite scrapVisual;
}
