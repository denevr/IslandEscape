using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform[] stickmanPositions;
    public bool isCompleted;

    private bool isSelected;

    void OnEnable()
    {
        isCompleted = false;
        isSelected = false;
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
}
