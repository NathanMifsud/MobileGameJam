using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : Character {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    [Header("Movement")]
    [SerializeField]
    private float _movementSpeed = 1;

    [Header("Play Area")]
    [SerializeField]
    private float _yOffset = 0.1f;

    [SerializeField]
    private float _yPlayRatio = 0.5f;
    [SerializeField]
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
        base.Update();

        _team = TEAM.PLAYER;

        Camera mainCamera = Camera.main;
        _playAreaMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - mainCamera.transform.position.z));
        _playAreaMax = Camera.main.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, transform.position.z - mainCamera.transform.position.z));

        //Move to bottom of screen
        float yOffset = (_playAreaMax.y - _playAreaMin.y) * (1 - _yPlayRatio) *0.5f;

        _playAreaMin.x *= _xPlayRatio;
        _playAreaMin.y *= _yPlayRatio;
        _playAreaMin.y += _yOffset - yOffset;

        _playAreaMax.x *= _xPlayRatio;
        _playAreaMax.y *= _yPlayRatio;
        _playAreaMax.y += _yOffset - yOffset;

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

    void FixedUpdate ()
    {
        Vector3 position = transform.position;

        float speed = _movementSpeed + _currentSpeedBonus;

        position.x += CrossPlatformInputManager.GetAxisRaw("Horizontal");
        position.y += CrossPlatformInputManager.GetAxisRaw("Vertical");

        if (position.x < _playAreaMin.x)
            position.x = _playAreaMin.x;

        if (position.x > _playAreaMax.x)
            position.x = _playAreaMax.x;

        if (position.y < _playAreaMin.y)
            position.y = _playAreaMin.y;

        if (position.y > _playAreaMax.y)
            position.y = _playAreaMax.y;

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
        Projectile projectile = null;
        switch (_weapon)
        {
            // Default firemode
            case WEAPON.BASIC:

                SpawnBullet(transform.position, transform.rotation, 0, this);

            // Spread firemode
            case WEAPON.SPREAD:

                // Projectile 1
                SpawnBullet(transform.position, transform.rotation, 30, this);

                // Projectile 2
                SpawnBullet(transform.position, transform.rotation, 0, this);

                // Projectile 3
                SpawnBullet(transform.position, transform.rotation, -30, this);

                // Play sound
                SoundManager._Instance.PlayPickupSpread(0.9f, 1.1f, this);
                break;

            default: break;
        }
    }

}