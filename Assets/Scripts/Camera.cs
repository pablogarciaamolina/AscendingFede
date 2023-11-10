using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class CameraSwitcher : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the camera's Transform component.
    // public Transform playerTransform;  // Reference to the player's Transform component.
    [SerializeField] public float cameraHeight = 2.5f; // The height of the camera above the player.
    [SerializeField] public Vector3[] positions;        // Array of position sets.
    [SerializeField] public Vector3[] rotations;        // Array of rotation sets.
    private int currentIndex = 0;

    // We need some more variables for the smooth camera movement.
    private bool isRotating = false;
    private float rotationTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial position and rotation of the camera.
        SetCamera(currentIndex);
    }

    void Update()
    {
        if (!isRotating) // We may switch the camera only if it is not rotating.
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartTransition(currentIndex + 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartTransition(currentIndex - 1);
            }
        }

        //UpdateCamera();
    }

    private void StartTransition(int newIndex)
    {
        currentIndex = Mod(newIndex, positions.Length);
        StartCoroutine(TransitionCamera());
    }

    IEnumerator TransitionCamera()
    {
        isRotating = true;
        float elapsedTime = 0f;
        Vector3 initialPosition = cameraTransform.position;
        Vector3 targetPosition = new Vector3(positions[currentIndex].x, cameraTransform.position.y, positions[currentIndex].z);
        Quaternion initialRotation = cameraTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(rotations[currentIndex]);

        while (elapsedTime < rotationTime)
        {
            cameraTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / rotationTime);
            cameraTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetCamera(currentIndex);
        isRotating = false;
    }

    //private void UpdateCamera()
    //{
        //if (playerTransform != null)
        //{
        //    cameraTransform.position = new Vector3(cameraTransform.position.x, playerTransform.position.y + cameraHeight, cameraTransform.position.z);
        //}
    //}

    private void SetCamera(int index)
    {
        cameraTransform.position = new Vector3(positions[index].x, cameraTransform.position.y, positions[index].z);
        cameraTransform.eulerAngles = rotations[index];

        // Rotate the player to face the camera.
        //playerTransform.eulerAngles = new Vector3(0, rotations[index].y, 0);
    }

    private int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
