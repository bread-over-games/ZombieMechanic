using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class UIHealthBar : MonoBehaviour
{
    public Image infectionLevel;
    public GameObject healthBarUI;

    [SerializeField] private float shakeStrength = 10f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private int shakeVibrato = 1;
    [SerializeField] private float shakeRandomness = 1;
    [SerializeField] private RectTransform rectTransform;
    private Tween shakeTween;
    private bool isShaking = false;

    private void OnEnable()
    {
        Infection.OnInfectionLevelChange += UpdateInfectionLevel;
        MissionController.OnMissionCompleted += DisplayInfectionBar;
    }

    private void OnDisable()
    {
        Infection.OnInfectionLevelChange -= UpdateInfectionLevel;
        MissionController.OnMissionCompleted -= DisplayInfectionBar;
    }

    private void Awake()
    {
        healthBarUI.SetActive(false);   
    }

    private void DisplayInfectionBar(Mission mission)
    {
        healthBarUI.SetActive(true);
    }

    private void UpdateInfectionLevel(float currentInfectionLevel)
    {
        infectionLevel.fillAmount = currentInfectionLevel / 100;

        if (currentInfectionLevel >= 70 && !isShaking)
        {
            StartShaking();
        }

        if (currentInfectionLevel < 70 && isShaking)
        {
            StopShaking();
        }
    }

    private void StartShaking()
    {
        isShaking = true;
        shakeTween = rectTransform.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness).SetLoops(-1, LoopType.Restart);
    }

    private void StopShaking()
    {
        isShaking = false;
        shakeTween?.Kill(complete: true); // rewinds to origin
        shakeTween = null;
    }
}
