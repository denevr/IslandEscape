using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomePanel : UIPanel
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StickmanFlowController stickmanFlowController;
    [SerializeField] private TextMeshProUGUI _levelText;

    void OnEnable()
    {
        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        _levelText.text = "Level " + (levelIndex + 1);
    }

    public void OnUndoLastMoveButtonTapped()
    {
        stickmanFlowController.UndoLastMove();
    }

    public void OnRestartLevelButtonTapped()
    {
        levelManager.RestartLevel();
    }

    public void OnPassLevelButtonTapped()
    {
        UIManager.ShowLevelEndPanel();
    }
}
