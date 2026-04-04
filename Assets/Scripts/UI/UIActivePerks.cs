using UnityEngine;

public class UIActivePerks : MonoBehaviour
{
    [Header("Perk references")]
    [SerializeField] private GameObject infectionPausePerk;
    [SerializeField] private GameObject instantSalvagePerk;
    [SerializeField] private GameObject luckyBastardPerk;
    [SerializeField] private GameObject repairMasteryPerk;
    [SerializeField] private GameObject salvageMasteryPerk;
    [SerializeField] private GameObject zombieSlayerPerk;

    [Header("Pulse references")]
    [SerializeField] private ScalePulse infectionPausePulse;
    [SerializeField] private ScalePulse instantSalvagePulse;
    [SerializeField] private ScalePulse luckyBastardPulse;
    [SerializeField] private ScalePulse repairMasteryPulse;
    [SerializeField] private ScalePulse salvageMasteryPulse;
    [SerializeField] private ScalePulse zombieSlayerPulse;

    private void OnEnable()
    {
        InfectionPause.OnInfectioanPauseActivated += ShowInfectionPause;
        InfectionPauseHandler.OnInfectionResume += HideInfectionPause;
        InstantSalvage.OnInstantSalvageActivated += ShowInstantSalvage;
        LuckyBastard.OnLuckyBastardActivated += ShowLuckyBastard;
        RepairMastery.OnRepairMasteryActivated += ShowRepairMastery;
        SalvageMastery.OnSalvageMasteryActivated += ShowSalvageMastery;
        ZombieSlayer.OnZombieSlayerActivated += ShowZombieSlayer;

        SalvageMasteryHandler.OnSalvageMasteryProc += PulseSalvageMastery;
        RepairMasteryHandler.OnRepairMasteryProc += PulseRepairMastery;
        InstantSalvageHandler.OnSalvageInstantly += PulseInstantSalvage;
        LuckyBastardHandler.OnLuckyBastardProc += PulseLuckyBastard;
    }

    private void OnDisable()
    {
        InfectionPause.OnInfectioanPauseActivated -= ShowInfectionPause;
        InfectionPauseHandler.OnInfectionResume -= HideInfectionPause;
        InstantSalvage.OnInstantSalvageActivated -= ShowInstantSalvage;
        LuckyBastard.OnLuckyBastardActivated -= ShowLuckyBastard;
        RepairMastery.OnRepairMasteryActivated -= ShowRepairMastery;
        SalvageMastery.OnSalvageMasteryActivated -= ShowSalvageMastery;
        ZombieSlayer.OnZombieSlayerActivated -= ShowZombieSlayer;

        SalvageMasteryHandler.OnSalvageMasteryProc -= PulseSalvageMastery;
        RepairMasteryHandler.OnRepairMasteryProc -= PulseRepairMastery;
        InstantSalvageHandler.OnSalvageInstantly -= PulseInstantSalvage;
        LuckyBastardHandler.OnLuckyBastardProc -= PulseLuckyBastard;
    }

    private void ShowInfectionPause()
    {
        infectionPausePerk.SetActive(true);
        PulseInfectionPause();
    }

    private void HideInfectionPause()
    {
        infectionPausePerk.SetActive(false);
    }

    private void ShowInstantSalvage()
    {
        instantSalvagePerk.SetActive(true);
        PulseInstantSalvage();
    }

    private void ShowLuckyBastard()
    {
        luckyBastardPerk.SetActive(true);
        PulseLuckyBastard();    
    }

    private void ShowRepairMastery()
    {
        repairMasteryPerk.SetActive(true);
        PulseRepairMastery();
    }

    private void ShowSalvageMastery()
    {
        salvageMasteryPerk.SetActive(true);
        PulseSalvageMastery();
    }

    private void ShowZombieSlayer()
    {
        zombieSlayerPerk.SetActive(true);
        PulseZombieSlayer();
    }

    private void PulseInfectionPause()
    {
        infectionPausePulse.Pulse();
    }

    private void PulseInstantSalvage()
    {
        instantSalvagePulse.Pulse();
    }

    private void PulseLuckyBastard()
    {
        luckyBastardPulse.Pulse();
    }

    private void PulseRepairMastery()
    {
        repairMasteryPulse.Pulse(); 
    }

    private void PulseSalvageMastery()
    {
        salvageMasteryPulse.Pulse();
    }

    private void PulseZombieSlayer()
    {
        zombieSlayerPulse.Pulse();
    }
}
