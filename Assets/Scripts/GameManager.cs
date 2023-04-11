using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InputManager inputManager;

    void Start()
    {
        Application.targetFrameRate = 60;

        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        levelManager.StartLevel(levelIndex);
        inputManager.isInputEnabled = true;
    }
}
