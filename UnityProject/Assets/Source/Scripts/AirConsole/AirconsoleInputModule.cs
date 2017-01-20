using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AirconsoleInputModule : BaseInputModule
{

    public override void ActivateModule()
    {
        //Debug.Log("activateModuel");
        base.ActivateModule();

        var toSelect = eventSystem.currentSelectedGameObject;
        if (toSelect == null)
            toSelect = eventSystem.firstSelectedGameObject;

        eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
    }

    public override void DeactivateModule()
    {
        //Debug.Log("DeactivateModule");
        base.DeactivateModule();
    }

    public override void Process()
    {
        //Debug.Log("process");
        ProcessMovement();
        ProcessButtons();
        AirConsoleManager.Instance.ResetInput(true);
    }

    void ProcessMovement()
    {

        for (int i = 0; i < 4; i++)
        {
            // Get the axis move event
            var axisEventData = GetAxisEventData(AirConsoleManager.Instance.GetPlayer(i).GetAxis(InputAction.GamePlay.MoveHorizontal), AirConsoleManager.Instance.GetPlayer(i).GetAxis(InputAction.GamePlay.MoveVertical), 0.6f);
            if (axisEventData.moveDir == MoveDirection.None) continue; // input vector was not enough to move this cycle, done

            //Debug.Log("movement... " + axisEventData.moveVector.x + " : " + axisEventData.moveVector.y);

            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        }
    }

    void ProcessButtons()
    {
        for (int i = 0; i < 4; i++)
        {

            //Debug.Log(AirConsoleManager.Instance.GetPlayer(0).GetButtonDown(InputAction.Menu.UISubmit));

            if (eventSystem.currentSelectedGameObject == null)
                return;

            var data = GetBaseEventData();

            if (AirConsoleManager.Instance.GetPlayer(i).GetButtonDown(InputAction.Menu.UISubmit))
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);
            }

            if (AirConsoleManager.Instance.GetPlayer(i).GetButtonDown(InputAction.Menu.UICancel))
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
            }
        }
    }
}
