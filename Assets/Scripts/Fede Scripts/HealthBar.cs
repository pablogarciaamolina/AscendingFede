using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private FedeHealth fede;
    private Slider healthBar;
    private float health = Constants.initHealth;

    private void Awake()
    {
        healthBar = this.gameObject.GetComponent<Slider>();
        fede.healthUpdate += InternalHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = Constants.initHealth;
        healthBar.value = Constants.initHealth;
    }

    // Update is called once per frame
    void Update()
    {
        SetHealth(health);
    }

    private void SetHealth(float health)
    {
        healthBar.value = health;
    }

    private void InternalHealth(float h)
    {
        health = h;
    }
}
