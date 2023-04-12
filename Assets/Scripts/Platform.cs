using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform[] stickmanPositions;
    public List<Stickman> stickmans;
    //public Stack<Stickman> stickmanStack;
    public bool isCompleted;

    [SerializeField] private Transform _bridgeConnectionPoint;

    private bool isSelected; //
    private LineRenderer _lineRenderer;
    private const int lengthOfLineRenderer = 2;
    private readonly Vector3 offsetVector = new Vector3(0, .25f, 0);

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        //stickmans = new Stickman[stickmanPositions.Length];
        stickmans = new List<Stickman>();
        //stickmanStack = new Stack<Stickman>();
        isCompleted = false;
        isSelected = false;
        _lineRenderer.enabled = false;
    }

    public void OnSelected()
    {
        isSelected = true;
        var pos = transform.position;
        pos.y += .5f;
        transform.position = pos;
    }

    public void OnDeselected()
    {
        isSelected = false;
        var pos = transform.position;
        pos.y -= .5f;
        transform.position = pos;
    }

    public void SetReadyForConnecting()
    {
        var platformCenterPos = Vector3.zero + offsetVector;
        var points = new Vector3[lengthOfLineRenderer] { platformCenterPos, _bridgeConnectionPoint.localPosition };

        _lineRenderer.SetPositions(points);
        _lineRenderer.enabled = true;
    }

    public void TerminateConnection()
    {
        _lineRenderer.enabled = false;
    }

    public Vector3 GetConnectionPoint()
    {
        return _bridgeConnectionPoint.position;
    }

    public List<Stickman> GetTransferableStickmans()
    {
        List<Stickman> stickmansToMove = new List<Stickman>();
        var color = GetLastStickmanColor();

        //while (stickmanStack.Peek().GetColor() == color)
        //{
        //    stickmansToMove.Add(stickmanStack.Pop());
        //}

        //return stickmansToMove;


        for (int i = stickmans.Count; i > 0; i--)
        {
            var stickman = stickmans[i - 1];

            if (stickman.GetColor() == color)
                stickmansToMove.Add(stickman);
            else
                break;
        }

        return stickmansToMove;
    }

    public Colors GetLastStickmanColor()
    {
        //return stickmanStack.Peek().GetColor();
        return stickmans[stickmans.Count - 1].GetColor();
    }

    public int GetNextPositionIndex()
    {
        for (int i = 0; i < stickmanPositions.Length; i++)
        {
            if (stickmanPositions[i].transform.childCount == 0)
                return i;
        }

        return -1;
    }
}
