using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextFlasher : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float duration = 0.3f;

    private Tween tween;

    public void Flash()
    {
        tween?.Kill();
        text.color = Color.white;

        tween = text.DOColor(Color.red, duration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => text.color = Color.white);
    }

    private void OnDestroy()
    {
        tween?.Kill();
    }
}
