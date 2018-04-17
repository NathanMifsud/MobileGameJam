using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public enum FadeState { idle, fadeIn, fadeOut }
    
    public static ScreenFade _Instance;

    public GameObject _UI_Panel;
    public float _FadeSpeed = 0.1f;
    public Color _FadeColour;

    private FadeState _FadeState;
    private Image _ImageComponent;
    private bool _IsFading = false;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Startup
    /// 
    /// -------------------------------------------

    private void Awake() {

        // Destroy old singleton if it doesnt match THIS instance
        if (_Instance != null && _Instance != this) {

            Destroy(this.gameObject);
            return;
        }

        // Set new singleton
        _Instance = this;
    }

    private void Start() {

        // Get component references
        _ImageComponent = GetComponent<Image>();
    }

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------

    private void FixedUpdate () {
		
        if (_UI_Panel != null) {

            switch (_FadeState) {

                // Idle / inactive
                case FadeState.idle:

                    // Hide panel
                    _UI_Panel.SetActive(false);
                    _IsFading = false;
                    break;

                // Fade in
                case FadeState.fadeIn:

                    // Show panel
                    _UI_Panel.SetActive(true);

                    // Fade screen into COLOUR
                    if (_ImageComponent.color.a < 1f) {

                        _ImageComponent.color = new Color(_ImageComponent.color.r, _ImageComponent.color.g, _ImageComponent.color.b, _ImageComponent.color.a + _FadeSpeed);
                        _IsFading = true;
                    }

                    // Fade sequence completed
                    else { _FadeState = FadeState.idle; }
                    break;

                // Fade out
                case FadeState.fadeOut:

                    // Show panel
                    _UI_Panel.SetActive(true);

                    // Fade screen from COLOUR
                    if (_ImageComponent.color.a > 0f) {

                        _ImageComponent.color = new Color(_ImageComponent.color.r, _ImageComponent.color.g, _ImageComponent.color.b, _ImageComponent.color.a + _FadeSpeed);
                        _IsFading = true;
                    }

                    // Fade sequence completed
                    else { _FadeState = FadeState.idle; }
                    break;

                default: break;
            }
        }
	}

    /// -------------------------------------------
    /// 
    ///     Fade
    /// 
    /// -------------------------------------------

    public void StartFade(FadeState state) {

        // Initialize
        _ImageComponent.color = _FadeColour;
        _FadeState = state;

        switch (state) {

            case FadeState.idle: break;

            case FadeState.fadeIn: {

                _UI_Panel.SetActive(true);
                _IsFading = true;
                _ImageComponent.color = new Color(_ImageComponent.color.r, _ImageComponent.color.g, _ImageComponent.color.b, 0f);
                break;
            }

            case FadeState.fadeOut: {

                _UI_Panel.SetActive(true);
                _IsFading = true;
                _ImageComponent.color = new Color(_ImageComponent.color.r, _ImageComponent.color.g, _ImageComponent.color.b, 1f);
                break;
            }

            default: break;
        }

    }

    public bool IsFadeComplete() { return !_IsFading; }

}