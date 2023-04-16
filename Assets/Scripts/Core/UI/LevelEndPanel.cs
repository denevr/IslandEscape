using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPanel : UIPanel
{
    [SerializeField] private LevelManager levelManager;

    public void OnNextLevelButtonTapped()
    {
        levelManager.PlayNextLevel();
        Hide();
    }
}
