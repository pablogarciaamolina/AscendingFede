using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FedeMovement : MonoBehaviour
{
    // Player stats
    public FedeStats stats;

    // Manager(s)
    private MovementManager mvM;
    private EnvironmentManager eM;

    // Physiscs elements
    private Rigidbody rb;
    private BoxCollider cc;

    // Animation elements
    public AnimationClip _walk, _jump;
    public Animation _Legs;
    private bool doAnimateJump = false;

    // Moving Direction
    private int directionIndex = 0;
    private Vector3 direction;
    private int lookingTo = 1; // 1 to look right, -1 to look left

    // Moving Input
    private int moveHorizontal = 0; // 1 for right, -1 for left. 0 no movement
    private bool doJump = false;
    private int rotateSense = 0; // 1 for positive rotation, -1 for negative rotation. 0 no rotation

    //Movement variables
    private Vector3 movingVector;
    private bool isJumping;
    private bool blockedRotation = false;
    private bool oneRotation = false;
    private float actualHeight; // Useful to determine if the level (grounded height) changes

    // EVENTS
    public event Action<float> LevelChangedEvent;
    public event Action<Vector3> SideChangeEvent;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize stats
        stats = gameObject.GetComponent<FedeStats>();

        // Intialize position
        transform.position = Constants.playerInitialPosition;

        // Obatain physic objects
        rb = gameObject.GetComponent<Rigidbody>();
        cc = gameObject.GetComponent<BoxCollider>();

        // Get Manager(s) instance(s)
        mvM = MovementManager.Instance;
        eM = EnvironmentManager.Instance;

        // Suscribe to movement orders (input derived)
        /// Horizontal movement
        mvM.HorizontalMovementEvent += SetMoveHorizontal;
        /// Jump movement
        mvM.JumpMovementEvent += SetDoJump;
        /// RotationMovement
        mvM.RotationMovementEvent += SetRotate;
        mvM.CameraStartRotation += CameraBeginRotation;
        mvM.CameraEndRotation += CameraEndRotation;
        // player depth movement
        //eM.SendPositionEvent += SendPosition;
        //eM.setPlayertoBlock += SetPlayerDepth;

        // Set suscriptions to events of this class
        LevelChangedEvent += mvM.ManageCharacterLevelChange;
        //SideChangeEvent += eM.ManageSideChange;

        // Initialize direction
        direction = Constants.Directions[directionIndex];

        // Other initialization of variables
        actualHeight = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        
        // Check grounded
        isJumping = !IsGrounded();

        // Check level
        if (!isJumping && actualHeight != transform.position.y) { ChangeOfLevel(); }

        // Prepare Direction
        if (rotateSense != 0 && !oneRotation)  
        {
            ChangeDirection(rotateSense);
        }


        // Prepare movement
        movingVector = PrepareStraightMove();
        movingVector += PrepareJump();

        // Set animation
        Animate();
        CheckLookingDirection();

        // Limit movement
        LimitVelocity();
        GroundStop();

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
        rotateSense = way;
    }

    private void SendPosition()
    {
        SideChangeEvent.Invoke(this.transform.position);
    }

    private void CameraBeginRotation()
    {
        blockedRotation = true;
    }

    private void CameraEndRotation()
    {
        blockedRotation = false;
        oneRotation = false;
    }

    private void LimitVelocity()
    {
        if (Mathf.Abs(rb.velocity.x) >= stats.maxHorizontalSpeed)
        {
            movingVector.x = 0;
        }
        else if (Mathf.Abs(rb.velocity.z) >= stats.maxHorizontalSpeed)
        {
            movingVector.z = 0;
        }
    }

    private void GroundStop()
    {
        if (moveHorizontal == 0 && !isJumping)
        {
            rb.velocity = direction * lookingTo * stats.speedAfterStop;
        }
    }

    private void ApplyForce(Vector3 direction)
    {
        rb.AddForce(direction);
    }

    private Vector3 PrepareStraightMove()
    {

        return direction * moveHorizontal * stats.movingForce;
    }

    private Vector3 PrepareJump()
    {
        Vector3 jumpVector = Vector3.zero;
        if (doJump && !isJumping)
        {
            jumpVector += Vector3.up * stats.jumpForce;
            isJumping = true;
            doAnimateJump = true;
        }
        doJump = false;

        return jumpVector;
    }

    private void Animate()
    {
        
        if (doAnimateJump)
        {
            _Legs.Stop();
            _Legs.clip = _jump;
            _Legs.Play();
            doAnimateJump = false;
        }
        else if ((!isJumping) && (moveHorizontal != 0))
        {
            _Legs.clip = _walk;
            _Legs.Play();
        }

    }

    private void CheckLookingDirection()
    {
        if (moveHorizontal != lookingTo)
        {
            if (moveHorizontal != 0)
            {
                RotateByAmount(180);
                lookingTo = moveHorizontal;
            }
        }
    }

    private void ChangeDirection(int way)
    {
        directionIndex = Mod(directionIndex + way, Constants.Directions.Count);
        direction = Constants.Directions[directionIndex];
        RotateByAmount(Constants.rotationAmount*way);
        rotateSense = 0;
        oneRotation = true;
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
        float raycastDistance = cc.bounds.extents.y + Constants.groundDetectionDistance;

        // Perform the Raycast
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // If the Raycast hits something below the player, return true (player is grounded)
            return true;
        }

        // If the Raycast doesn't hit anything below the player, return false (player is not grounded)
        return false;
    }

    private void ChangeOfLevel()
    {
        actualHeight = transform.position.y;
        LevelChangedEvent.Invoke(actualHeight);
    }

    private void SetPlayerDepth(Vector3 position)
    {
        transform.position = position;
    }

}
