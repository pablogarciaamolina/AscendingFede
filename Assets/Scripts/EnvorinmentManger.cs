using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnvorinmentManger : GenericSingleton<EnvorinmentManger>
{
    [SerializeField] private GameObject InvisibleCube;
    [SerializeField] private Transform playableEnvironment; // places where you can step ( idealy a block ) 
    [SerializeField] private Transform unplayableEnvironment; // tower
    [SerializeField] private Camera cam;

    private bool StandingOnInvisibleCube;

    private List<Transform> InvisibleList = new List<Transform>();

    private int sideIndex = 0;
    private List<Vector3> Directions = new List<Vector3>() { new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, -1f) };
  
    private List<GameObject> visiblePlayableEnvironment = new List<GameObject>();


    // create an event for changing VisualLevel when the camera chnges side and height (either) // when changing side DONE 


    // private InputManager ipM; // quitar
    // private MovementManager mM;
   
    public override void Awake()
    {
        base.Awake();
        StandingOnInvisibleCube = false;
        //ipM = InputManager.Instance; // quitar
        //mM = MovementManager.Instance;

        //ipM.ToRotate += sideRotation; // quitar
     

        ChangeVisibleEnvironment();


    }
    void Start()
    {
        
    }
    // Update is called once per frame
    public void Update()
    {
        ChangeVisibleEnvironment();
    }


    private void OnInvisibleCube()
    {
        StandingOnInvisibleCube = true;
    }
    private void NotOnInvisibleCube()
    {
        StandingOnInvisibleCube = false;
    }

    private int Mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }
    private void ChangeDirection(int way)
    {
        sideIndex = Mod(sideIndex + way, Directions.Count);
    }
    private void sideRotation(int way)
    {
        ChangePlayertoPlatform();
        ChangeDirection(way);
        ChangeVisibleEnvironment();
        
    }
    private void ChangeVisibleEnvironment() // when sidechange and when camera move (up)
    {
        visiblePlayableEnvironment.Clear();
        foreach (Transform t in InvisibleList)
        {
            InvisibleCube sc = t.gameObject.GetComponent<InvisibleCube>();
            sc.CollidedWithPlayer -= OnInvisibleCube;
            sc.ColisionExit -= NotOnInvisibleCube;
            Destroy(sc.gameObject);
        }

        InvisibleList.Clear();
        float depth = 6f;

        // get depth

        createRayGrid(); // actualiza visibleplayableenvironment
        BuildInvisibleCubes(depth);
    }

    private void ChangePlayertoPlatform()
    {
        //when rotating change the player depth tp the platform he is in instead of the invisible cube
        if (StandingOnInvisibleCube)
        {
            Vector3 playerposition = new Vector3(0, 0, 0); // get player position
            if (sideIndex == 0 || sideIndex == 2)
            {
                foreach (GameObject go in visiblePlayableEnvironment)
                {
                    if (go.transform.position.x == playerposition.x || go.transform.position.y - playerposition.y <= 1)
                    {
                        playerposition.z = go.transform.position.z;
                    }
                }
            }
            else if (sideIndex == 1 || sideIndex == 3)
            {
                foreach (GameObject go in visiblePlayableEnvironment)
                {
                    if (go.transform.position.z == playerposition.z || go.transform.position.y - playerposition.y <= 1)
                    {
                        playerposition.x = go.transform.position.x;
                    }
                }
            }
            // move player to that position
                   
        }
    }
    private bool checkforblock(Vector3 pos)
    {
        foreach (Transform t in playableEnvironment)
        {
            if (t.position == pos)
            {
                return true;
            }
        }
        return false;
    }

    private void BuildInvisibleCubes(float depth) {
        foreach (GameObject block in visiblePlayableEnvironment)
        {
            

            Vector3 pos = new Vector3(0,0,0);

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
                GameObject inv = Instantiate(InvisibleCube);
                inv.transform.position = pos;
                InvisibleList.Add(inv.transform);
                InvisibleCube sc = inv.GetComponent<InvisibleCube>();
                sc.CollidedWithPlayer += OnInvisibleCube;
                sc.ColisionExit += NotOnInvisibleCube;
            }
        }
        
    
    }

    private void createRayGrid()
    {
        GameObject obj;

        for (int i = 0; i < Screen.width; i += 50)
        {
            for (int j = 0; j < Screen.height; j += 50)
            {
                Vector3 coord = new Vector3(i, j, 0);
                Ray ray = cam.ScreenPointToRay(coord);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    obj = hit.transform.gameObject;
                    if (!visiblePlayableEnvironment.Contains(obj))
                    {
                        bool inlist = false;
                        foreach(Transform t in playableEnvironment)
                        {
                            if (t.gameObject == obj) {
                                inlist = true;
                            }
                        }
                        if (inlist)
                        {
                            visiblePlayableEnvironment.Add(obj);
                        }
                    }
                    
                }
            }
        }
    }
    
}
