using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StickmanFlowController : MonoBehaviour
{
    [SerializeField] private BridgeController _bridgeController;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager UIManager;

    private Vector3 offset = new Vector3(0, .25f, 0);
    private float _speed = 3f;

    public bool IsFlowAvailableBetween(Platform startPlatform, Platform endPlatform)
    {
        if (endPlatform.stickmans.Count == 16) return false;
        if (startPlatform.stickmans.Count == 0) return false;
        if (startPlatform.GetTransferableStickmans().Count > 16 - endPlatform.stickmans.Count) return false;

        if (startPlatform.GetLastStickmanColor() != endPlatform.GetLastStickmanColor())
        {
            if (endPlatform.GetLastStickmanColor() != Colors.None) return false;
        }

        return true;
    }

    public void StartFlowBetween(Platform startPlatform, Platform endPlatform)
    {
        var stickmans = startPlatform.GetTransferableStickmans();
        StartCoroutine(MoveStickmans(stickmans, startPlatform, endPlatform));
    }

    private IEnumerator MoveStickmans(List<Stickman> stickmans, Platform startPlatform, Platform endPlatform)
    {
        var startPos = startPlatform.GetConnectionPoint() + offset;
        var endPos = endPlatform.GetConnectionPoint() + offset;
        var stickmansToMove = stickmans.Count;
        int stickmansMoved = 0;

        for (int i = 0; i < stickmansToMove; i++)
        {
            var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
            var stickman = stickmans[i];
            stickman.transform.SetParent(placementPos);
            startPlatform.RemoveStickmanToPlatform(stickman);
            endPlatform.AddStickmanToPlatform(stickman);
            Vector3[] path = new[] { stickman.transform.position, startPos, endPos, placementPos.position };

            float duration = .15f;
            yield return new WaitForSeconds(duration);

            stickman.Run();
            var distance = Vector3.Distance(startPos, endPos);
            stickman.transform.DOPath(path, distance / _speed)
                .SetLookAt(.01f, -Vector3.forward)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    stickman.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
                    stickman.Idle();
                    stickmansMoved++;

                    if (stickmansMoved == stickmansToMove)
                    {
                        _bridgeController.RemoveBridgeBetween(startPlatform, endPlatform);
                        CheckLevelEnd();
                    }
                });
        }

        if (endPlatform.IsFullyLoadedWithStickmansOfSameColor())
        {
            endPlatform.Lock();
            //CheckLevelEnd();
        }

        //inputManager.isInputEnabled = true;
    }

    public void CheckLevelEnd()
    {
        var platforms = levelManager.GetPlatformsInLevel();

        for (int i = 0; i < platforms.Count; i++)
        {
            if (platforms[i].stickmans.Count == 0 ||
                platforms[i].IsFullyLoadedWithStickmansOfSameColor())
                continue;
            else
                return;
        }

        UIManager.ShowLevelEndPanel();
    }
}
