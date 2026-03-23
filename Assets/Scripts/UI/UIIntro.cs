using UnityEngine;
using System;

public class UIIntro : MonoBehaviour
{
    [SerializeField] private GameObject introUI;

    [SerializeField] private GameObject journalEntry1;
    [SerializeField] private GameObject journalEntry2;
    [SerializeField] private GameObject journalEntry3;

    [SerializeField] private GameObject continueHint;

    private void OnEnable()
    {
        PlayerInteraction.OnIntroSkip += SkipIntro;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnIntroSkip -= SkipIntro;
    }

    private void Awake()
    {
        introUI.SetActive(true);
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
        introUI.SetActive(false);        
    }
}
