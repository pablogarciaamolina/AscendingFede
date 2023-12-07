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
    public const int rotationAmount = -90;
    public const float groundDetectionDistance = 0.2f;
    public static Vector3 Center = new Vector3(0f, 0f, 0f);

    // Player contants
    /// Initial status
    public static Vector3 playerInitialPosition = new Vector3(0f, 5f, -15f);
    public const float maxHealth = 220;
    /// Movements constants
    public static List<Vector3> Directions = new List<Vector3>() { new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, -1f) };

    // Camera constants
    public const float deepCameraDistance = 50f;
    public const float upCameraDistance = 2f;

    // Dragon constants
    public const float flyingHeightAboveCharacter = 20f;
    public const float UpDownTime = 2f;
    public const float distanceUpDownFromCenter = 5f;
    public const float rotationSpeed = 5f;
    public const float multiplierTimeRotatingInOneSense = 1;
    public const float fireballRate = 3f;
    public const float fireballDamage = 10f;

}
