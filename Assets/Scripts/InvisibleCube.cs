using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvisibleCube : MonoBehaviour
{
    // Start is called before the first frame update

    public event Action CollidedWithPlayer;
    public event Action ColisionExit;
    void Start()
    {
        
    }

    // Update is called once per frame
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
