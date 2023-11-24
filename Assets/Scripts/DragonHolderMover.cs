using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHolderMover : MonoBehaviour
{

    private float rotationSpeed = 5f;
    private float multiplierTimeRotatingInOneSense= 1;

    private bool rotating = false;
    private float timeRotatingInOneSense;
    private float currentTime = 0f;
    private bool timerIsRunning = true;
    private int sense = 1;

    // Start is called before the first frame update
    void Start()
    {
        timeRotatingInOneSense = Random.Range(0, multiplierTimeRotatingInOneSense);
    }

// Update is called once per frame
void Update()
    {
        if (!rotating)
        {
            StartCoroutine(Pivot(sense));
        }
        
        if (timerIsRunning)
        {
            currentTime += Time.deltaTime;
        }

        if (currentTime >= timeRotatingInOneSense)
        {
            sense *= -1;
            currentTime = 0f;
            timeRotatingInOneSense = Random.Range(0, multiplierTimeRotatingInOneSense);
        }
    }

    IEnumerator Pivot(int sense)
    {
        rotating = true;

        // Rotations to transist between
        /// initial
        Quaternion initialRotation = transform.rotation;
        /// target
        float degree = initialRotation.eulerAngles.y + sense * Constants.rotationAmount;
        Quaternion targetRotation = Quaternion.Euler(0, degree, 0);

        float elapsedTime = 0f;
        while (elapsedTime < rotationSpeed)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotating = false;

    }

}
