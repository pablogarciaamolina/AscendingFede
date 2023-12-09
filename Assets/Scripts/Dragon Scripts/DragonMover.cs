using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMover : MonoBehaviour
{
    private MovementManager mvM;

    private int way = 1; // 1 Up, -1 Down
    private bool moving = false;

    // Start is called before the first frame update
    void Awake()
    {
        mvM = MovementManager.Instance;

        // Suscribe to event to change the height
        mvM.CharacterChangeOfLevel += ChangeLevel;
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

    IEnumerator TransitionToLevel(float newLevel)
    {
        // Positions to transist between
        /// initial
        float initialHeight = transform.position.y;
        /// target
        float targetHeight = newLevel;

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
    }

    private void ChangeLevel(float newHeight)
    {
        StartCoroutine(TransitionToLevel(newHeight + Constants.flyingHeightAboveCharacter));
    }

}
