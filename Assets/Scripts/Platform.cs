using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform[] stickmanPositions;
    public bool isCompleted;

    void OnEnable()
    {
        isCompleted = false;
    }

}
