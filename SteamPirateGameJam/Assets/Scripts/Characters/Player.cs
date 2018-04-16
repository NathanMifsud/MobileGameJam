using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : Character {

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

    enum WEAPON { BASIC, SPREAD }
    private WEAPON _weapon = WEAPON.BASIC;

    [Header("PickupBoosts")]
    //Boosts
    public int _speedBonus = 1;
    public int _addHealthAmount = 1;
    [Tooltip("This is percent reduction from previous firing delay, e.g. 0.9 = base firing delay * 0.9 = 0.9seconds")]
    public float _reduceFiringDelay = 0.9f;

    private int _currentSpeedBonus = 1;

    private Rigidbody _rb = null;
    // Use this for initialization
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
    }
	
	// Update is called once per frame
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

    public void OnItemPickup(Pickup.PickupType pickupType)
    {
        switch (pickupType)
        {
            case Pickup.PickupType.Shotgun:
                _weapon = WEAPON.SPREAD;
                break;

            case Pickup.PickupType.RapidFire:
                _currentFireDelay *= _reduceFiringDelay;
                break;

            case Pickup.PickupType.SpeedBoost:
                _currentSpeedBonus++;
                break;

            case Pickup.PickupType.Healthpack:
                _currentHealth += _addHealthAmount;
                break;

            default:
                break;
        }
    }

    public override void FireProjectile()
    {
        Projectile projectile = null;
        switch (_weapon)
        {
            case WEAPON.BASIC:
                projectile = GameManager._Instance.GetProjectile(_team);
                if (projectile != null)
                    projectile.transform.rotation = transform.rotation;
                break;
            case WEAPON.SPREAD:
                projectile = GameManager._Instance.GetProjectile(_team);
                if (projectile != null)
                    projectile.transform.rotation = transform.rotation * Quaternion.Euler(0,30,0);
                projectile = GameManager._Instance.GetProjectile(_team);
                if (projectile != null)
                    projectile.transform.rotation = transform.rotation;
                projectile = GameManager._Instance.GetProjectile(_team);
                if (projectile != null)
                    projectile.transform.rotation = transform.rotation * Quaternion.Euler(0, -30, 0);
                break;
            default:
                break;
        }
    }
}
