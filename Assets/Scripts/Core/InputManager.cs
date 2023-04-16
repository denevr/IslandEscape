using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isInputEnabled;

    [SerializeField] private Camera _camera;
    [SerializeField] private BridgeController _bridgeController;
    [SerializeField] private StickmanFlowController _stickmanFlowController;
    [SerializeField] private ActionRecorder _actionRecorder;

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
                        if (platform.stickmans.Count == 0) return;

                        platform.OnSelected();
                        _selectedPlatform = platform;
                    }
                    else
                    {
                        if (_selectedPlatform != platform)
                        {
                            if (!_stickmanFlowController.IsFlowAvailableBetween(_selectedPlatform, platform)) return;

                            isInputEnabled = false;
                            _selectedPlatform.OnDeselected();
                            _bridgeController.CreateBridgeBetween(_selectedPlatform, platform);
                            var action = new StickmanFlowAction(_stickmanFlowController, _selectedPlatform, platform);
                            _actionRecorder.Record(action);
                            _selectedPlatform = null;
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
                    if (_selectedPlatform != null)
                    {
                        _selectedPlatform.OnDeselected();
                        _selectedPlatform = null;
                    }
                }
            }
        }
    }
}
