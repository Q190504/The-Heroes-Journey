using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]

public class AIController : InputController
{
    public override bool RetrieveJumpHoldInput()
    {
        return false;
    }

    public override bool RetrieveJumpInput()
    {
        return false;
    }

    public override float RetrieveMoveInput()
    {
        return 1f;
    }

}
