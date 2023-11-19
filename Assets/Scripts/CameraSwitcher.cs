using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraSwitcher : MonoBehaviour
{

    private MovementManager mvM;
    
    private int currentIndex = 0;
    private bool isRotating = false;
    private float rotationTime = 0.5f;

    public event Action StartRotation;
    public event Action EndRotation;

    void Awake()
    {
        // Get MovementManager
        mvM = MovementManager.Instance;

        // Suscribe to rotation event
        mvM.RotationMovementEvent += RotationInput;

        // Set the initial position and rotation of the camera.
        SetCamera(currentIndex);
    }

    private void StartTransition(int newIndex)
    {
        currentIndex = Mod(newIndex, Constants.CameraUnitaryPositions.Length);
        StartCoroutine(TransitionCamera());
    }

    IEnumerator TransitionCamera()
    {
        isRotating = true;
        StartRotation.Invoke();
        float elapsedTime = 0f;

        // Position to transist between
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = Constants.CameraUnitaryPositions[currentIndex] * Constants.deepCameraDistance;
        targetPosition.y = Constants.upCameraDistance;

        // Rotations to transist between
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(Constants.CameraRotations[currentIndex]);

        while (elapsedTime < rotationTime)
        {
            transform.position = Vector3.Slerp(initialPosition, targetPosition, elapsedTime / rotationTime);
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetCamera(currentIndex);
        isRotating = false;
        EndRotation.Invoke();
    }

    private void SetCamera(int index)
    {
        // Set position
        transform.position = Constants.CameraUnitaryPositions[currentIndex] * Constants.deepCameraDistance;
        transform.position +=  Vector3.up * Constants.upCameraDistance;

        // Set rotation
        transform.eulerAngles = Constants.CameraRotations[index];

    }

    private void RotationInput(int way)
    {
        if (!isRotating) 
        {
            StartTransition(currentIndex + way);
        }
    }

    private int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
