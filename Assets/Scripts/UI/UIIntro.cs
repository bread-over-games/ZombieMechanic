using UnityEngine;
using System;

public class UIIntro : MonoBehaviour
{
    [SerializeField] private GameObject introUI;

    [SerializeField] private GameObject journalEntry1;
    [SerializeField] private GameObject journalEntry2;
    [SerializeField] private GameObject journalEntry3;

    [SerializeField] private GameObject continueHint;

    public static Action OnIntroStarted;
    public static Action OnIntroSkipped;

    private void Start()
    {
        PlayerInteraction.OnPrimaryInteractionInterceptor = SkipIntro;
        introUI.SetActive(true);
        OnIntroStarted?.Invoke();
        Invoke("DisplayJournalEntry1", 0.1f);
        Invoke("DisplayJournalEntry2", 8f);
        Invoke("DisplayJournalEntry3", 15f);
        Invoke("DisplayContinueHint", 22f);
    }

    private void DisplayJournalEntry1()
    {
        journalEntry1.SetActive(true);
    }

    private void DisplayJournalEntry2()
    {
        journalEntry2.SetActive(true);
    }

    private void DisplayJournalEntry3()
    {
        journalEntry3.SetActive(true);
    }

    private void DisplayContinueHint()
    {
        continueHint.SetActive(true);   
    }

    private void SkipIntro()
    {
        OnIntroSkipped?.Invoke();
        introUI.SetActive(false);        
    }
}
