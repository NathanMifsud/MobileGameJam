using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : Character {

    [SerializeField]
    private float _movementSpeed = 1;

    [SerializeField]
    private float _yOffset = 0.1f;

    [SerializeField]
    private float _yPlayRatio = 0.5f;
    [SerializeField]
    private float _xPlayRatio = 0.9f;

    private Vector3 _playAreaMin;
    private Vector3 _playAreaMax;

    private Rigidbody _rb = null;
    // Use this for initialization
    void Start ()
    {
        Camera mainCamera = Camera.main;
        _playAreaMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - mainCamera.transform.position.z));
        _playAreaMax = Camera.main.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, transform.position.z - mainCamera.transform.position.z));

        Debug.Log(_playAreaMin);
        Debug.Log(_playAreaMax);

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
                break;

            case Pickup.PickupType.RapidFire:
                break;

            case Pickup.PickupType.SpeedBoost:
                break;

            case Pickup.PickupType.Healthpack:
                break;

            default:
                break;
        }
    }
}
