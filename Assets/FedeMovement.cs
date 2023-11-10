using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D)) 
        {
            MoveDirection(Vector3.right);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveDirection(Vector3.left);
        }
        if (Input.GetKey(KeyCode.W))
        {
            MoveDirection(Vector3.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveDirection(Vector3.down);
        }
    }

    private void MoveDirection(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime;
    }
}
