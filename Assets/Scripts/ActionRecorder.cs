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
        var action = _actions.Pop();
        action.Undo();
    }
}
public class StickmanFlowAction : ActionBase
{
    //private readonly StickmanFlowController _stickmanFlowController;
    //private readonly Platform _startPlatform;
    //private readonly Platform _endPlatform;

    public StickmanFlowAction(StickmanFlowController stickmanFlowController, Platform startPlatform, Platform endPlatform) :
        base(stickmanFlowController, startPlatform, endPlatform)
    {
        //_stickmanFlowController = stickmanFlowController;
        //_startPlatform = startPlatform;
        //_endPlatform = endPlatform;
    }

    public override void Execute()//Platform startPlatform, Platform endPlatform)
    {
        _stickmanFlowController.StartFlowBetween(_startPlatform, _endPlatform); //
    }

    public override void Undo()
    {
        //_stickmanFlowController.StartFlowBetween(_startPlatform, _endPlatform);
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
