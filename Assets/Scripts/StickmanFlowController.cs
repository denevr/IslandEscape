using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StickmanFlowController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, .25f, 0);
    private float _speed = 3f;

    public bool IsFlowAvailableBetween(Platform startPlatform, Platform endPlatform)
    {
        //if (endPlatform.stickmanStack.Count == 16) return false;
        //if (startPlatform.stickmanStack.Count == 0) return false;
        if (endPlatform.stickmans.Count == 16) return false;
        if (startPlatform.stickmans.Count == 0) return false;
        if (startPlatform.GetLastStickmanColor() != endPlatform.GetLastStickmanColor()) return false;
        if (startPlatform.GetTransferableStickmans().Count > 16 - endPlatform.stickmans.Count) return false; //pop problemi

        Debug.LogError("Flow available!");
        return true;
    }

    public void StartFlowBetween(Platform startPlatform, Platform endPlatform)
    {
        var stickmans = startPlatform.GetTransferableStickmans();
        //var startPos = startPlatform.GetConnectionPoint() + offset;
        //var endPos = endPlatform.GetConnectionPoint() + offset;

        //for (int i = 0; i < stickmans.Count; i++)
        //{
        //    var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
        //    StartCoroutine(MoveStickman(stickmans[i], startPos, endPos, placementPos, i / 2));
        //}

        StartCoroutine(MoveStickmans(stickmans, startPlatform, endPlatform));
    }

    private IEnumerator MoveStickmans(List<Stickman> stickmans, Platform startPlatform, Platform endPlatform)
    {
        var startPos = startPlatform.GetConnectionPoint() + offset;
        var endPos = endPlatform.GetConnectionPoint() + offset;

        for (int i = 0; i < stickmans.Count; i++)
        {
            var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
            var stickman = stickmans[i];
            stickman.transform.SetParent(placementPos);

            Vector3[] path = new[] { stickman.transform.position, startPos, endPos, placementPos.position };

            //float duration = i * .15f;
            float duration = .15f;
            yield return new WaitForSeconds(duration);

            stickman.Run();
            var distance = Vector3.Distance(startPos, endPos);
            stickman.transform.DOPath(path, distance / _speed)
                .SetLookAt(.01f, -Vector3.forward)
                //.SetSpeedBased(true)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    stickman.transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
                    stickman.Idle();
                });
        }

        //deconstruct bridge + enable input + add newcomer stickmans to new platforms stickmans
    }
}
