using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StickmanFlowController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, .25f, 0);

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
        var startPos = startPlatform.GetConnectionPoint() + offset;
        var endPos = endPlatform.GetConnectionPoint() + offset;

        for (int i = 0; i < stickmans.Count; i++)
        {
            var placementPos = endPlatform.stickmanPositions[endPlatform.GetNextPositionIndex()];
            StartCoroutine(MoveStickman(stickmans[i], startPos, endPos, placementPos));
        }
    }

    private IEnumerator MoveStickman(Stickman stickman, Vector3 startPos, Vector3 endPos, Transform placementPos)
    {
        //animation
        //async
        //direction facing
        //movement duration
        //rotate according to target platform
        stickman.Run();
        stickman.transform.SetParent(placementPos);

        yield return new WaitForSeconds(.5f);

        stickman.transform.DOMove(startPos, .25f).OnComplete(() =>
        {
            stickman.transform.DOMove(endPos, .75f).OnComplete(() =>
            {
                stickman.transform.DOMove(placementPos.position, .25f);
                stickman.Idle();
            });
        });
    }
}
