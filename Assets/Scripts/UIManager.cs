using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [Header("UI Panels")]
    [SerializeField] private HomePanel _homePanel;
    [SerializeField] private LevelEndPanel _levelEndPanel;

    void Start()
    {
        
    }

    public void Initialize()
    {
        _homePanel.Show();
    }

    public void ShowHomePanel()
    {
        _homePanel.Show();
    }

    public void ShowLevelEndPanel()
    {
        _homePanel.Hide();
        _levelEndPanel.Show();
    }

    public void SetInteractabilityUndoButton(bool flag)
    {
        _homePanel.EnableUndoButton(flag);
    }
}
