using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : GenericSingleton<EnvironmentManager>
{
    [SerializeField] private GameObject InvisibleCube;
    [SerializeField] private Transform playableEnvironment;
    [SerializeField] private Transform unplayableEnvironment;
   
    // Visible Environment
    private List<List<GameObject>> visibleEnvSidesParents = new List<List<GameObject>>() { new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>()};
    private List<List<Vector3>> visibleEnvSides = new List<List<Vector3>>() { new List<Vector3>(), new List<Vector3>(), new List<Vector3>(), new List<Vector3>() };

    // InvisibleCubes
    private List<Transform> InvisibleList = new List<Transform>();
    private bool StandingOnInvisibleCube;

    // Player
    private Vector3 actual_pos = Constants.playerInitialPosition;

    // Camera
    private GameObject Camera;
    private CameraSwitcher cameraSwitcher;
    private int sideIndex = 0;

    // Manager(s) instances
    private InputManager ipM;

    // Events
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
        ipM.ToRotate += sideRotation;

        // Obtain Camera
        Camera = GameObject.Find("Camera");
        cameraSwitcher = Camera.GetComponent<CameraSwitcher>();
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
        sideIndex = Mod(sideIndex + way, Constants.Directions.Count);
    }
    private void sideRotation(int way)
    {
        checkSideIndex.Invoke(sideIndex);
        checkIsBlocking.Invoke(actual_pos);
        checkinviscubes.Invoke();
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
      
        if (StandingOnInvisibleCube)
        {
            Vector3 playerposition = actual_pos;
            Debug.Log(playerposition);

            if (sideIndex == 0 || sideIndex == 2)
            {
                foreach (Vector3 pos in visibleEnvSides[sideIndex])
                {
                    if (pos.x - playerposition.x <= 0.5f && pos.y - playerposition.y <= 0.5f)
                    {
                        if (Math.Abs(pos.z) <= Math.Abs(playerposition.z)) { playerposition.z = pos.z; }
                    }
                }
            }
            else if (sideIndex == 1 || sideIndex == 3)
            {
                foreach (Vector3 pos in visibleEnvSides[sideIndex])
                {
                    if (pos.z - playerposition.z <= 0.5f && pos.y - playerposition.y <= 0.5f)
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

        
        setPlayertoBlock.Invoke(playerposition);
    }
    private void createRayGrid()
    {
        for (int i = -Constants.max_width; i < Constants.max_width; i += 1)
        {
            for (int j = -10; j < Constants.max_height; j += 1)
            {
                Vector3 coord = new Vector3(i, j, -2*Constants.max_width);
                Ray ray = new Ray(coord, new Vector3(0f, 0f, 1f));
                setVisibleEnvironment(ray, 0);

                coord = new Vector3(-2*Constants.max_width, j, i);
                ray = new Ray(coord, new Vector3(1f, 0f, 0f));
                setVisibleEnvironment(ray, 3);

                coord = new Vector3(i, j, 2*Constants.max_width);
                ray = new Ray(coord, new Vector3(0f, 0f, -1f));
                setVisibleEnvironment(ray, 2);

                coord = new Vector3(2*Constants.max_width, j, i);
                ray = new Ray(coord, new Vector3(-1f, 0f, 0f));
                setVisibleEnvironment(ray, 1);
            }
        }
    }
    private void setVisibleEnvironment(Ray ray,int index)
    {
        RaycastHit hit;
        Vector3 pos;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                pos = hit.point;
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

        if (visibleEnvSidesParents[sideIndex].Contains(obj))
        {

            Vector3 invSize = Vector3.zero;
            Vector3 pos = Vector3.zero;

            if (sideIndex == 0 || sideIndex == 2)
            {
                invSize = new Vector3(size.x, size.y, 2);
                pos = new Vector3(pe.transform.position.x, pe.transform.position.y, depth);
            }
            else if (sideIndex == 1 || sideIndex == 3)
            {
                invSize = new Vector3(2, size.y, size.z);
                pos = new Vector3(depth, pe.transform.position.y, pe.transform.position.z);
            }

            GameObject inv = Instantiate(InvisibleCube);
            if (pe.quality == "ice")
            {
                inv.AddComponent<IceTerrain>();
            }
            else if (pe.quality == "fire")
            {
                inv.AddComponent<FireTerrain>();
            }
            else if (pe.quality == "heal")
            {
                inv.AddComponent<HealingTerrain>();
            }
            else
            {
                inv.AddComponent<BaseTerrain>();
            }
            inv.transform.localScale = invSize;
            inv.transform.position = pos;
            InvisibleList.Add(inv.transform);
            InvisibleCube sc = inv.GetComponent<InvisibleCube>();
            sc.CollidedWithPlayer += OnInvisibleCube;
            sc.ColisionExit += NotOnInvisibleCube;
        }
    }
    public void ManageSideChange(Vector3 position)
    {
        actual_pos = position;
    }
}
