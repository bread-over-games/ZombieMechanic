using UnityEngine;

[CreateAssetMenu(fileName = "NewAntibiotics", menuName = "Game/Antibiotics")]
public class AntibioticsData : ScriptableObject
{
    public Sprite antibioticsVisual;
    public GameObject antibioticsVisualPrefab;
    public string antibioticsName;
    public int maxAmount;
}
