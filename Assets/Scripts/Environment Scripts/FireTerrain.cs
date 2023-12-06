using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTerrain : BaseTerrain 
{
    public override void ModifyHealth(FedeHealth fedeHealth)
    {
        fedeHealth.StartBurning();
    }

}
