using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvisibleCube : MonoBehaviour
{
    public event Action CollidedWithPlayer;
    public event Action ColisionExit;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnCollisionExit(Collision collision)
    {
        ColisionExit.Invoke();

    }

    private void OnCollisionEnter(Collision collision)
    {
        CollidedWithPlayer.Invoke();

    }

    
}
