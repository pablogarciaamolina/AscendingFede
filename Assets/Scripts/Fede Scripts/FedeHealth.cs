using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeHealth : MonoBehaviour
{

    public FedeStats stats;

    // Terrain variables
    private bool beingBurned = false;

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

    public void StartBurning()
    {
        beingBurned = true;

        if (!stats.isBurning)
        {
            stats.isBurning = true;
            StartCoroutine(Burning());
        }
    }

    public void StopBurning()
    {
        beingBurned = false;
        stats.isBurning = false;
    }

    IEnumerator Burning()
    {
        while (beingBurned)
        {
            stats.health -= Constants.fireDamage;

            yield return new WaitForSeconds(Constants.rateFireDamage);
        }
    }

}
