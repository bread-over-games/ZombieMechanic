using DG.Tweening;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] float targetX = 5f;
    [SerializeField] float duration = 3f;

    [ContextMenu("Play Pan")]
    public void PlayPan()
    {
        transform.DOLocalMoveX(targetX, duration);
    }
}