using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FedeHealth : MonoBehaviour
{
    [SerializeField] private HealthBar Bar;
    private float health;

    public Action<float> healthUpdate;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        health = Constants.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthUpdate.Invoke(health);
    }

    private void OnParticleCollision(GameObject other)
    {

        ProcessHit(Constants.fireballDamage);
    }

    private void ProcessHit(float damage)
    {
        health -= damage;
    }
}
