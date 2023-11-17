using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FedeMovement : MonoBehaviour
{
    // Custom Game parameters
    [Header("Parameters")]
    [SerializeField] float movingForce = 35f;
    [SerializeField] float maxHorizontalSpeed = 10f;
    [SerializeField] float jumpForce = 250f;

    // Constants
    private const float rotationAmount = -90f;
    private const float groundDetectionDistance = 0.01f;

    // Manager(s)
    private MovementManager mvM;

    // Physiscs elements
    private Rigidbody rb;
    private BoxCollider bc;

    // Moving Direction
    private List<Vector3> Directions = new List<Vector3>() { new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, -1f) };
    private int directionIndex = 0;
    private Vector3 direction;

    // Moving Input
    private int moveHorizontal = 0f; // 1 for right, -1 for left. 0 no movement
    private bool doJump = false;
    private int rotateSense = 0f; // 1 for positive rotation, -1 for negative rotation. 0 no rotation

    //Movement variables
    private Vector3 movingVector;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        // Obatain physic objects
        rb = gameObject.GetComponent<Rigidbody>();
        bc = gameObject.GetComponent<BoxCollider>();

        // Get Manager(s) instance(s)
        mvM = MovementManager.Instance

        // Suscribe to movement orders (input derived)
        /// Horizontal movement
        mvM.HorizontalMovementEvent += SetMoveHorizontal
        /// Jump movement
        mvM.JumpMovementEvent += SetDoJump;
        /// RotationMovement
        mvM.RotationMovementEvent += SetRotate

        // Initialize direction
        direction = Directions[directionIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input
        ///moveHorizontal = Input.GetAxis("Horizontal");
        ///doJump = Input.GetKeyDown(KeyCode.Space);
        ///rotatePositive = Input.GetKeyDown(KeyCode.G);
        ///rotateNegative = Input.GetKeyDown(KeyCode.F);

        // Check grounded
        isJumping = !IsGrounded();

        // Prepare Direction
        if (rotateSense)  { ChangeDirection(rotateSense); }

        // Prepare movement
        movingVector = PrepareStraightMove();
        movingVector += PrepareJump();

        // Limit movement
        LimitVelocity();

        // Do movement
        ApplyForce(movingVector);

    }

    // Quick implementation of modulo operation
    private int Mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }
    
    private void SetMoveHorizontal(int way)
    {
        moveHorizontal = way;
    }

    private void SetDoJump() 
    {
        doJump = true;
    }

    private void SetRotate(int way) 
    {
        rotateSense = way
    }

    private void LimitVelocity()
    {
        if (Mathf.Abs(rb.velocity.x) >= maxHorizontalSpeed)
        {
            movingVector.x = 0;
        }
    }

    private void ApplyForce(Vector3 direction)
    {
        rb.AddForce(direction);
        moveHorizontal = 0;
    }

    private Vector3 PrepareStraightMove()
    {
        return direction * moveHorizontal * movingForce;
    }

    private Vector3 PrepareJump()
    {
        Vector3 jumpVector = Vector3.zero;
        if (doJump && !isJumping)
        {
            jumpVector += Vector3.up * jumpForce;
            isJumping = true;
        }
        isJumping = false;

        return jumpVector;
    }

    private void ChangeDirection(int way)
    {
        directionIndex = Mod(directionIndex + way, Directions.Count);
        direction = Directions[directionIndex];
        RotateByAmount(rotationAmount*way);
        rotateSense = 0;
    }

    private void RotateByAmount(float amount)
    {
        // Rotate the object around its up axis by the specified amount
        transform.Rotate(Vector3.up, amount);
    }

    bool IsGrounded()
    {
        // Create a Raycast hit variable to store information about the hit
        RaycastHit hit;

        // Create a Ray from the center of the player downwards
        Ray ray = new Ray(transform.position, Vector3.down);

        // Adjust the raycast distance based on the player's height
        float raycastDistance = bc.bounds.extents.y + groundDetectionDistance;

        // Perform the Raycast
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // If the Raycast hits something below the player, return true (player is grounded)
            return true;
        }

        // If the Raycast doesn't hit anything below the player, return false (player is not grounded)
        return false;
    }

}
