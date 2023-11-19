using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : GenericSingleton<MovementManager>
{
    // Manager(s) instances
    private InputManager ipM;
    ////private EnvironmentManager eM;

    // Camera
    private GameObject Camera;
    private CameraSwitcher cameraSwitcher;

    // Events
    public event Action<int> HorizontalMovementEvent;
    public event Action JumpMovementEvent;
    public event Action<int> RotationMovementEvent;
    public event Action BlockRotation;
    public event Action UnblockRotation;


    public override void Awake()
    {
        base.Awake();

        // Obtain Manger(s) istances
        ipM = InputManager.Instance;
        ////eM = EnvironmentManager.Instance;

        // Obtain Camera
        Camera = GameObject.Find("Camera");
        cameraSwitcher = Camera.GetComponent<CameraSwitcher>();

        // Suscribe to Input events
        /// Horizontal input
        ipM.ToMove += HorizontalInputEvent;
        /// Jump input
        ipM.ToJump += JumpInputEvent;
        /// Rotattion input
        ipM.ToRotate += RotationInputEvent;
        cameraSwitcher.StartRotation += CameraStartRotating;
        cameraSwitcher.EndRotation += CameraDoneRotating;

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

    public void CameraStartRotating()
    {
        BlockRotation.Invoke();
    }

    public void CameraDoneRotating()
    {
        UnblockRotation.Invoke();
    }

}
