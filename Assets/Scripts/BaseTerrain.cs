using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTerrain : MonoBehaviour
{

    public virtual void ModifyStats(ref FedeStats stats)
    {
        // Reset Movement
        stats.speedAfterStop = Constants.initSpeedAfterStop;
        stats.movingForce = Constants.initMovingForce;
        stats.speedAfterStop = Constants.initSpeedAfterStop;
        stats.jumpForce = Constants.initJumpForce;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        ModifyStats(ref other.GetComponent<FedeMovement>().stats);
    }

    /*
    protected virtual void OnTrigerEnter(Collision collision)
    {
        ModifyStats(ref collision.gameObject.GetComponent<FedeMovement>().stats);
    }
    */
}
