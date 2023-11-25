using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableEnvironment : MonoBehaviour
{
    // Start is called before the first frame update
    private EnvironmentManager eM;
    private bool isBlocking = false;
    private int sideIndex = 0;

    public Action<PlayableEnvironment> scanBlocking;

    private void Awake()
    {
        eM = EnvironmentManager.Instance;

        eM.checkIsBlocking += setIsBlocking;
        eM.checkSideIndex += setSideIndex;
        eM.checkinviscubes += setInvisiCubes; 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void setIsBlocking(Vector3 playerpos)
    {
        isBlocking = false;
        foreach (Transform t in this.gameObject.transform)
        {
            if ( sideIndex == 0 || sideIndex == 2)
            {
                if (Mathf.Abs(t.position.x - playerpos.x) <= 1 && Mathf.Abs(t.position.y - playerpos.y )<= 1)
                {
                    isBlocking = true;
                    break;
                }
            }
            else if (sideIndex == 1|| sideIndex == 3) 
            {
                if (Mathf.Abs(t.position.y - playerpos.y) <= 1 && Mathf.Abs(t.position.z - playerpos.z)<= 1)
                {
                    isBlocking = true;
                    break;
                }
            }
        }
    }

    private void setSideIndex(int index)
    {
        sideIndex = index;  
    }

    private void setInvisiCubes()
    {
        if (!isBlocking)
        {
            scanBlocking.Invoke(this);
        }
    }
}

