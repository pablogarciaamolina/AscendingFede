using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMover : MonoBehaviour
{
    // Animation elements
    private Animator _animator;

    // Movement const
    private float distanceUpDownFromCenter = 1f;
    private float UpDownTime = 2f;


    private int way = 1; // 1 Up, -1 Down
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            // _animator.Play("Vox_Dragon_Fly"); CHECK
            StartCoroutine(UpDownMovement(way));
            way *= -1;
        }
    }

    IEnumerator UpDownMovement(int way)
    {

        moving = true;

        // Positions to transist between
        /// initial
        float initialHeight = transform.position.y;
        /// target
        float targetHeight = transform.position.y + way * distanceUpDownFromCenter;

        float elapsedTime = 0f;
        while (elapsedTime < UpDownTime)
        {
            Vector3 initPosition = transform.position;
            initPosition.y = initialHeight;
            Vector3 finalPosition = transform.position;
            finalPosition.y = targetHeight;

            transform.position = Vector3.Lerp(initPosition, finalPosition, elapsedTime / UpDownTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moving = false;
    }
}
