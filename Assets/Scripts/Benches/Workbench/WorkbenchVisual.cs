using UnityEngine;
using DG.Tweening;

public class WorkbenchVisual : MonoBehaviour
{
    public ObjectDisplay workbenchObjDisplay;
    public ParticleSystem weldSparks;
    public ParticleSystem grinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light weldLight;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Bench workbench;
    private IInteractable interactableTable;

    private void OnEnable()
    {
        Workbench.OnRepairStart += StartWorkingEffect;
        Workbench.OnRepairStop += StopWorkingEffect;
        PlayerInteraction.OnInteractableApproached += OpenWorkbench;
        PlayerInteraction.OnInteractableLeft += CloseWorkbench;
        Workbench.OnRepairTick += RepairTickEffect;
    }

    private void OnDisable()
    {
        Workbench.OnRepairStart -= StartWorkingEffect;
        Workbench.OnRepairStop -= StopWorkingEffect;
        PlayerInteraction.OnInteractableApproached -= OpenWorkbench;
        Workbench.OnRepairTick -= RepairTickEffect;
    }

    private void Awake()
    {
        interactableTable = workbench as IInteractable;
    }
    private void RepairTickEffect()
    {
        if (workbenchObjDisplay.currentObjects[0] == null) return;
        workbenchObjDisplay.currentObjects[0].GetComponent<ObjectEffects>().Shake();
    }

    private void StartWorkingEffect()
    {
        weldSparks.Play();
        grinderSparks.Play();
        weldLight.enabled = true;
        FlickerNext();
        animator.SetBool("isWorking", true);
    }

    private void StopWorkingEffect()
    {
        weldSparks.Stop();
        grinderSparks.Stop();
        weldLight.enabled = false;
        StopFlicker();
        animator.SetBool("isWorking", false);
        workbenchObjDisplay.currentObjects[0].GetComponent<ObjectEffects>().Pulse();
    }

    private void FlickerNext()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minInterval, maxInterval);

        weldLight.DOIntensity(targetIntensity, duration).SetEase(Ease.InOutSine).OnComplete(FlickerNext);
    }

    private void StopFlicker()
    {
        weldLight.DOKill();
    }

    void OnDestroy()
    {
        weldLight.DOKill();
    }

    private void OpenWorkbench(IInteractable approachedWorkbench)
    {
        if (approachedWorkbench == interactableTable)
        {
            animator.SetTrigger("Open");
        }
    }

    private void CloseWorkbench(IInteractable leftWorkbench)
    {
        if (leftWorkbench == interactableTable)
        {
            animator.SetTrigger("Close");
        }
    }
}
