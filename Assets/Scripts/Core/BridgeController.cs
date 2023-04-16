using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void CreateBridgeBetween(Platform startPlatform, Platform endPlatform)
    {
        if (startPlatform == null ||
            endPlatform == null) return;

        startPlatform.SetReadyForConnecting();
        var points = new Vector3[2] { startPlatform.GetConnectionPoint(), endPlatform.GetConnectionPoint() };
        _lineRenderer.SetPositions(points);
        endPlatform.SetReadyForConnecting();

        _lineRenderer.enabled = true;
    }

    public void RemoveBridgeBetween(Platform startPlatform, Platform endPlatform)
    {
        startPlatform.TerminateConnection();
        _lineRenderer.enabled = false;
        endPlatform.TerminateConnection();
    }
}
