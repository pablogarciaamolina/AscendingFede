using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeHealth : MonoBehaviour
{

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = Constants.maxHealth;
    }

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
        health -= damage;
    }
}
