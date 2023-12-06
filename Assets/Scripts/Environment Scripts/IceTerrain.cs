using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTerrain : BaseTerrain
{
    public override void ModifyStats(ref FedeStats stats)
    {
        stats.speedAfterStop = Constants.iceSpeed;
    }

}
