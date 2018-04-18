using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//script written by Deeon

public class Player : Character {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    [Header("Movement")]
    [SerializeField]
    private float _movementSpeed = 1;

    [Header("Play Area")]
    [SerializeField]
    [Tooltip("Expand play area downwards, negitive moves it upwards")]
    private float _zOffset = 0.1f;

    [SerializeField]
    [Tooltip("Up and down play area")]
    private float _zPlayRatio = 0.5f;
    [SerializeField]
    [Tooltip("Expand play area left and right")]
    private float _xPlayRatio = 0.9f;

    private Vector3 _playAreaMin;
    private Vector3 _playAreaMax;

    private enum WEAPON { BASIC, SPREAD }
    private WEAPON _weapon = WEAPON.BASIC;

    [Header("Pickups")]
    public float _PickupDuration = 5f;
    public int _speedBonus = 1;
    public int _addHealthAmount = 1;
    private bool _HasPickupSpeedboost = false;
    private float _PickupTimerSpeedboost = 0f;
    [HideInInspector]
    public bool _HasPickupRapidFire = false;
    private float _PickupTimerRapidFire = 0f;
    private bool _HasPickupSpread = false;
    private float _PickupTimerSpread = 0f;

    [Header("THIS HAS A TOOLTIP - READ IT!")]
    [Tooltip("This is percent reduction from previous firing delay, e.g. 0.9 = base firing delay * 0.9 = 0.9seconds")]
    public float _reduceFiringDelay = 0.9f;

    private int _currentSpeedBonus = 1;

    private Rigidbody _rb = null;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Startup
    /// 
    /// -------------------------------------------

    protected override void Start()
    {

        _team = TEAM.PLAYER;

        Camera mainCamera = Camera.main;
        _playAreaMax = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.y - mainCamera.transform.position.y));
        _playAreaMin = Camera.main.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, transform.position.y - mainCamera.transform.position.y));

        //Move to bottom of screen
        float zOffset = (_playAreaMax.z - _playAreaMin.z) * -_zOffset;

        _playAreaMin.x *= _xPlayRatio;
        _playAreaMin.z *= _zPlayRatio;
        _playAreaMin.z += zOffset;

        _playAreaMax.x *= _xPlayRatio;
        _playAreaMax.z *= _zPlayRatio;
        _playAreaMax.z += zOffset;

        _rb = GetComponent<Rigidbody>();

        //-------------------------------
        /* Update active pickup timers */
        //-------------------------------

        // Speed boost
        if (_HasPickupSpeedboost) {

            _PickupTimerSpeedboost += Time.deltaTime;

            // Speed boost pickup has reached duration threshold
            if (_PickupTimerSpeedboost >= _PickupDuration) {

                _HasPickupSpeedboost = false;
            }
        }

        // Speed boost has ended / not active
        else { _currentSpeedBonus = 0; }

        // Rapid fire timer
        if (_HasPickupRapidFire)
        {

            _PickupTimerRapidFire += Time.deltaTime;

            // Speed boost pickup has reached duration threshold
            if (_PickupTimerRapidFire >= _PickupDuration)
            {

                _HasPickupRapidFire = false;
            }
        }

        // Rapid fire has ended / not active
        else { _currentFireDelay = _baseFireDelay; }

        // Spread fire timer
        if (_HasPickupSpread) {

            _PickupTimerSpread += Time.deltaTime;

            // Speed boost pickup has reached duration threshold
            if (_PickupTimerSpread >= _PickupDuration) {

                _HasPickupSpread = false;
            }
        }

        // Spread fire has ended / not active
        else { _weapon = WEAPON.BASIC; }

    }

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------
    protected override void Update()
    {
        base.Update();
    }

    void FixedUpdate ()
    {
        Vector3 position = transform.position;

        float speed = (_movementSpeed + _currentSpeedBonus) * Time.deltaTime;

        position.x += CrossPlatformInputManager.GetAxisRaw("Horizontal") * speed;
        position.z += CrossPlatformInputManager.GetAxisRaw("Vertical") * speed;

        if (position.x < _playAreaMin.x)
            position.x = _playAreaMin.x;

        if (position.x > _playAreaMax.x)
            position.x = _playAreaMax.x;

        if (position.z < _playAreaMin.z)
            position.z = _playAreaMin.z;

        if (position.z > _playAreaMax.z)
            position.z = _playAreaMax.z;

        transform.position = position;
    }

    /// -------------------------------------------
    /// 
    ///     Pickups
    /// 
    /// -------------------------------------------

    public void OnItemPickup(Pickup.PickupType pickupType)
    {
        switch (pickupType)
        {
            // Spread fire
            case Pickup.PickupType.Spread:
                _weapon = WEAPON.SPREAD;
                _HasPickupSpread = true;
                break;

            // Rapid fire
            case Pickup.PickupType.RapidFire:
                _currentFireDelay *= _reduceFiringDelay;
                _HasPickupRapidFire = true;
                break;

            // Speed boost
            case Pickup.PickupType.SpeedBoost:
                _currentSpeedBonus += _speedBonus;
                _HasPickupSpeedboost = true;
                break;
            
            // Health pack
            case Pickup.PickupType.Healthpack:
                _currentHealth += _addHealthAmount;
                break;

            default: break;
        }
    }

    /// -------------------------------------------
    /// 
    ///     Firing
    /// 
    /// -------------------------------------------

    public override void FireProjectile()
    {
        switch (_weapon)
        {
            // Default firemode
            case WEAPON.BASIC:

                SpawnBullet(_MuzzleLaunchPoint.position, _MuzzleLaunchPoint.rotation, 0, this);
                break;
            // Spread firemode
            case WEAPON.SPREAD:

                // Projectile 1
                SpawnBullet(_MuzzleLaunchPoint.position, _MuzzleLaunchPoint.rotation, 30, this);

                // Projectile 2
                SpawnBullet(_MuzzleLaunchPoint.position, _MuzzleLaunchPoint.rotation, 0, this);

                // Projectile 3
                SpawnBullet(_MuzzleLaunchPoint.position, _MuzzleLaunchPoint.rotation, -30, this);
                break;

            default: break;
        }
    }
}