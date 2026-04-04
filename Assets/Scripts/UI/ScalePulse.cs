using DG.Tweening;
using UnityEngine;

public class ScalePulse : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void Pulse()
    {
        transform.DOKill();
        transform.localScale = _originalScale;

        transform.DOScale(_originalScale * 1.5f, duration).OnComplete(() => transform.DOScale(_originalScale, duration));
    }
}