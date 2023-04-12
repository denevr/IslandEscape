using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSelector : MonoBehaviour
{
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _orangeMaterial;
    [SerializeField] private Material _purpleMaterial;

    //public Material GetMaterialFromColor(Colors color)
    //{
    //    switch (color)
    //    {
    //        case Color.blue:
    //            return _blueMaterial;
    //        case Color.green:
    //            return _greenMaterial;
    //        case Color.yellow:
    //            return _yellowMaterial;
    //        case Color.red:
    //            return _redMaterial;
    //        default:
    //            return _blueMaterial;
    //    }
    //}

}
