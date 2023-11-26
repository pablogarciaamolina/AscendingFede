using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireBalls : MonoBehaviour
{
    private ParticleSystem systemOfParticles;
    private GameObject fede;

    // Start is called before the first frame update
    void Awake()
    {
        systemOfParticles = GetComponent<ParticleSystem>();
        fede = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        InvokeRepeating("ShootFireBall", 0f, Constants.fireballRate);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void ShootFireBall()
    {
        // Find Fede's position
        Vector3 position = fede.transform.position;

        // Orient particle system
        systemOfParticles.transform.LookAt(position);

        // Emit fireball
        systemOfParticles.Emit(1);
    }
}
