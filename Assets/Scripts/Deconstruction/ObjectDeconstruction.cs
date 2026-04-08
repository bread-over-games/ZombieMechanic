using UnityEngine;
using System;
using System.Collections;

public class ObjectDeconstruction : MonoBehaviour, IDeconstructible
{
    [TextArea(3,6)]
    [SerializeField] private string deconstructibleDescription;
    [SerializeField] private string deconstructibleName;
    [SerializeField] private int sparePartsReward;
    [SerializeField] private int totalDurability;
    private int currentDurability;
    [SerializeField] private float deconstructionInterval;

    private Coroutine deconstructionCoroutine;

    public static Action<GameObject> OnDeconstructionStart;
    public static Action OnDeconstructionTick;
    public static Action<GameObject> OnDeconstructionStop;
    public static Action<GameObject> OnDeconstructionFinished;

    private void Awake()
    {
        currentDurability = totalDurability;
    }

    public int GetSparePartsReward()
    {
        return sparePartsReward;
    }

    public float GetCurrentDurabilityLevel()
    {
        return (float)currentDurability / totalDurability;
    }

    public int GetSparePartsLeft()
    {
        return currentDurability * sparePartsReward;
    }

    public string GetName()
    {
        return deconstructibleName;
    }

    public string GetDescription()
    {
        return deconstructibleDescription;
    }

    public void StartInteractionPrimary() { }

    public void EndInteractionPrimary() { }

    private void TryDeconstruction()
    {
        deconstructionCoroutine = StartCoroutine(DoDecounstruction());
        OnDeconstructionStart?.Invoke(gameObject);
    }

    private IEnumerator DoDecounstruction()
    {
        while (currentDurability > 0)
        {
            yield return new WaitForSeconds(deconstructionInterval);

            if (ResourceController.Instance.CheckSparePartsLimit(sparePartsReward))
            {
                ConstructionTick();     
                
                if (currentDurability <= 0)
                {
                    EndInteractionSecondary();
                    OnDeconstructionFinished?.Invoke(gameObject);
                }
            }
        }    
    }

    private void ConstructionTick()
    {
        ResourceController.Instance.ChangeSparePartsAmount(sparePartsReward); // reward
        currentDurability -= 1;
        OnDeconstructionTick?.Invoke();
    }

    public void StartInteractionSecondary()
    {        
        TryDeconstruction();
    }

    public void EndInteractionSecondary()
    {
        if (deconstructionCoroutine != null)
        {
            StopCoroutine(deconstructionCoroutine);
            OnDeconstructionStop?.Invoke(gameObject);
            deconstructionCoroutine = null;
        }
    }

    public bool IsInteractionPossible()
    {
        return true;
    }
}
