using UnityEngine;
using System;
using System.Collections;

public class BenchConstruction : Bench, IInteractable
{
    [SerializeField] private int sparePartsConstructionCost;
    [SerializeField] private int maxConstructionLevel;
    [SerializeField] private float constructinInterval;

    private int currentConstructionLevel;

    private Coroutine constructionCoroutine;

    public static Action OnConstructionStart;
    public static Action OnConstructionTick;
    public static Action OnConstructionStop;
    public static Action OnConstructionFinished;

    private void TryConstruction()
    {
        if (ResourceController.Instance.CanRepair(sparePartsConstructionCost))
        {
            constructionCoroutine = StartCoroutine(DoConstruction());
        }
    }

    private IEnumerator DoConstruction()
    {
        while (currentConstructionLevel < maxConstructionLevel)
        {
            yield return new WaitForSeconds(constructinInterval);            
            ConstructionTick();

            if (currentConstructionLevel == maxConstructionLevel)
            {
                EndInteractionSecondary();
                OnConstructionFinished?.Invoke();                
            }

            if (ResourceController.Instance.GetSparePartsAmount() < sparePartsConstructionCost)
            {
                EndInteractionSecondary();
            }
        }        
    }

    private void ConstructionTick()
    {
        ResourceController.Instance.ChangeSparePartsAmount(-sparePartsConstructionCost);
        currentConstructionLevel += 1;
        OnConstructionTick?.Invoke();
    }

    public override void StartInteractionSecondary()
    {
        TryConstruction();
    }

    public override void EndInteractionSecondary()
    {
        if (constructionCoroutine != null)
        {
            StopCoroutine(constructionCoroutine);
            OnConstructionStop?.Invoke();
            constructionCoroutine = null;
        }
    }
}
