using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTerrain : BaseTerrain 
{
    public override void ModifyStats(ref FedeStats stats)
    {
        stats.health -= Constants.fireDamage;
    }
}
