using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TeamType
{
    BlueTeam,
    RedTeam
}

[System.Serializable]
public class TeamUI
{
    public TeamType teamType;
    public Image healthBar;
    public TextMeshProUGUI healthText;
}

public class BoxingUI : MonoBehaviour
{
    [Header("BlueTeam References")]
    public TeamUI blueTeamUI;

    [Header("RedTeam References")]
    public TeamUI redTeamUI;

    private void OnEnable()
    {
        BoxingManager.onAnyFighterApplyHealth += UpdateHealthUI;
        BoxingManager.onAnyFighterTakeDamage += UpdateHealthUI;
        BoxingManager.onLevelStarted += UpdateHealthUI;
    }

    private void OnDisable()
    {
        BoxingManager.onAnyFighterApplyHealth -= UpdateHealthUI;
        BoxingManager.onAnyFighterTakeDamage -= UpdateHealthUI;
        BoxingManager.onLevelStarted -= UpdateHealthUI;
    }

    public void UpdateHealthUI()
    {
        blueTeamUI.healthBar.fillAmount = BoxingManager.Instance.ActiveTeam[FighterType.Player].GetPercentHealth();
        blueTeamUI.healthText.text = (int)(BoxingManager.Instance.ActiveTeam[FighterType.Player].GetPercentHealth() * 100) + "%";

        redTeamUI.healthBar.fillAmount = BoxingManager.Instance.ActiveTeam[FighterType.Enemy].GetPercentHealth();
        redTeamUI.healthText.text = (int)(BoxingManager.Instance.ActiveTeam[FighterType.Enemy].GetPercentHealth() * 100) + "%";
    }
}
