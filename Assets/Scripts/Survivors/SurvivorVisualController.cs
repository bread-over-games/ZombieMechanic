using DG.Tweening;
using UnityEngine;

public class SurvivorVisualController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Armory assignedArmory;

    [SerializeField] private Renderer survivorRenderer;

    private void OnEnable()
    {
        MissionController.OnMissionStarted += StandUp;
        MissionController.OnMissionCompleted += SitDown;
    }

    private void Awake()
    {
        foreach (Material mat in survivorRenderer.materials)
        {
            Color c = mat.GetColor("_BaseColor");
            DOTween.To(() => c.a, a => { c.a = a; mat.SetColor("_BaseColor", c); }, 0f, 0.01f)
                   .SetLink(gameObject);
        }
        Invoke("FadeIn", 0.1f);        
    }

    private void StandUp(Mission mission)
    {
        if (mission.armoryOwner != assignedArmory) return;
        animator.SetTrigger("Stand");
        FadeOut();
    }

    private void SitDown(Mission mission)
    {
        if (mission.armoryOwner != assignedArmory) return;
        animator.SetTrigger("Sit");
        FadeIn();
    }

    private void FadeOut()
    {
        foreach (Material mat in survivorRenderer.materials)
        {
            Color c = mat.GetColor("_BaseColor");
            DOTween.To(() => c.a, a => { c.a = a; mat.SetColor("_BaseColor", c); }, 0f, 1.5f)
                   .SetLink(gameObject);
        }
    }

    private void FadeIn()
    {
        foreach (Material mat in survivorRenderer.materials)
        {
            Color c = mat.GetColor("_BaseColor");
            DOTween.To(() => c.a, a => { c.a = a; mat.SetColor("_BaseColor", c); }, 1f, 1.5f)
                   .SetLink(gameObject);
        }
    }
}
