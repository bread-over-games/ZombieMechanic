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

    [Header("Flyout")]
    [SerializeField] private UIFlyoutVisual flyoutPrefab;
    [SerializeField] private Transform xpFlyoutAnchor;
    [SerializeField] private Transform sparePartsFlyinAnchor;
    private Transform flyoutsParent;
    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform currentLevelTarget;
    private RectTransform sparePartsSpawnPosition;

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
        PlayerInteraction.OnInteractableLeft -= CloseWorkbench;
        Workbench.OnRepairTick -= RepairTickEffect;
    }

    private void Awake()
    {
        interactableTable = workbench as IInteractable;
        canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();   
        canvasRect = canvas.GetComponent<RectTransform>();
        flyoutsParent = GameObject.Find("FlyoutsParent").GetComponent<Transform>(); 
        currentLevelTarget = GameObject.Find("CurrentLevel").GetComponent<RectTransform>();
        sparePartsSpawnPosition = GameObject.Find("SparePartsIcon").GetComponent<RectTransform>();
    }

    private void RepairTickEffect(Bench sourceBench)
    {
        if (workbench != sourceBench) return;
        if (workbenchObjDisplay.currentObjects[0] == null) return;
        workbenchObjDisplay.currentObjects[0].GetComponent<ObjectEffects>().Shake();

        // xp flyout initialization        
        Vector2 xpFlyoutPosition = ConvertWorldToScreenPos(xpFlyoutAnchor);
        UIFlyoutVisual xpFlyout = Instantiate(flyoutPrefab, flyoutsParent);

        xpFlyout.GetComponent<RectTransform>().anchoredPosition = xpFlyoutPosition;
        xpFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.XP, workbench.GetComponent<Workbench>().repairValue * XPCounter.Instance.repairXP, 0.05f, 0.5f, currentLevelTarget.position);        

        // spare parts initialization
                
        UIFlyoutVisual sparePartsFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        sparePartsFlyout.GetComponent<RectTransform>().position = sparePartsSpawnPosition.position;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(sparePartsFlyinAnchor.position);
        sparePartsFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.SpareParts, 1, 0.01f, 0.25f, targetScreenPos);
    } 
    
    private Vector2 ConvertWorldToScreenPos(Transform anchor)
    {
        Vector3 worldPos = anchor.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, canvas.worldCamera, out Vector2 localPoint);
        return localPoint;
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
