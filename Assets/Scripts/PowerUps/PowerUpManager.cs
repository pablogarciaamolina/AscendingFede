using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : GenericSingleton<PowerUpManager>
{
    public static event Action DoubleJumpPickedUp;
    public static event Action HeavyBootsPickedUp;
    public static event Action ArmorPickedUp;
}
