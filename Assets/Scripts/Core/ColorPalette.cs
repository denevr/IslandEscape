using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colors
{
    Blue,
    Green,
    Yellow,
    Red,
    Orange,
    Purple,
    None
}

public class ColorPalette : MonoBehaviour
{
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _orangeMaterial;
    [SerializeField] private Material _purpleMaterial;
    [SerializeField] private Material _defaultMaterial;

    public Material GetMaterialFromColor(Colors color)
    {
        switch (color)
        {
            case Colors.Blue:
                return _blueMaterial;
            case Colors.Green:
                return _greenMaterial;
            case Colors.Yellow:
                return _yellowMaterial;
            case Colors.Red:
                return _redMaterial;
            case Colors.Orange:
                return _orangeMaterial;
            case Colors.Purple:
                return _purpleMaterial;

            default:
                return _defaultMaterial;
        }
    }


}
