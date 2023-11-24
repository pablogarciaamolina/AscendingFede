using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPowerUp : MonoBehaviour
{
    public float duration = 5f;

    public virtual void Awake()
    {
        // destroy the power up after a certain amount of time
        Destroy(gameObject, duration);
    }
}
