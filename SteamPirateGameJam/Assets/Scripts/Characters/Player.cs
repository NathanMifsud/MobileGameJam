using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : Character {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
            Debug.Log("AA");
	}
}
