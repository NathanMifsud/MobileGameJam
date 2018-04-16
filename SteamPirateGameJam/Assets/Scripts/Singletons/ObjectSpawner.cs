﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    //Setup as singleton
    [HideInInspector]
    public static ObjectSpawner _Instance;
    private void Awake()
    {

        // Destroy old singleton if it doesnt match THIS instance
        if (_Instance != null && _Instance != this)
        {

            Destroy(this.gameObject);
            return;
        }

        // Set new singleton
        _Instance = this;
    }


    [Header("Spawning Varibles")]
    public float _minEnemySpawnTime;
    public float _maxEnemySpawnTime;

    public float _minPickupSpawnTime;
    public float _maxPickupSpawnTime;

    private Vector3 _playAreaMin;
    private Vector3 _playAreaMax;

    // Use this for initialization
    void Start ()
    {
        Camera mainCamera = Camera.main;
        _playAreaMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - mainCamera.transform.position.z));
        _playAreaMax = Camera.main.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, transform.position.z - mainCamera.transform.position.z));

        Invoke("SpawnPickup", Random.Range(_minPickupSpawnTime, _maxPickupSpawnTime));
        Invoke("SpawnEnemies", Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
    }

    public void SpawnPickup()
    {
        Pickup pickup = GameManager._Instance.GetPickup();
        pickup.gameObject.SetActive(true);
        Vector3 position = pickup.transform.position;
        //Random x pos in range of screen;
        position.x = Random.Range(_playAreaMin.x, _playAreaMax.x);

        //Starting Y just above play area;
        position.y = _playAreaMax.y + 1;

        pickup.transform.position = position;

        Invoke("SpawnPickup", Random.Range(_minPickupSpawnTime, _maxPickupSpawnTime));
    }

    public void SpawnEnemies()
    {
        int randomEnemySelect = Random.Range(0,);
        List<Enemy> enemies = GameManager._Instance.GetEnemies(1);

        Invoke("SpawnEnemies", Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
    }
}
