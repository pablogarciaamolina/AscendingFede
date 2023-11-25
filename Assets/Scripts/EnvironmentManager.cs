using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class EnvironmentManager : GenericSingleton<EnvironmentManager>
{
    [SerializeField] private GameObject InvisibleCube;
    [SerializeField] private Transform playableEnvironment; // places where you can step ( idealy a block ) 
    [SerializeField] private Transform unplayableEnvironment; // tower

    private bool StandingOnInvisibleCube;
    private List<Transform> InvisibleList = new List<Transform>();

    private int sideIndex = 0;
    private List<Vector3> Directions = new List<Vector3>() { new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, -1f) };
  
    private List<List<GameObject>> visibleEnvSidesParents = new List<List<GameObject>>() { new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>()};
    private List<List<GameObject>> visibleEnvSides = new List<List<GameObject>>() { new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };

    private int max_height = 110;
    private int max_width = 40;

    // create an event for changing VisualLevel when the camera chnges side and height (either) // when changing side DONE 

    private InputManager ipM;

    private Vector3 actual_pos = Constants.playerInitialPosition;

    // Camera
    private GameObject Camera;
    private CameraSwitcher cameraSwitcher;

    public Action<Vector3> checkIsBlocking;
    public Action<int> checkSideIndex;
    public Action checkinviscubes;
    public Action<Vector3> setPlayertoBlock;
    public Action SendPositionEvent;

    public override void Awake()
    {
        base.Awake();

        StandingOnInvisibleCube = false;
        ipM = InputManager.Instance; 

        // Obtain Camera
        Camera = GameObject.Find("Camera");
        cameraSwitcher = Camera.GetComponent<CameraSwitcher>();

        ipM.ToRotate += sideRotation;
        cameraSwitcher.EndRotation += CameraDoneRotating;

        foreach (Transform t in playableEnvironment.transform)
        {
            PlayableEnvironment pe = t.gameObject.GetComponent<PlayableEnvironment>();
            pe.scanBlocking += BuildInvisibleCubes;
        }

        createRayGrid();     
    }
    void Start()
    {
        
    }
    public void Update()
    {
        checkSideIndex.Invoke(sideIndex);
        checkIsBlocking.Invoke(actual_pos);
        checkinviscubes.Invoke();
    }
    private void OnInvisibleCube()
    {
        StandingOnInvisibleCube = true;
    }
    private void NotOnInvisibleCube()
    {
        StandingOnInvisibleCube = StandingOnInvisibleCube || false ;
    }
    private int Mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }
    private void ChangeDirection(int way)
    {
        sideIndex = Mod(sideIndex + way, Directions.Count);
    }
    private void sideRotation(int way)
    {
        SendPositionEvent.Invoke();
        ChangePlayertoPlatform();
        ChangeDirection(way);
    }
    private void CameraDoneRotating()
    {
        DeleteInvisibleEnvironment();
        checkSideIndex.Invoke(sideIndex);
        SendPositionEvent.Invoke();
        MovePlayerForward();
    }
    private void DeleteInvisibleEnvironment() 
    {
        foreach (Transform t in InvisibleList)
        {
            InvisibleCube sc = t.gameObject.GetComponent<InvisibleCube>();
            sc.CollidedWithPlayer -= OnInvisibleCube;
            sc.ColisionExit -= NotOnInvisibleCube;
            Destroy(sc.gameObject);
        }

        InvisibleList.Clear();
    }
    private void ChangePlayertoPlatform()
    {
        //when rotating change the player depth to the platform he is in instead of the invisible cube
        if (StandingOnInvisibleCube)
        {
            Vector3 playerposition = actual_pos; // get player position

            if (sideIndex == 0 || sideIndex == 2)
            {
                foreach (GameObject go in visibleEnvSides[sideIndex])
                {
                    if (go.transform.position.x - playerposition.x <= 0.9f && go.transform.position.y - playerposition.y <= 1)
                    {
                        playerposition.z = go.transform.position.z;
                    }
                }
            }
            else if (sideIndex == 1 || sideIndex == 3)
            {
                foreach (GameObject go in visibleEnvSides[sideIndex])
                {
                    if (go.transform.position.z - playerposition.z <= 0.9f && go.transform.position.y - playerposition.y <= 1)
                    {
                        playerposition.x = go.transform.position.x;
                    }
                }
            }
            // move player to that position playerposition
            actual_pos = playerposition;
            setPlayertoBlock.Invoke(playerposition);
        }
    }
    private void MovePlayerForward()
    {
        Vector3 playerposition = actual_pos; // get player position

        if (sideIndex == 0)
        {
            foreach (GameObject go in visibleEnvSides[sideIndex])
            {
                if (playerposition.z - go.transform.position.z >= 0.9f)
                {
                    playerposition.z = go.transform.position.z;
                }
            }
        }
        else if (sideIndex == 1)
        {
            foreach (GameObject go in visibleEnvSides[sideIndex])
            {
                if (go.transform.position.x - playerposition.x >= 0.9f)
                {
                    playerposition.x = go.transform.position.x;
                }
            }
        }
        else if (sideIndex == 2)
        {
            foreach (GameObject go in visibleEnvSides[sideIndex])
            {
                if (go.transform.position.z - playerposition.z >= 0.9f)
                {
                    playerposition.z = go.transform.position.z;
                }
            }
        }
        else if (sideIndex == 3)
        {
            foreach (GameObject go in visibleEnvSides[sideIndex])
            {
                if (playerposition.x - go.transform.position.x >= 0.9f)
                {
                    playerposition.x = go.transform.position.x;
                }
            }
        }
        checkIsBlocking.Invoke(playerposition);
        actual_pos = playerposition;
        checkinviscubes.Invoke();

        // move player to that position playerposition
        setPlayertoBlock.Invoke(playerposition);
    }
    private bool checkforblock(Vector3 pos)
    {
        foreach (Transform tp in playableEnvironment)
        {
            foreach ( Transform t in tp)
            {
                if (Mathf.Abs(t.position.x - pos.x) <= 0.75 && Mathf.Abs(t.position.y - pos.y) <= 0.75 && Mathf.Abs(t.position.z - pos.z) <= 0.75)
                {
                    return true;
                }
            }
            
        }
        return false;
    }
    private void createRayGrid()
    { 
        for (int i = -max_width; i < max_width; i += 1)
        {
            for (int j = -10; j < max_height; j += 1)
            {
                Vector3 coord = new Vector3(i, j, -2*max_width);
                Ray ray = new Ray(coord, new Vector3(0f, 0f, 1f));
                setVisibleEnvironment(ray,0);

                coord = new Vector3(-2*max_width, j, i);
                ray = new Ray(coord, new Vector3(1f, 0f, 0f));
                setVisibleEnvironment(ray,3);

                coord = new Vector3(i, j, 2*max_width);
                ray = new Ray(coord, new Vector3(0f, 0f, -1f));
                setVisibleEnvironment(ray,2);

                coord = new Vector3(2*max_width, j, i);
                ray = new Ray(coord, new Vector3(-1f, 0f, 0f));
                setVisibleEnvironment(ray,1);
            }
        }
    }
    private void setVisibleEnvironment(Ray ray,int index)
    {
        RaycastHit hit;
        GameObject obj;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                obj = hit.transform.gameObject;
                if (!visibleEnvSides[index].Contains(obj))
                {
                    checkplayable(obj,index);
                }
            }
        }
    }
    private void checkplayable(GameObject obj,int index)
    {
        bool inlist = false;
        GameObject parentobj = new GameObject();

        foreach (Transform tp in playableEnvironment)
        {
            foreach (Transform t in tp)
            {
                if (t.gameObject == obj)
                {
                    inlist = true;
                    Destroy(parentobj);
                    parentobj = tp.gameObject;
                    if (!visibleEnvSides[index].Contains(obj))
                    {
                        visibleEnvSides[index].Add(obj);
                    }
                }
            }
        }
        if (inlist)
        {
            if (!visibleEnvSidesParents[index].Contains(parentobj))
            {
                visibleEnvSidesParents[index].Add(parentobj);
            }
        }
        else
        {
            Destroy(parentobj);
        }
        
    }
    private void BuildInvisibleCubes(PlayableEnvironment pe)
    {
        GameObject obj = pe.gameObject;
        float depth = 0;

        if (sideIndex == 0 || sideIndex == 2)
        {
            depth = actual_pos.z;
        }
        else if (sideIndex == 1 || sideIndex == 3)
        {
            depth = actual_pos.x;
        }

        foreach (Transform t in obj.transform)
        {
            if (visibleEnvSides[sideIndex].Contains(t.gameObject))
            {
                GameObject block = t.gameObject;
                Vector3 pos = new Vector3(0, 0, 0);

                if (sideIndex == 0 || sideIndex == 2)
                {
                    pos = new Vector3(block.transform.position.x, block.transform.position.y, depth);
                }
                else if (sideIndex == 1 || sideIndex == 3)
                {
                    pos = new Vector3(depth, block.transform.position.y, block.transform.position.z);
                }

                if (!checkforblock(pos))
                {
                    CreateInvisibleCube(pos);
                }
            }
        }
    }
    public void CreateInvisibleCube(Vector3 pos)
    {

        GameObject inv = Instantiate(InvisibleCube);
        inv.transform.position = pos;
        InvisibleList.Add(inv.transform);
        InvisibleCube sc = inv.GetComponent<InvisibleCube>();
        sc.CollidedWithPlayer += OnInvisibleCube;
        sc.ColisionExit += NotOnInvisibleCube;
    }
    public void ManageSideChange(Vector3 position)
    {
        actual_pos = position;
    }
}
