using UnityEngine;
using System;
using System.Collections;

public class BenchConstruction : MonoBehaviour, IConstructible
{
    [SerializeField] private int sparePartsConstructionCost;
    [SerializeField] private int maxConstructionLevel;
    [SerializeField] private float constructinInterval;
    [SerializeField] public string constructibleName;
    [TextArea(3,6)]
    [SerializeField] public string constructibleDescription;

    private int currentConstructionLevel;

    private Coroutine constructionCoroutine;

    public static Action<GameObject> OnConstructionStart;
    public static Action OnConstructionTick;
    public static Action<GameObject> OnConstructionStop;
    public static Action<GameObject> OnConstructionFinished;

    public string GetName()
    { 
        return constructibleName; 
    }

    public string GetDescription()
    {
        return constructibleDescription; 
    }

    public int GetSparePartsRequired()
    {
        return (maxConstructionLevel - currentConstructionLevel) * sparePartsConstructionCost;
    }

    public float GetCurrentConstructionLevel()
    {        
        return (float)currentConstructionLevel / maxConstructionLevel; 
    }  
    
    public int GetSparePartsTickCost()
    {
        return sparePartsConstructionCost;
    }

    private void TryConstruction()
    {
        if (ResourceController.Instance.CanRepair(sparePartsConstructionCost))
        {
            constructionCoroutine = StartCoroutine(DoConstruction());
            OnConstructionStart?.Invoke(gameObject);
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
                OnConstructionFinished?.Invoke(gameObject);                
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

    public void StartInteractionPrimary(){}

    public void EndInteractionPrimary(){}

    public virtual bool IsInteractionPossible()
    {
        return true;
    }

    public void StartInteractionSecondary()
    {
        TryConstruction();
    }

    public void EndInteractionSecondary()
    {
        if (constructionCoroutine != null)
        {
            StopCoroutine(constructionCoroutine);
            OnConstructionStop?.Invoke(gameObject);
            constructionCoroutine = null;
        }
    }
}
