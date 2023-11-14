using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : GenericSingleton<InputManager>
{
    public event Action<int> ToMove;
    public event Action ToJump;
    public event Action<int> ToRotate;

    // Input variables
    private float horizontalInput;
    private bool jumpInput;
    private bool positiveRotationInput;
    private bool negativeRotationInput;

    private void Update()
    {
        // Inputs: Lateral movement and jump
        horizontalInput = Input.GetAxis(Constants.INPUT_HORIZONTAL);
        jumpInput = Input.GetKeyDown(Constants.INPUT_JUMP);
        positiveRotationInput = Input.GetKeyDown(Constants.INPUT_POSITIVE_ROTATION);
        negativeRotationInput = Input.GetKeyDown(Constants.INPUT_NEGATIVE_ROTATION);

        // Events
        if (horizontalInput > 0) { ToMove.Invoke(1); }
        else if (horizontalInput < 0) { ToMove.Invoke(-1); }

        if (jumpInput) { ToJump.Invoke(); }

        if (positiveRotationInput) { ToRotate.Invoke(1); }
        else if (negativeRotationInput) { ToRotate.Invoke(-1); }
    }
}
