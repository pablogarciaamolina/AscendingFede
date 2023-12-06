using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMover : MonoBehaviour
{  
   
    private int way = 1; // 1 Up, -1 Down
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
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
        float targetHeight = transform.position.y + way * Constants.distanceUpDownFromCenter;

        float elapsedTime = 0f;
        while (elapsedTime < Constants.UpDownTime)
        {
            Vector3 initPosition = transform.position;
            initPosition.y = initialHeight;
            Vector3 finalPosition = transform.position;
            finalPosition.y = targetHeight;

            transform.position = Vector3.Lerp(initPosition, finalPosition, elapsedTime / Constants.UpDownTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moving = false;
    }
}
