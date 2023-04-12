using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public bool isInputEnabled;

    [SerializeField] private Camera _camera;

    private Platform _selectedPlatform;

    void FixedUpdate()
    {
        if (!isInputEnabled) return;

        var isJustPressed = Input.GetMouseButtonDown(0);
        var isPressing = Input.GetMouseButton(0);
        if (!isJustPressed && !isPressing) return;

        if (isJustPressed)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200))
            {
                if (hit.transform.gameObject.tag == "Platform")
                {
                    var platform = hit.transform.GetComponent<Platform>();

                    if (_selectedPlatform == null)
                    {
                        platform.OnSelected();
                        _selectedPlatform = platform;
                    }
                    else
                    {
                        if (_selectedPlatform != platform)
                        {
                            isInputEnabled = false; // enable after stickman flow completed
                            _selectedPlatform.OnDeselected();
                            // TODO: Perform stickman flow from first platform to second.
                            _selectedPlatform = null; //if flow is possible
                        }
                        else
                        {
                            _selectedPlatform.OnDeselected();
                            _selectedPlatform = null;
                        }
                    }
                }
                else
                {
                    _selectedPlatform.OnDeselected();
                    _selectedPlatform = null;
                }
            }
        }
    }
}
