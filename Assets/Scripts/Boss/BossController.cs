using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossController : NetworkBehaviour
{
    [SerializeField] private LaserManager _laser;
    [SerializeField] private DashManager _dash;

    //Not implemented
    public void LaunchLaser()
    {
        _laser.StartAbility();
    }

    public void LaunchDash()
    {
        _dash.StartAbility();
    }
}
