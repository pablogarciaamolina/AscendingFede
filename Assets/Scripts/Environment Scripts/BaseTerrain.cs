using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTerrain : MonoBehaviour
{

    public virtual void ModifyMovement(ref FedeStats stats)
    {
        // Reset Movement
        stats.speedAfterStop = Constants.initSpeedAfterStop;
        stats.movingForce = Constants.initMovingForce;
        stats.jumpForce = Constants.initJumpForce;
        stats.maxHorizontalSpeed = Constants.initMaxHorizontalSpeed;
    }

    public virtual void ModifyHealth(FedeHealth fedeHealth) 
    {
        fedeHealth.StopBurning();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        ModifyMovement(ref other.GetComponent<FedeMovement>().stats);
        ModifyHealth(other.GetComponent<FedeHealth>());
    }

}
