using DG.Tweening;
using UnityEngine;

public class SalvageTableVisual : MonoBehaviour
{
    public ObjectDisplay salvageTableObjDisplay;
    public ParticleSystem angleGrinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light grindLight;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Bench salvageTable;
    private IInteractable interactableTable;

    [Header("Flyout")]
    [SerializeField] private UIFlyoutVisual flyoutPrefab;
    [SerializeField] private Transform xpFlyoutAnchor;
    [SerializeField] private Transform sparePartsFlyoutAnchor;
    private Transform flyoutsParent;
    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform currentLevelTarget;
    private RectTransform sparePartsTarget;

    private void OnEnable()
    {
        SalvageTable.OnSalvageStart += _ => StartAngleGrinderSparks();
        SalvageTable.OnSalvageStop += _ => StopAngleGrinderSparks();
        SalvageTable.OnSalvageTick += _ => SalvageTickEffect();
    }

    private void OnDisable()
    {
        SalvageTable.OnSalvageStart -= _ => StartAngleGrinderSparks();
        SalvageTable.OnSalvageStop -= _ => StopAngleGrinderSparks();
        SalvageTable.OnSalvageTick -= _ => SalvageTickEffect();
    }

    private void Awake()
    {
        interactableTable = salvageTable as IInteractable;
        canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        flyoutsParent = GameObject.Find("FlyoutsParent").GetComponent<Transform>();
        currentLevelTarget = GameObject.Find("CurrentLevel").GetComponent<RectTransform>();
        sparePartsTarget = GameObject.Find("SparePartsIcon").GetComponent<RectTransform>();
    }

    private void SalvageTickEffect()
    {
        salvageTableObjDisplay.currentObjects[0].GetComponent<ObjectEffects>().Shake();
        
        // xp flyout initialization        
        UIFlyoutVisual xpFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        xpFlyout.GetComponent<RectTransform>().anchoredPosition = ConvertWorldToScreenPos(xpFlyoutAnchor);
        xpFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.XP, salvageTable.GetComponent<SalvageTable>().salvagingValue * XPCounter.Instance.salvageXP, 0.04f, 0.5f, currentLevelTarget.position);

        // spare parts initialization
        UIFlyoutVisual sparePartsFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        sparePartsFlyout.GetComponent<RectTransform>().anchoredPosition = ConvertWorldToScreenPos(sparePartsFlyoutAnchor);
        sparePartsFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.SpareParts, -salvageTable.GetComponent<SalvageTable>().salvagingValue, 0.06f, 0.5f, sparePartsTarget.position);
    }

    private Vector2 ConvertWorldToScreenPos(Transform anchor)
    {
        Vector3 worldPos = anchor.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, canvas.worldCamera, out Vector2 localPoint);
        return localPoint;
    }

    private void StartAngleGrinderSparks()
    {
        angleGrinderSparks.Play();
        grindLight.enabled = true;
        FlickerNext();
    }

    private void StopAngleGrinderSparks()
    {
        angleGrinderSparks.Stop();
        grindLight.enabled = false;
        StopFlicker();
    }

    private void FlickerNext()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minInterval, maxInterval);

        grindLight.DOIntensity(targetIntensity, duration).SetEase(Ease.InOutSine).OnComplete(FlickerNext);
    }

    private void StopFlicker()
    {
        grindLight.DOKill();
    }

    void OnDestroy()
    {
        grindLight.DOKill();
    }
}
