using DG.Tweening;
using UnityEngine;

public class ObjectEffects : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float scaleIncrease = 1.5f;

    private Vector3 originalScale;
    private Object myObject;

    private void Awake()
    {
        originalScale = new Vector3(1, 1, 1);
    }

    public void Shake()
    {
        transform.DOKill();
        transform.DOShakeRotation(duration: 0.08f, strength: new Vector3(0f, 8f, 0f));
    }

    public void Pulse()
    {
        transform.DOKill();
        transform.localScale = originalScale;

        transform.DOScale(originalScale * scaleIncrease, duration).OnComplete(() => transform.DOScale(originalScale, duration));
    }
}