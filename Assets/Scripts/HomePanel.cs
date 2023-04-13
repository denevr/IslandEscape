using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomePanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI _levelText;

    void OnEnable()
    {
        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        _levelText.text = "Level " + (levelIndex + 1);
    }

    void Start()
    {

    }

}
