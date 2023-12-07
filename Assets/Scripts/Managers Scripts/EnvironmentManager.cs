using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    private List<List<Vector3>> visibleEnvSides = new List<List<Vector3>>() { new List<Vector3>(), new List<Vector3>(), new List<Vector3>(), new List<Vector3>() };

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

        //ipM.ToRotate += sideRotation;
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
                foreach (Vector3 pos in visibleEnvSides[sideIndex])
                {
                    if (pos.x - playerposition.x <= 0.9f && pos.y - playerposition.y <= 1)
                    {
                        playerposition.z = pos.z;
                    }
                }
            }
            else if (sideIndex == 1 || sideIndex == 3)
            {
                foreach (Vector3 pos in visibleEnvSides[sideIndex])
                {
                    if (pos.z - playerposition.z <= 0.9f && pos.y - playerposition.y <= 1)
                    {
                        playerposition.x = pos.x;
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
            foreach (Vector3 pos in visibleEnvSides[sideIndex])
            {
                if (playerposition.z - pos.z >= 0.9f)
                {
                    playerposition.z = pos.z;
                }
            }
        }
        else if (sideIndex == 1)
        {
            foreach (Vector3 pos in visibleEnvSides[sideIndex])
            {
                if (pos.x - playerposition.x >= 0.9f)
                {
                    playerposition.x = pos.x;
                }
            }
        }
        else if (sideIndex == 2)
        {
            foreach (Vector3 pos in visibleEnvSides[sideIndex])
            {
                if (pos.z - playerposition.z >= 0.9f)
                {
                    playerposition.z = pos.z;
                }
            }
        }
        else if (sideIndex == 3)
        {
            foreach (Vector3 pos in visibleEnvSides[sideIndex])
            {
                if (playerposition.x - pos.x >= 0.9f)
                {
                    playerposition.x = pos.x;
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
        foreach (Vector3 posible_pos in visibleEnvSides[sideIndex])
        {
            if (Mathf.Abs(posible_pos.x - pos.x) <= 0.75 && Mathf.Abs(posible_pos.y - pos.y) <= 0.75 && Mathf.Abs(posible_pos.z - pos.z) <= 0.75)
            {
                return true;
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
                setVisibleEnvironment(ray, 0);

                coord = new Vector3(-2*max_width, j, i);
                ray = new Ray(coord, new Vector3(1f, 0f, 0f));
                setVisibleEnvironment(ray, 3);

                coord = new Vector3(i, j, 2*max_width);
                ray = new Ray(coord, new Vector3(0f, 0f, -1f));
                setVisibleEnvironment(ray, 2);

                coord = new Vector3(2*max_width, j, i);
                ray = new Ray(coord, new Vector3(-1f, 0f, 0f));
                setVisibleEnvironment(ray, 1);
            }
        }
    }
    private void setVisibleEnvironment(Ray ray,int index)
    {
        // añadir la posicion en vez del hijo
        RaycastHit hit;
        Vector3 pos;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                pos = hit.point;
                // revisar que funcione bien 
                if (!visibleEnvSides[index].Contains(pos))
                {
                    checkplayable(pos, index, hit.transform.gameObject);
                }
            }
        }
    }
    private void checkplayable(Vector3 pos,int index, GameObject go)
    {
        bool inlist = false;

        foreach (Transform tp in playableEnvironment)
        {
            if (tp.gameObject == go)
            {
                inlist = true;
                if (!visibleEnvSides[index].Contains(pos))
                {
                    visibleEnvSides[index].Add(pos);
                }
            }
        }
        if (inlist)
        {
            if (!visibleEnvSidesParents[index].Contains(go))
            {
                visibleEnvSidesParents[index].Add(go);
            }
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

        Vector3 size = pe.GetComponent<Collider>().bounds.size;
        Vector3 max_pos = pe.transform.position + size / 2;
        Vector3 min_pos = pe.transform.position - size / 2;

        foreach (Vector3 p in visibleEnvSides[sideIndex])
        {
            Vector3 pos = new Vector3(0, 0, 0);

            if (sideIndex == 0 || sideIndex == 2)
            {
                if (min_pos.x <= p.x && min_pos.y <= p.y && max_pos.x >= p.x && max_pos.y >= p.y)
                {
                    pos = new Vector3(p.x, p.y, depth);
                }
            }
            else if (sideIndex == 1 || sideIndex == 3)
            {
                if (min_pos.z <= p.z && min_pos.y <= p.y && max_pos.z >= p.z && max_pos.y >= p.y)
                {
                    pos = new Vector3(depth, p.y, p.z);
                }
            }

            if (!checkforblock(pos) && pos != Vector3.zero)
            {
                CreateInvisibleCube(pos);
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
