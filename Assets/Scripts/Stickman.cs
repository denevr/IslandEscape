using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    //public Color color;
    [SerializeField] private SkinnedMeshRenderer _smr;
    [SerializeField] private Animator _animator;

    private Colors color;

    void OnEnable()
    {
        Idle();
    }

    public void SetColorMaterial(Material mat)
    {
        _smr.material = mat;
    }

    public void SetColor(Colors col)
    {
        color = col;
    }

    public Colors GetColor()
    {
        return color;
    }

    public void Run()
    {
        _animator.SetBool("Run", true);
    }

    public void Idle()
    {
        _animator.SetBool("Run", false);
    }
}
