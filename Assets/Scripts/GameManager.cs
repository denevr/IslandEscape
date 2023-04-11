using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        Application.targetFrameRate = 60;

        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        levelManager.StartLevel(levelIndex);
    }
}
