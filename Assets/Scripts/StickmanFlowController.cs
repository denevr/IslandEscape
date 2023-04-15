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

    private Stack<Stickman> _movingStickmans = new Stack<Stickman>(); //reset these on level start
    private Stack<Platform> _platformsConnected = new Stack<Platform>();
    private Stack<int> _stickmansRelocatedInEveryMove = new Stack<int>();
    private Coroutine _coroutine;
    private Vector3 offset = new Vector3(0, .25f, 0);
    private int _movedStickmanCountOnTheLastMove; //
    private readonly float _speed = 3f;

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

        var stickmansToMove = stickmans.Count;
        //_movedStickmanCountOnTheLastMove = stickmansToMove; //no need for a new variable?
        _stickmansRelocatedInEveryMove.Push(stickmansToMove);
        _platformsConnected.Push(startPlatform);
        _platformsConnected.Push(endPlatform);

        for (int i = 0; i < stickmansToMove; i++)
        {
            var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
            var stickman = stickmans[i];
            _movingStickmans.Push(stickman);
            startPlatform.RemoveStickmanFromPlatform(stickman);
            endPlatform.AddStickmanToPlatform(stickman);
        }

        _coroutine = StartCoroutine(MoveStickmans(stickmans, startPlatform, endPlatform));
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
                        inputManager.isInputEnabled = true;
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

    public void UndoLastMove()
    {
        if (_stickmansRelocatedInEveryMove.Count == 0) 
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        var count = _stickmansRelocatedInEveryMove.Peek();
        Debug.LogError("_movingStickmans.Count: " + _movingStickmans.Count);
        //Debug.LogError("_platformsConnected.Count: " + _platformsConnected.Count);
        Debug.LogError("count: " + count);

        if (_movingStickmans.Count != 0 && 
            _movingStickmans.Count >= count &&
            _platformsConnected.Count >= 2)
        {
            Platform toPlatform = _platformsConnected.Pop();
            Platform fromPlatform = _platformsConnected.Pop();
            var stickmansToRelocate = _stickmansRelocatedInEveryMove.Pop();
            Debug.LogError("stickmansToRelocate: " + stickmansToRelocate);

            for (int i = 0; i < stickmansToRelocate; i++)
            {
                var stickman = _movingStickmans.Pop();
                var placementPos = fromPlatform.stickmanPositions[fromPlatform.GetNextPositionIndex()];

                stickman.transform.SetParent(placementPos);
                stickman.transform.position = placementPos.position;
                stickman.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
                toPlatform.RemoveStickmanFromPlatform(stickman);
                fromPlatform.AddStickmanToPlatform(stickman);
            }

            _bridgeController.RemoveBridgeBetween(fromPlatform, toPlatform);
            toPlatform.Unlock();
        }
    }

    public void ResetActionData()
    {
        _movingStickmans.Clear();
        _platformsConnected.Clear();
        _stickmansRelocatedInEveryMove.Clear();
    }
}
