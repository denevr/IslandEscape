using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRecorder : MonoBehaviour
{
    private readonly Stack<ActionBase> _actions = new Stack<ActionBase>();

    public void Record(ActionBase action)
    {
        _actions.Push(action);
        action.Execute();
    }

    public void Rewind()
    {
        if (_actions.Count != 0)
        {
            var action = _actions.Pop();
            action.Undo();
        }
    }
}
public class StickmanFlowAction : ActionBase
{
    public StickmanFlowAction(StickmanFlowController stickmanFlowController, Platform startPlatform, Platform endPlatform) :
        base(stickmanFlowController, startPlatform, endPlatform)
    {

    }

    public override void Execute()
    {
        _stickmanFlowController.StartFlowBetween(_startPlatform, _endPlatform); 
    }

    public override void Undo()
    {
        _stickmanFlowController.UndoLastMove();
    }
}

public abstract class ActionBase
{
    protected readonly StickmanFlowController _stickmanFlowController;
    protected readonly Platform _startPlatform;
    protected readonly Platform _endPlatform;

    protected ActionBase(StickmanFlowController stickmanFlowController, Platform startPlatform, Platform endPlatform)
    {
        _stickmanFlowController = stickmanFlowController;
        _startPlatform = startPlatform;
        _endPlatform = endPlatform;
    }

    public abstract void Execute();
    public abstract void Undo();
}
