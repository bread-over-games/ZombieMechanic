using DG.Tweening;
using UnityEngine;

public class DeconstructibleVisual : MonoBehaviour
{    
    public ParticleSystem angleGrinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light grinderLight;

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
        ObjectDeconstruction.OnDeconstructionStart += StartVisual;
        ObjectDeconstruction.OnDeconstructionStop += StopVisual;
        ObjectDeconstruction.OnDeconstructionTick += DeconstructionTickEffect;
    }

    private void OnDisable()
    {
        ObjectDeconstruction.OnDeconstructionStart -= StartVisual;
        ObjectDeconstruction.OnDeconstructionStop -= StopVisual;
        ObjectDeconstruction.OnDeconstructionTick -= DeconstructionTickEffect;
    }

    private void Awake()
    {
        canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        flyoutsParent = GameObject.Find("FlyoutsParent").GetComponent<Transform>();
        currentLevelTarget = GameObject.Find("CurrentLevel").GetComponent<RectTransform>();
        sparePartsTarget = GameObject.Find("SparePartsIcon").GetComponent<RectTransform>();
    }

    private void DeconstructionTickEffect(GameObject obj)
    {
        if (obj != gameObject) return;

        // xp flyout initialization        
        UIFlyoutVisual xpFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        xpFlyout.GetComponent<RectTransform>().anchoredPosition = ConvertWorldToScreenPos(xpFlyoutAnchor);
        xpFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.XP, 1 * XPCounter.Instance.salvageXP, 0.04f, 0.5f, currentLevelTarget.position);

        // spare parts initialization
        UIFlyoutVisual sparePartsFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        sparePartsFlyout.GetComponent<RectTransform>().anchoredPosition = ConvertWorldToScreenPos(sparePartsFlyoutAnchor);
        sparePartsFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.SpareParts, 1, 0.06f, 0.5f, sparePartsTarget.position);
    }

    private Vector2 ConvertWorldToScreenPos(Transform anchor)
    {
        Vector3 worldPos = anchor.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, canvas.worldCamera, out Vector2 localPoint);
        return localPoint;
    }

    private void StartVisual(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Play();
        grinderLight.enabled = true;
        FlickerNext();
    }

    private void StopVisual(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Stop();
        grinderLight.enabled = false;
        StopFlicker();
    }

    private void FlickerNext()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minInterval, maxInterval);

        grinderLight.DOIntensity(targetIntensity, duration).SetEase(Ease.InOutSine).OnComplete(FlickerNext);
    }

    private void StopFlicker()
    {
        grinderLight.DOKill();
    }

    void OnDestroy()
    {
        grinderLight.DOKill();
    }
}
