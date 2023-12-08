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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        ModifyMovement(ref other.GetComponent<FedeMovement>().stats);
        ModifyHealth(other.GetComponent<FedeHealth>());
    }

}
