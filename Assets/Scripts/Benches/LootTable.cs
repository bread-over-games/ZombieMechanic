/// This is the main source of weapons, salvage, medicine, etc. from outside world. 
/// Player does not work here, only sift thru scavenged loot
/// He unpacks salvage and sends it to storage
/// Or he sends weapons to workbenches to salvage, repair or modify

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LootTable : Bench, IBench
{
    public static Action OnTutorialSparePartsPicked;
    public static Action OnTutorialBaseballBatPicked;

    public override void StartInteractionPrimary()
    {
        base.StartInteractionPrimary();

        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.sparePartsPicked)
            {
                OnTutorialSparePartsPicked?.Invoke();
                return;
            }

            if (!TutorialController.Instance.baseballBatPicked)
            {
                OnTutorialBaseballBatPicked?.Invoke();
            }
        }
    }
}
