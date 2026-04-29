using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIXPCounter : MonoBehaviour
{
    public TMP_Text currentLevel;
    public Image xpBar;

    private XPCounter xpCounter;

    private void OnEnable()
    {
        UIFlyoutVisual.OnFlyoutReachedDestination += ChangeXPBar;
        XPCounter.OnLevelUp += ChangeCurrentLevel;
    }

    private void OnDisable()
    {
        UIFlyoutVisual.OnFlyoutReachedDestination -= ChangeXPBar;
        XPCounter.OnLevelUp -= ChangeCurrentLevel;
    }

    private void Start()
    {
        xpCounter = XPCounter.Instance;
        ChangeCurrentLevel();
        ChangeXPBar(UIFlyoutVisual.FlyoutTypes.XP);
    }

    private void ChangeXPBar(UIFlyoutVisual.FlyoutTypes type)
    {
        if (type != UIFlyoutVisual.FlyoutTypes.XP) return;

        xpBar.fillAmount = (float)xpCounter.currentXP / xpCounter.xpRequiredForNextLevel[xpCounter.currentLvl];
    }

    private void ChangeCurrentLevel()
    {
        currentLevel.text = xpCounter.currentLvl.ToString();
        ChangeXPBar(UIFlyoutVisual.FlyoutTypes.XP);
    }
}
