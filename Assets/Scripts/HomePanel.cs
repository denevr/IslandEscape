using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HomePanel : UIPanel
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StickmanFlowController stickmanFlowController;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Button _undoButton;
    [SerializeField] private ActionRecorder _actionRecorder;

    void OnEnable()
    {
        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        _levelText.text = "Level " + (levelIndex + 1);
    }

    public void OnUndoLastMoveButtonTapped()
    {
        //stickmanFlowController.UndoLastMove();
        _actionRecorder.Rewind();
    }

    public void OnRestartLevelButtonTapped()
    {
        levelManager.RestartLevel();
    }

    public void OnPassLevelButtonTapped()
    {
        UIManager.ShowLevelEndPanel();
    }

    public void EnableUndoButton(bool flag)
    {
        _undoButton.interactable = flag;
    }
}
