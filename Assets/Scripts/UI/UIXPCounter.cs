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
        XPCounter.OnXPChange += ChangeXPBar;
        XPCounter.OnLevelUp += ChangeCurrentLevel;
    }

    private void OnDisable()
    {
        XPCounter.OnXPChange -= ChangeXPBar;
        XPCounter.OnLevelUp -= ChangeCurrentLevel;
    }

    private void Start()
    {
        xpCounter = XPCounter.Instance;
        ChangeCurrentLevel();
        ChangeXPBar();
    }

    private void ChangeXPBar()
    {
        xpBar.fillAmount = (float)xpCounter.currentXP / xpCounter.xpRequiredForNextLevel[xpCounter.currentLvl];
    }

    private void ChangeCurrentLevel()
    {
        currentLevel.text = xpCounter.currentLvl.ToString();
    }
}
