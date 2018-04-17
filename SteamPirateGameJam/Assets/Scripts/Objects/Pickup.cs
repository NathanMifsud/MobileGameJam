using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public enum PickupType { Spread, RapidFire, SpeedBoost, Healthpack }

    [Header("Properties")]
    public PickupType _PickupType;
    public float _pickupSpeed;

    private bool _inGameArea = false;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------

    private void Update ()
    {
        //Move downwards
        Vector3 position = transform.position;
        position += _pickupSpeed * Vector3.down;
        transform.position = position;
    }

    /// -------------------------------------------
    /// 
    ///     Events
    /// 
    /// -------------------------------------------

    //Moved onto the screen, so player can see
    private void OnBecameVisible()
    {
        _inGameArea = true;
    }

    //Moved off screen
    void OnBecameInvisible()
    {
        if(_inGameArea)
            GameManager._Instance.OnPickup(this);
    }

    //Player hits pickup
    private void OnTriggerEnter(Collider other) {
        
        // Only do stuff if its a player
        if (other.gameObject.CompareTag("Player")) {

            // On pickup event
            Player plyer = other.gameObject.GetComponent<Player>();
            plyer.OnItemPickup(_PickupType);
            GameManager._Instance.OnPickup(this);

            // Play pickup sound
            switch (_PickupType) {

                case PickupType.Spread:
                    SoundManager._Instance.PlayPickupShotgun(0.9f, 1.1f);
                    break;

                case PickupType.RapidFire:
                    SoundManager._Instance.PlayPickupRapidFire(0.9f, 1.1f);
                    break;

                case PickupType.SpeedBoost:
                    SoundManager._Instance.PlayPickupSpeedBoost(0.9f, 1.1f);
                    break;

                case PickupType.Healthpack:
                    SoundManager._Instance.PlayPickupHealthpack(0.9f, 1.1f);
                    break;

                default: break;
            }
        }
    }

}