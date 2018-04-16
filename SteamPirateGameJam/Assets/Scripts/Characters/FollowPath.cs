using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public List<Transform> _TargetPoints;

    private Enemy _EnemyController;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    private void Start () {

        // Get component references
        _EnemyController = GetComponent<Enemy>();
	}

    private void Update () {
		
        
	}

}