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
    public event Action CameraStartRotation;
    public event Action CameraEndRotation;
    public event Action<float> CharacterChangeOfLevel;



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

        // Suscrbibe to Movement events
        PowerUpManager.DoubleJumpPickedUp += DoubleJumpPickedUpEvent;
        PowerUpManager.HeavyBootsPickedUp += HeavyBootsPickedUpEvent;
        PowerUpManager.ArmorPickedUp += ArmorPickedUpEvent;

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
        CameraStartRotation.Invoke();
    }

    public void CameraDoneRotating()
    {
        CameraEndRotation.Invoke();
    }

    public void ManageCharacterLevelChange(float newLevel)
    {
        CharacterChangeOfLevel.Invoke(newLevel);
    }

    private void DoubleJumpPickedUpEvent()
    {
        JumpMovementEvent += DoubleJumpMovementEvent;
    }

    private void HeavyBootsPickedUpEvent()
    {
        HorizontalMovementEvent += HeavyBootsMovementEvent;
    }

    private void ArmorPickedUpEvent()
    {
        JumpMovementEvent += ArmorMovementEvent;
    }
}
