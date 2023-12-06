using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeStats : MonoBehaviour
{
    // Movement
    public float movingForce = Constants.initMovingForce;
    public float maxHorizontalSpeed = Constants.initMaxHorizontalSpeed;
    public float jumpForce = Constants.initJumpForce;
    public float speedAfterStop = Constants.initSpeedAfterStop;

    // Health
    public float health = Constants.initHealth;
}
