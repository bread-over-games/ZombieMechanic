using DG.Tweening;
using UnityEngine;

public class BenchConstruictibleVisual : MonoBehaviour
{
    public ParticleSystem angleGrinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light weldLight;

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
        BenchConstruction.OnConstructionStart += StartWeldSparks;
        BenchConstruction.OnConstructionStop += StopWeldSparks;
        BenchConstruction.OnConstructionTick += ConstructTickEffect;
    }

    private void OnDisable()
    {
        BenchConstruction.OnConstructionStart -= StartWeldSparks;
        BenchConstruction.OnConstructionStop -= StopWeldSparks;
        BenchConstruction.OnConstructionTick -= ConstructTickEffect;
    }

    private void Awake()
    {        
        canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        flyoutsParent = GameObject.Find("FlyoutsParent").GetComponent<Transform>();
        currentLevelTarget = GameObject.Find("CurrentLevel").GetComponent<RectTransform>();
        sparePartsSpawnPosition = GameObject.Find("SparePartsIcon").GetComponent<RectTransform>();
    }

    private void StartWeldSparks(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Play();
        weldLight.enabled = true;
        FlickerNext();
    }

    private void StopWeldSparks(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Stop();
        weldLight.enabled = false;
        StopFlicker();
    }

    private void ConstructTickEffect(GameObject obj)
    {
        if (obj != gameObject) return;

        // xp flyout initialization        
        UIFlyoutVisual xpFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        xpFlyout.GetComponent<RectTransform>().anchoredPosition = ConvertWorldToScreenPos(xpFlyoutAnchor);
        xpFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.XP, 1, 0.05f, 0.5f, currentLevelTarget.position);

        // spare parts initialization                
        UIFlyoutVisual sparePartsFlyout = Instantiate(flyoutPrefab, flyoutsParent);
        sparePartsFlyout.GetComponent<RectTransform>().position = sparePartsSpawnPosition.position;
        sparePartsFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.SpareParts, 1, 0.01f, 0.25f, Camera.main.WorldToScreenPoint(sparePartsFlyinAnchor.position));
    }

    private Vector2 ConvertWorldToScreenPos(Transform anchor)
    {
        Vector3 worldPos = anchor.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, canvas.worldCamera, out Vector2 localPoint);
        return localPoint;
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
}
