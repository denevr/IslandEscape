using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> levelDatas;

    [SerializeField] private ColorPalette _colorPalette;
    [SerializeField] private GameObject _water;
    [SerializeField] private Platform _platformPrefab;
    [SerializeField] private GameObject _stickmanPrefab;

    [SerializeField] private Transform _platformParent;

    private const int stickmanCountInLine = 4;

    void Start()
    {
        _water.SetActive(true);
    }

    public void StartLevel(int levelIndex)
    {
        LevelData levelData = levelDatas.Find((x) => x.Id == levelIndex);

        var platformCount = levelData.PlatformDatas.Length;

        for (int i = 0; i < platformCount; i++)
            GeneratePlatform(levelData.PlatformDatas[i]);
    }

    public void GeneratePlatform(PlatformData platformData)
    {
        var platform = LeanPool.Spawn(_platformPrefab, _platformParent);
        platform.transform.position = platformData.Position;
        platform.transform.rotation = platformData.Rotation;

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

                var stickman = go.GetComponent<Stickman>();
                var mat = _colorPalette.GetMaterialFromColor(color);
                stickman.SetColorMaterial(mat);
                stickman.SetColor(color);
                //Debug.LogError((stickmanCountInLine - count) * i);
                //platform.stickmans[(stickmanCountInLine - count) * i] = stickman;
                platform.stickmans.Add(stickman);
                //platform.stickmanStack.Push(stickman);

                index++;
                count--;
            }
        }
    }
}
