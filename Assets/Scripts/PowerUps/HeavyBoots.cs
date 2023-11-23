using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBoots : MonoBehaviour
{
    
    public event System.Action HeavyBootsPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HeavyBootsPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
