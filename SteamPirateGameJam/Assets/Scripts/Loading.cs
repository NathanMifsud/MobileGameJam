using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public static Loading _Instance;

    public AsyncOperation _aSync { get; private set; }

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Startup
    /// 
    /// -------------------------------------------

    private void Awake () {

        // Destroy old singleton if it doesnt match THIS instance
        if (_Instance != null && _Instance != this) {

            Destroy(this.gameObject);
            return;
        }

        // Set new singleton
        _Instance = this;
    }

    /// -------------------------------------------
    /// 
    ///     Loading
    /// 
    /// -------------------------------------------
    
    public void LoadLevel(int index) {

        _aSync = SceneManager.LoadSceneAsync(index);
    }

}