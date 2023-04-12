using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    //public Color color;
    [SerializeField] private SkinnedMeshRenderer _smr;

    public void SetColorMaterial(Material mat)
    {
        _smr.material = mat;
    }
}
