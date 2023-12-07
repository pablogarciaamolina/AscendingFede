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
        // quitar break una vez rehecho !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        isBlocking = false;
        foreach (Transform t in this.gameObject.transform)
        {
            Vector3 size = t.GetComponent<Collider>().bounds.size;
            Vector3 max_pos = t.position + size / 2;
            Vector3 min_pos = t.position - size / 2;

            if ( sideIndex == 0 || sideIndex == 2)
            {
                if (min_pos.x <= playerpos.x && min_pos.y <= playerpos.y && max_pos.x >= playerpos.x && max_pos.y >= playerpos.y)
                {
                    isBlocking = true;
                    break;
                }
            }
            else if (sideIndex == 1|| sideIndex == 3) 
            {
                if (min_pos.z <= playerpos.z && min_pos.y <= playerpos.y && max_pos.z >= playerpos.z && max_pos.y >= playerpos.y)
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

