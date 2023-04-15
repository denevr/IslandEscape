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

    //private List<Stickman> _movingStickmans = new List<Stickman>(); //reset these on level start
    private Stack<Stickman> _movingStickmans = new Stack<Stickman>(); //reset these on level start
    private Stack<Platform> _platformsConnected = new Stack<Platform>();
    private Platform _fromPlatform, _toPlatform;
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

        //_movingStickmans.Clear();
        var stickmansToMove = stickmans.Count;
        _movedStickmanCountOnTheLastMove = stickmansToMove; //no need for a new variable?
        //SetDataBeforeFlow(startPlatform, endPlatform);
        //_fromPlatform = startPlatform;
        //_toPlatform = endPlatform;
        _platformsConnected.Push(startPlatform);
        _platformsConnected.Push(endPlatform);

        for (int i = 0; i < stickmansToMove; i++)
        {
            var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
            var stickman = stickmans[i];
            //_movingStickmans.Add(stickman);
            _movingStickmans.Push(stickman);
            //stickman.transform.SetParent(placementPos);
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
            //Debug.LogError(stickmansToMove);
            //Debug.LogError(endPlatform.GetNextPositionIndex());
            var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
            var stickman = stickmans[i];
            stickman.transform.SetParent(placementPos);
            //startPlatform.RemoveStickmanFromPlatform(stickman);
            //endPlatform.AddStickmanToPlatform(stickman);
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

    //public void SetDataBeforeFlow(Platform startPlatform, Platform endPlatform)
    //{
    //    _fromPlatform = startPlatform;
    //    _toPlatform = endPlatform;


    //}

    public void UndoLastMove()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_movingStickmans.Count != 0 && 
            _movingStickmans.Count >= _movedStickmanCountOnTheLastMove &&
            _platformsConnected.Count >= 2)
        {
            //Debug.LogError(_platformsConnected.Count);
            Platform toPlatform = _platformsConnected.Pop();
            Platform fromPlatform = _platformsConnected.Pop();

            for (int i = 0; i < _movedStickmanCountOnTheLastMove; i++)
            {
                var stickman = _movingStickmans.Pop();
                var placementPos = fromPlatform.stickmanPositions[fromPlatform.GetNextPositionIndex()];

                stickman.transform.SetParent(placementPos);
                stickman.transform.position = placementPos.position;
                stickman.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
                toPlatform.RemoveStickmanFromPlatform(stickman);
                fromPlatform.AddStickmanToPlatform(stickman);


                //var stickman = _movingStickmans[i];
                //var placementPos = _fromPlatform.stickmanPositions[_fromPlatform.GetNextPositionIndex()];

                //stickman.transform.SetParent(placementPos);
                //stickman.transform.position = placementPos.position;
                //stickman.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
                //_toPlatform.RemoveStickmanFromPlatform(stickman);
                //_fromPlatform.AddStickmanToPlatform(stickman);
            }

            //_toPlatform.Unlock();
            toPlatform.Unlock();
        }
    }
}
