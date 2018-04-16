using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public enum PickupType { Shotgun, RapidFire, SpeedBoost, Healthpack }

    [Header("Properties")]
    public PickupType _PickupType;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    private void Start () {
		
	}
	
	private void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        
        // Only do stuff if its a player
        if (other.gameObject.CompareTag("Player")) {

            // On pickup event
            Player plyr = other.gameObject.GetComponent<Player>();
            plyr.OnItemPickup(_PickupType);
        }

    }
}