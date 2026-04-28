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
    private RectTransform targetPosition;
    private int amount; // doesn't matter if it's xp, zombies, atb, etc.
    private float lingerDuration;
    public TMP_Text displayAmount;
    public Image displayIcon;

    [Header("Flyout Icons")]
    public Sprite zombieIcon;
    public Sprite antibioticsIcon;
    public Sprite sparePartsIcon;
    public Sprite xpIcon;

    public static Action<int, FlyoutTypes> OnFlyoutReachedDestination;

    public void Initialize(FlyoutTypes myType, int passedAmount, float givenLingerDuration, RectTransform target)
    {
        flyoutType = myType;
        amount = passedAmount;
        lingerDuration = givenLingerDuration;
        targetPosition = target;

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

    IEnumerator FlyToTarget(RectTransform target)
    {
        yield return new WaitForSeconds(lingerDuration);

        Vector3 mid = Vector3.Lerp(transform.position, target.position, 0.5f) + new Vector3(0f, -50f, 0f);

        Vector3[] path = { mid, target.position };

        transform.DOPath(path, 1f, PathType.CatmullRom)
            .SetEase(Ease.InQuad)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                OnFlyoutReachedDestination(amount, flyoutType);
                Destroy(gameObject);
            });
    }
}
