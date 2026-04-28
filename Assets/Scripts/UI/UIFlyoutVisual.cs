using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;

public class UIFlyoutVisual : MonoBehaviour
{
    public enum FlyoutTypes 
    { 
        Zombies,
        Antibiotics,
        SpareParts,
        XP
    }

    private FlyoutTypes flyoutType;
    private Vector2 targetPosition;
    private int amount; // doesn't matter if it's xp, zombies, atb, etc.
    private float lingerDuration;
    private float flyDuration;
    public TMP_Text displayAmount;
    public Image displayIcon;

    [Header("Flyout Icons")]
    public Sprite zombieIcon;
    public Sprite antibioticsIcon;
    public Sprite sparePartsIcon;
    public Sprite xpIcon;

    public static Action<int, FlyoutTypes> OnFlyoutReachedDestination;

    public void Initialize(FlyoutTypes myType, int passedAmount, float givenLingerDuration, float givenFlyDuration, Vector2 target)
    {
        flyoutType = myType;
        amount = passedAmount;
        lingerDuration = givenLingerDuration;
        targetPosition = target;
        flyDuration = givenFlyDuration; 

        switch (myType)
        {
            case FlyoutTypes.Zombies:
                displayIcon.sprite = zombieIcon;
                break;
            case FlyoutTypes.Antibiotics:
                displayIcon.sprite = antibioticsIcon;
                break;
            case FlyoutTypes.SpareParts:
                displayIcon.sprite = sparePartsIcon;
                break;
            case FlyoutTypes.XP:
                displayIcon.sprite = xpIcon;
                break;
        }

        displayAmount.text = amount.ToString();
        StartCoroutine(FlyToTarget(targetPosition));        
    }

    IEnumerator FlyToTarget(Vector2 target)
    {
        yield return new WaitForSeconds(lingerDuration);

        Vector3 mid = Vector3.Lerp(transform.position, target, 0.5f) + new Vector3(0f, -50f, 0f);

        Vector3[] path = { mid, target };

        transform.DOPath(path, flyDuration, PathType.CatmullRom)
            .SetEase(Ease.InQuad)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                OnFlyoutReachedDestination(amount, flyoutType);
                Destroy(gameObject);
            });
    }
}
