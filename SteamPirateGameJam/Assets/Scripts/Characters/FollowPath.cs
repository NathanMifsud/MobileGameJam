using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public List<Transform> _TargetPoints;
    public float _TargetThreshold = 5f;
    public float _MaxRotationDelta = 1f;
    public float _MaxMagnitudeDelta = 0f;

    private Enemy _Agent;
    private Transform _TargetTransform;

    private int _TargetsLength;
    private int _ArrayPosition = 0;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Startup
    /// 
    /// -------------------------------------------

    private void Start() {

        // Get component references
        _Agent = GetComponent<Enemy>();

        _TargetsLength = _TargetPoints.Count;

        // Initial target point is the first in the transform array
        if (_TargetPoints.Count > 0) {

            _ArrayPosition = 0;
            _TargetTransform = _TargetPoints[_ArrayPosition];
        }
        else {

            _TargetTransform.position += Vector3.down;
        }
    }

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------

    private void Update () {

        if (_TargetPoints.Count == 0) {

            // Continuously move the target position down so that the agent follows the updated path
            _TargetTransform.position += Vector3.down * Time.deltaTime;
        }

        else  {

            // Agent & target are within the threshold (has reached target)
            if (Vector3.Distance(_Agent.transform.position, _TargetTransform.position) < _TargetThreshold) {

                // Not the end of the array yet
                if (_ArrayPosition < _TargetsLength) {

                    // Set new go to target
                    _ArrayPosition += 1;
                    _TargetTransform = _TargetPoints[_ArrayPosition];
                }

                // Has reached the end of the transform array
                else {

                    // Kill agent for pending recycle
                    _Agent.OnDeath();
                }
            }

            // Hasnt reached target yet
            else {

                // Look at current target's position
                ///_Agent.transform.LookAt(_TargetTransform);

                Vector3 dir = _TargetTransform.position - transform.position;
                // Rotate towards target position
                _Agent.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, dir, _MaxRotationDelta * Time.deltaTime, _MaxMagnitudeDelta));

                // Move towards last known facing direction
                float speed = _Agent._MovementSpeed * Time.deltaTime;
                _Agent.transform.Translate(Vector3.forward * speed);
            }
        }
	}

}