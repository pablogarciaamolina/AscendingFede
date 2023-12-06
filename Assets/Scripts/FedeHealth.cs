using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeHealth : MonoBehaviour
{

    public FedeStats stats;

    private void Start()
    {
        stats = gameObject.GetComponent<FedeStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit(Constants.fireballDamage);
    }

    private void ProcessHit(float damage)
    {
       stats.health -= damage;
    }
}
