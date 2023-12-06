using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeHealth : MonoBehaviour
{

    public FedeStats stats;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Fede has been hit");

        ProcessHit(Constants.fireballDamage);
    }

    private void ProcessHit(float damage)
    {
       stats.health -= damage;
    }
}
