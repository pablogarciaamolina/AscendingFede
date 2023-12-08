using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStageManager : MonoBehaviour
{
    private MovementManager mvM;
    private DragonStats stats;

    private int currentStage = -1;

    private void Awake()
    {
        mvM = MovementManager.Instance;
        stats = gameObject.GetComponent<DragonStats>();

        // Suscribe to event of character's level change
        mvM.CharacterChangeOfLevel += CheckStageChange;
    }


    private void CheckStageChange(float newHeight)
    {
        if (currentStage + 1 < Constants.NumStages && newHeight+Constants.flyingHeightAboveCharacter >= Constants.Stages[currentStage + 1])
        {
            currentStage += 1;
            stats.fireballRate = (stats.fireballRate / Constants.rateOfFireballsChange);
            stats.numFireballs = stats.numFireballs + Constants.numberOfFireballsChange;
        }
    }
}
