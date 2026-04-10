using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class EmissionFlasher : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] Color  m_EmissionColor     = Color.white;
    [SerializeField] float  m_PeakIntensity      = 3f;    // HDR multiplier at peak
    [SerializeField] float  m_BaseIntensity      = 0f;    // intensity at rest
    [SerializeField] float  m_FlashDuration      = 0.4f;  // seconds for one half-cycle

    static readonly int s_EmissiveColorID   = Shader.PropertyToID("_EmissiveColor");
    static readonly int s_EmissiveIntensity = Shader.PropertyToID("_EmissiveIntensity");

    Renderer  m_Renderer;
    Material  m_Material;
    Tween     m_Tween;
    float     m_CurrentIntensity;

    void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Material = m_Renderer.material;   
    }

    public void StartFlash()
    {
        if (m_Tween != null && m_Tween.IsActive()) return;

        m_Material.EnableKeyword("_EMISSION");

        m_CurrentIntensity = m_BaseIntensity;
        ApplyEmission(m_CurrentIntensity);

        m_Tween = DOTween
            .To(() => m_CurrentIntensity,
                x  => { m_CurrentIntensity = x; ApplyEmission(x); },
                m_PeakIntensity,
                m_FlashDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject);
    }

    public void StopFlash(bool resetEmission = true)
    {
        m_Tween?.Kill();
        m_Tween = null;

        if (resetEmission)
            ApplyEmission(m_BaseIntensity);
    }

    public bool IsFlashing => m_Tween != null && m_Tween.IsActive();

    void ApplyEmission(float intensity)
    {
        m_Material.SetColor(s_EmissiveColorID, m_EmissionColor * intensity);
    }

    void OnDestroy()
    {
        m_Tween?.Kill();
    }
}
