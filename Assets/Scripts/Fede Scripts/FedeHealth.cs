using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FedeHealth : MonoBehaviour
{
    [SerializeField] private HealthBar Bar;

    public Action<float> healthUpdate;
    public FedeStats stats;

    private bool beingBurned = false;
    private bool beingHealed = false;


    private void Awake()
    {
        stats = gameObject.GetComponent<FedeStats>();
    }
    private void OnParticleCollision(GameObject other)
    {
        ProcessHit(Constants.fireballDamage);
    }

    private void ProcessHit(float damage)
    {
        stats.health -= damage;
        healthUpdate.Invoke(stats.health);
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

    public void StartHealing()
    {
        beingHealed = true;

        if (!stats.isHealing)
        {
            stats.isHealing = true;
            StartCoroutine(Healing());
        }
    }

    public void StopHealing()
    {
        beingHealed = false;
        stats.isHealing = false;
    }

    IEnumerator Burning()
    {
        while (beingBurned)
        {
            stats.health -= Constants.fireDamage;
            healthUpdate.Invoke(stats.health);

            yield return new WaitForSeconds(Constants.rateFireDamage);
        }
    }

    IEnumerator Healing()
    {
        while (beingHealed)
        {
            if (stats.health < Constants.initHealth)
            {
                stats.health += Constants.healthHealed;
                healthUpdate.Invoke(stats.health);
            }
            else
            {
                stats.health = Constants.initHealth;
            }

            yield return new WaitForSeconds(Constants.healingRate);
        }
    }

}
