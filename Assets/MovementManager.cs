using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : GenericSingleton<MovementManager>
{
    // Manager(s) instances
    private InputManager ipM;
    private EnvironmentManager eM;

    // Events
    public event Action<int> HorizontalMovementEvent;
    public event Action JumpMovementEvent;
    public event Action<int> RotationMovementEvent;


    public override void Awake()
    {
        base.Awake();

        // Obtain Manger(s) istances
        ipM = InputManager.Instance;
        eM = EnvironmentManager.Instance;

        // Suscribe to Input events
        /// Horizontal input
        ipM.ToMove += HorizontalInputEvent;
        /// Jump input
        ipM.ToJump += JumpInputEvent;
        /// Rotattion input
        ipM.ToRotate += RotationInputEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HorizontalInputEvent(int way)
    {
        HorizontalMovementEvent.Invoke(way);
    }

    private void JumpInputEvent()
    {
        JumpMovementEvent.Invoke();
    }

    private void RotationInputEvent(int way)
    {
        RotationMovementEvent(way);
    }

}
