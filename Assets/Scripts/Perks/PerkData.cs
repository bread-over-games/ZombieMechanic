/// scriptable object for perks

using UnityEngine;

[CreateAssetMenu(fileName = "NewPerk", menuName = "Game/Perk")]
public class PerkData : ScriptableObject
{
    public Sprite perkVisual;    
    public string perkName;
    public string perkDescription;
    public string perkLevel;
    public Perk perkEffect;
}
