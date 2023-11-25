using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class CameraSwitcher : MonoBehaviour
{

    private MovementManager mvM;
    
    private bool isRotating = false;
    private float rotationTime = 0.5f;

    public event Action StartRotation;
    public event Action EndRotation;

    void Awake()
    {
        // Initialize position
        SetInitialPosition();

        // Get MovementManager
        mvM = MovementManager.Instance;

        // Suscribe to rotation event
        mvM.RotationMovementEvent += RotationInput;

        // Suscribe to event to change the height
        mvM.CharacterChangeOfLevel += ChangeLevel;

    }

    private void SetInitialPosition()
    {
        transform.position = new Vector3(Constants.Center.x, Constants.playerInitialPosition.y + Constants.upCameraDistance, Constants.Center.z);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator Pivot(int sense)
    {
        isRotating = true;
        StartRotation.Invoke();

<<<<<<< HEAD
=======

>>>>>>> SNDB-v2
        // Rotations to transist between
        /// initial
        Quaternion initialRotation = transform.rotation;
        /// target
        float degree = initialRotation.eulerAngles.y + sense * Constants.rotationAmount;
        Quaternion targetRotation = Quaternion.Euler(0, degree, 0);
      
        float elapsedTime = 0f;
        while (elapsedTime <= rotationTime)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationTime);

            elapsedTime += 0.005f;
            yield return null;
        }

        isRotating = false;
        EndRotation.Invoke();
    }

    IEnumerator TransitionToLevel(float newLevel)
    {
        float elapsedTime = 0f;

        // Position to transist between
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, newLevel, transform.position.z);

        while (elapsedTime < rotationTime)
        {
            transform.position = Vector3.Slerp(initialPosition, targetPosition, elapsedTime / rotationTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    private void ChangeLevel(float newHeight)
    {
        StartCoroutine(TransitionToLevel(newHeight + Constants.upCameraDistance));
    }

    private void RotationInput(int way)
    {
        if (!isRotating) 
        {
            StartCoroutine(Pivot(way));
        }
    }

}
