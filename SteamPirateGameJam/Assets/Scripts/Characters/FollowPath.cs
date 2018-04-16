using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public List<Transform> _TargetPoints;
    public float _TargetThreshold = 5f;

    private Enemy _EnemyController;
    private Transform _TargetTransform;

    private int _TargetsLength;
    private int _ArrayPosition = 0;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    private void Start () {

        // Get component references
        _EnemyController = GetComponent<Enemy>();

        _TargetsLength = _TargetPoints.Count;

        // Initial target point is the first in the transform array
        _ArrayPosition = 0;
        _TargetTransform = _TargetPoints[_ArrayPosition];
    }

    private void Update () {

        // Agent & target are within the threshold (has reached target)
        if (Vector3.Distance(_EnemyController.transform.position, _TargetTransform.position) < _TargetThreshold) {

            // Not the end of the array yet
            if (_ArrayPosition < _TargetsLength) {

                // Set new go to target
                _ArrayPosition += 1;
                _TargetTransform = _TargetPoints[_ArrayPosition];
            }

            // Has reached the end of the transform array
            else {

                // Kill agent for pending recycle
                
            }
        }
        
        // Hasnt reached target yet
        else {

            // Look at current target's position
            _EnemyController.transform.LookAt(_TargetTransform);

            // Move towards last known facing direction
            float speed = _EnemyController._MovementSpeed * Time.deltaTime;
            _EnemyController.transform.Translate(Vector3.forward * speed);
        }
	}

}