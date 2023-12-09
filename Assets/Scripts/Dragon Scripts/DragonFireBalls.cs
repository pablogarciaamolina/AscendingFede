using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireBalls : MonoBehaviour
{
    private Animator _animator;
    private DragonStats stats;
    private ParticleSystem systemOfParticles;
    private GameObject fede;
    private DragonStageManager stageManager;

    // Start is called before the first frame update
    void Awake()
    {
        stageManager = GetComponent<DragonStageManager>();
        stats = gameObject.GetComponent<DragonStats>();
        _animator = GetComponent<Animator>();
        systemOfParticles = GetComponent<ParticleSystem>();
        fede = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(ShootingFireballs());
    }

    IEnumerator ShootingFireballs()
    {
        while (stats.shooting)
        {
            // Find Fede's position
            Vector3 position = fede.transform.position;

            // Emit fireball and animate
            _animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(Constants.dragonShootingAnimationDelay);

            // Orient particle system
            systemOfParticles.transform.LookAt(position);
            // Shoot
            systemOfParticles.Emit(stats.numFireballs);

            yield return new WaitForSeconds(stats.fireballRate);
        }
    }
}
