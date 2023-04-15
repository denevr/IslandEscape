using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> levelDatas;

    [SerializeField] private UIManager UIManager;
    [SerializeField] private ColorPalette _colorPalette;
    [SerializeField] private GameObject _water;
    [SerializeField] private Platform _platformPrefab;
    [SerializeField] private GameObject _stickmanPrefab;
    [SerializeField] private Transform _platformParent;
    [SerializeField] private StickmanFlowController _stickmanFlowController;

    private List<Platform> _platformsInLevel;
    private const int stickmanCountInLine = 4;

    private void OnEnable()
    {
        _platformsInLevel = new List<Platform>();
    }

    void Start()
    {
        _water.SetActive(true);
    }

    public void StartLevel(int levelIndex)
    {
        LevelData levelData = levelDatas.Find((x) => x.Id == levelIndex);
        UIManager.ShowHomePanel();
        _platformsInLevel.Clear();
        _stickmanFlowController.ResetActionData();

        var platformCount = levelData.PlatformDatas.Length;

        for (int i = 0; i < platformCount; i++)
            GeneratePlatform(levelData.PlatformDatas[i]);
    }

    public void GeneratePlatform(PlatformData platformData)
    {
        var platform = LeanPool.Spawn(_platformPrefab, _platformParent);
        platform.transform.position = platformData.Position;
        platform.transform.rotation = platformData.Rotation;
        _platformsInLevel.Add(platform);

        var platformColorCount = platformData.Colors.Length;
        var index = 0;

        for (int i = 0; i < platformColorCount; i++)
        {
            var count = stickmanCountInLine;
            var color = platformData.Colors[i];

            while (count > 0)
            {
                var go = LeanPool.Spawn(_stickmanPrefab, platform.stickmanPositions[index]);
                go.transform.position = platform.stickmanPositions[index].position;
                go.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));

                var stickman = go.GetComponent<Stickman>();
                var mat = _colorPalette.GetMaterialFromColor(color);
                stickman.SetColorMaterial(mat);
                stickman.SetColor(color);
                //Debug.LogError((stickmanCountInLine - count) * i);
                //platform.stickmans[(stickmanCountInLine - count) * i] = stickman;
                platform.stickmans.Add(stickman);

                index++;
                count--;
            }
        }
    }

    public List<Platform> GetPlatformsInLevel()
    {
        return _platformsInLevel;
    }

    public void PlayNextLevel()
    {
        //clean scene
        //for (int i = 0; i < _platformsInLevel.Count; i++)
        //{
        //    //LeanPool.Despawn(_platformsInLevel[i].stickmans);
        //    _platformsInLevel[i].ClearStickmans();
        //}
        LeanPool.DespawnAll();

        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        levelIndex++;
        PlayerPrefs.SetInt(PlayerPrefsConstants.playerLevel, levelIndex);
        StartLevel(levelIndex);
    }

    public void RestartLevel()
    {
        LeanPool.DespawnAll();

        int levelIndex = PlayerPrefs.GetInt(PlayerPrefsConstants.playerLevel, 0);
        StartLevel(levelIndex);
    }
}
