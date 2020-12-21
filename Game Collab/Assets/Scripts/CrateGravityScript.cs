using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateGravityScript : MonoBehaviour
{
    private playerBehaviour MonitorPlayer;

    private void Start()
    {
        MonitorPlayer = FindObjectOfType<playerBehaviour>();

        MonitorPlayer.OnGravityEffect += MonitorPlayer_OnGravityEffect; //Subscribe to main EventHandler
    }

    private void MonitorPlayer_OnGravityEffect(object sender, System.EventArgs e)
    {
        SwitchingGravityEffect.instance.Switch(GetComponent<Rigidbody2D>(), false);
    }
}
