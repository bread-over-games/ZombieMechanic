using DG.Tweening;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] float targetX = 5f;
    [SerializeField] float panAngle = 60f;
    [SerializeField] float duration = 3f;
    [SerializeField] Ease ease = Ease.InOutSine;

    [ContextMenu("Play Pan")]
    public void PlayPan()
    {
        transform.DOLocalMoveX(targetX, duration).SetEase(ease);
        transform.DOLocalRotate(new Vector3(0f, panAngle, 0f), duration, RotateMode.LocalAxisAdd).SetEase(ease);
    }
}