using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : GenericSingleton<UIManager>
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject completedUI;
    [SerializeField] private GameObject failedUI;

    private void OnEnable()
    {
        BoxingManager.onLevelCompleted += Show_CompleteUI;
        BoxingManager.onLevelFailed += Show_FailedUI;
        DataManager.OnLevelChanged += UpdateLevelText;
        UpdateLevelText(DataManager.Instance.LevelGame); 
    }

    private void OnDisable()
    {
        BoxingManager.onLevelCompleted -= Show_CompleteUI;
        BoxingManager.onLevelFailed -= Show_FailedUI;
    }

    public void Show_InGameUI()
    {
        inGameUI.SetActive(true);
        completedUI.SetActive(false);
        failedUI.SetActive(false);
    }

    public void Show_CompleteUI()
    {
        StartCoroutine(C_Show_CompleteUI());
    }

    private IEnumerator C_Show_CompleteUI()
    {

        yield return new WaitForSeconds(1.5f);
        inGameUI.SetActive(false);
        completedUI.SetActive(true);
        failedUI.SetActive(false);
    }

    public void Show_FailedUI()
    {
        StartCoroutine(C_Show_FailedUI());
    }
    private IEnumerator C_Show_FailedUI()
    {
        yield return new WaitForSeconds(1.5f);
        inGameUI.SetActive(false);
        completedUI.SetActive(false);
        failedUI.SetActive(true);
    }
    public void OnClick_Next()
    {
        Show_InGameUI();
        BoxingManager.Instance.GenerateLevel();
    }

    private void UpdateLevelText(int level)
    {
        if (levelText != null)
            levelText.text = "Level " + (level + 1);
    }
}
