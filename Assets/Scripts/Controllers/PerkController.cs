using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PerkController : MonoBehaviour
{
    private List<PerkData> activePerks = new List<PerkData>(); // perks player has chosen
    public List<PerkData> availablePerks = new List<PerkData>(); // all perks in the game minus those which player has already chosen

    [HideInInspector] public PerkData firstPerkPick;
    [HideInInspector] public PerkData secondPerkPick;

    public static PerkController Instance { get; private set; }
    public static Action OnRandomPerksGenerated;
    public static Action OnPerkSelected;

    private void OnEnable()
    {
        XPCounter.OnLevelUp += GenerateRandomPerks;
        UIPerkSystem.OnCurrentPerkSlotSelected += PlayerChosePerk;
    }

    private void OnDisable()
    {
        XPCounter.OnLevelUp -= GenerateRandomPerks;
        UIPerkSystem.OnCurrentPerkSlotSelected -= PlayerChosePerk;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void GenerateRandomPerks()
    {
        List<PerkData> availablePerksWOFirstPick = new List<PerkData>(availablePerks); // temporary storage for generating secondPerkPick
        firstPerkPick = availablePerksWOFirstPick[UnityEngine.Random.Range(0, availablePerks.Count)];
        availablePerksWOFirstPick.Remove(firstPerkPick);
        secondPerkPick = availablePerksWOFirstPick[UnityEngine.Random.Range(0, availablePerksWOFirstPick.Count)];

        OnRandomPerksGenerated?.Invoke();
    }

    private void PlayerChosePerk(UIPerkSystem.PerkSlot perkSlot)
    {
        if (perkSlot == UIPerkSystem.PerkSlot.FirstPerkSlot)
        {
            firstPerkPick.perkEffect.ActivatePerk();
            activePerks.Add(firstPerkPick);
            availablePerks.Remove(firstPerkPick);            

        } else
        {
            secondPerkPick.perkEffect.ActivatePerk();
            activePerks.Add(secondPerkPick);
            availablePerks.Remove(secondPerkPick);
        }

        firstPerkPick = null;
        secondPerkPick = null;   
        OnPerkSelected?.Invoke();   
    }
}
