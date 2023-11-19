using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Constants
{
    // Input constants
    public const UnityEngine.KeyCode INPUT_JUMP = KeyCode.Space;
    public const string INPUT_HORIZONTAL = "Horizontal";
    public const UnityEngine.KeyCode INPUT_POSITIVE_ROTATION = KeyCode.G;
    public const UnityEngine.KeyCode INPUT_NEGATIVE_ROTATION = KeyCode.F;

    // Physics constants
    public const float rotationAmount = -90f;
    public const float groundDetectionDistance = 0.01f;

    // Camera constants
    public const float deepCameraDistance = 25f;
    public const float upCameraDistance = 4f;
    public static Vector3[] CameraUnitaryPositions = { new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f) };
    public static Vector3[] CameraRotations = { new Vector3(0f, 0f, 0f), new Vector3(0f, Constants.rotationAmount, 0f), new Vector3(0f, 2 * Constants.rotationAmount, 0f), new Vector3(0f, -Constants.rotationAmount, 0f) };

}
