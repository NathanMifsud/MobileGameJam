using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClicks : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    [Header("USER INTERFACE PANELS")]
    public GameObject _UI_MainMenu;
    public GameObject _UI_Credits;

    private bool _ClosingApplication = false;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------

    private void FixedUpdate() {

        // Once the fade after initializing quit is complete
        if (_ClosingApplication == true && ScreenFade._Instance.IsFadeComplete() == true) {

            // Close the application
            Application.Quit();
        }
    }

    /// -------------------------------------------
    /// 
    ///     Button Click events
    /// 
    /// -------------------------------------------

    public void OnButtonClick_Play() {
        
        // Load gameplay level
        Loading._Instance.LoadLevel(1);
    }

    public void OnButtonClick_Credits() {
        
        // Transition to credits menu
        if (_UI_MainMenu != null && _UI_Credits != null) {

            _UI_Credits.SetActive(true);
            _UI_MainMenu.SetActive(false);
        }
    }

    public void OnButtonClick_GoBackCredits() {

        // Transition to main menu
        if (_UI_MainMenu != null && _UI_Credits != null) {

            _UI_MainMenu.SetActive(true);
            _UI_Credits.SetActive(false);
        }
    }

    public void OnButtonClick_QuitApplication() {

        // Fade in then quit application
        ScreenFade._Instance.StartFade(ScreenFade.FadeState.fadeIn);
        _ClosingApplication = true;
    }

    public void OnButtonClick_Resume() {

        // Unpause gameplay
        ///GameManager._Instance.SetPause(false);
    }

    public void OnButtonClick_Restart() {

        // Reload gameplay level
        Loading._Instance.LoadLevel(1);

        // Force reset timescale
        ///GameManager._Instance.SetPause(false);
        Time.timeScale = 1f;
    }

    public void OnButtonClick_Mainmenu() {

        // Transition to main menu
        if (_UI_MainMenu != null && _UI_Credits != null) {

            _UI_MainMenu.SetActive(true);
            _UI_Credits.SetActive(false);
        }
        
        // Load mainmenu level
        Loading._Instance.LoadLevel(0);

        // Force reset timescale
        ///GameManager._Instance.SetPause(false);
        Time.timeScale = 1f;
    }

}