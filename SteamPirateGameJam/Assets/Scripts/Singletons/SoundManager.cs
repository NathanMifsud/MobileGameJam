using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public static SoundManager _Instance;

    // Voxel player
    private bool _IsPlayingVoxel = false;
    private float _TimeSinceLastVoxel = 0f;
    private List<AudioSource> _VoxelWaitingList;

    [Header("UI Buttons")]
    public AudioSource _SFX_ButtonClick;
    public AudioSource _SFX_ButtonHover;
    public AudioSource _SFX_ButtonGoBack;

    [Header("Music")]
    public AudioSource _MUSIC_Gameplay;
    public AudioSource _MUSIC_Mainmenu;

    [Header("Player")]
    public List<AudioSource> _SFX_EngineLoops;
    public List<AudioSource> _SFX_OnPlayerDeath;

    [Header("Projectiles")]
    public List<AudioSource> _SFX_OnEnemyProjectileImpact;
    public List<AudioSource> _SFX_OnPlayerProjectileImpact;
    public List<AudioSource> _SFX_OnFireProjectileDefault;
    public List<AudioSource> _SFX_OnFireProjectileRapidFire;
    public List<AudioSource> _SFX_OnFireProjectileSpread;

    [Header("Pickups")]
    public List<AudioSource> _SFX_OnPickupRapidFire;
    public List<AudioSource> _SFX_OnPickupSpread;
    public List<AudioSource> _SFX_OnPickupHealth;
    public List<AudioSource> _SFX_OnPickupSpeedBoost;

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

        // Initialize voxel player
        _VoxelWaitingList = new List<AudioSource>();
    }

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------

    private void Update () {

        // Voxel player update
        if (_VoxelWaitingList != null)
            UpdateVoxelPlayer();
    }

    /// -------------------------------------------
    /// 
    ///     Voxel Player
    /// 
    /// -------------------------------------------
    
    private void UpdateVoxelPlayer() {

        // If there are voxel sounds waiting to be played
        if (_VoxelWaitingList.Count > 0) {

            if (_IsPlayingVoxel == true) {

                // Find the voxel sound that is current playing
                AudioSource vox = null;
                foreach (var sound in _VoxelWaitingList) {

                    // If a sound from the voxel list is playing
                    if (sound.isPlaying == true) {

                        // Then a voxel is playing
                        vox = sound;
                    }
                    break;
                }

                _IsPlayingVoxel = vox != null;
            }

            // A vox has finished playing
            else { /// _IsPlayingVoxel == false

                // Get the last voxel that was playing (should be at the front of the list) & remove it from the queue
                _VoxelWaitingList.RemoveAt(0);

                // If there are still voxels in the queue
                if (_VoxelWaitingList.Count > 0) {

                    // Play the next vox sound in the queue
                    _VoxelWaitingList[0].Play();

                    _IsPlayingVoxel = true;
                    _TimeSinceLastVoxel = 0f;
                }
            }
        }

        // No more voxels are left in the playing queue
        else if (_VoxelWaitingList.Count == 0) {

            // Add to timer
            _TimeSinceLastVoxel += Time.deltaTime;
        }
    }

    public int RandomSoundInt(List<AudioSource> SoundList) {

        // Returns a random integer between 0 & the size of the audio source list
        int i = Random.Range(0, SoundList.Count);
        return i;
    }

    public List<AudioSource> GetVoxelWaitingList() { return _VoxelWaitingList; }

    public void StartingPlayingVoxels() { _IsPlayingVoxel = true; }

    public bool GetIsPlayingVoxel() { return _IsPlayingVoxel; }

    /// -------------------------------------------
    /// 
    ///    UI Buttons
    /// 
    /// -------------------------------------------

    public void PlayButtonClick() {

        // Play click sound
        if (_SFX_ButtonClick != null)
            _SFX_ButtonClick.Play();
    }

    public void PlayButtonHover() {

        // Play hover sound
        if (_SFX_ButtonHover != null)
            _SFX_ButtonHover.Play();
    }

    public void PlayButtonGoBack() {

        // Play go back sound
        if (_SFX_ButtonGoBack != null)
            _SFX_ButtonGoBack.Play();
    }

    /// -------------------------------------------
    /// 
    ///     Music
    /// 
    /// -------------------------------------------

    public void PlayMusicMainMenu() {

        // Precautions
        if (_MUSIC_Mainmenu != null)
            _MUSIC_Mainmenu.Play();
    }

    public void PlayMusicGameplay() {

        // Precautions
        if (_MUSIC_Gameplay != null)
            _MUSIC_Gameplay.Play();
    }

    /// -------------------------------------------
    /// 
    ///     Player
    /// 
    /// -------------------------------------------
    
    public void PlayEngineLoop() {

        // Precautions
        if (_SFX_EngineLoops.Count > 0) {

            // Get random sound from list
            AudioSource sound = _SFX_EngineLoops[RandomSoundInt(_SFX_EngineLoops)];

            // Queue the sound to the voxel waiting list
            GetVoxelWaitingList().Add(sound);

            // If the sound is the only one in the list
            if (GetVoxelWaitingList().Count == 1) {
                
                // Play the sound 
                GetVoxelWaitingList()[0].Play();
                StartingPlayingVoxels();
            }
        }
    }

    public void PlayPlayerDeath(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnPlayerDeath.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnPlayerDeath);
            AudioSource sound = _SFX_OnPlayerDeath[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    /// -------------------------------------------
    /// 
    ///     Projectiles
    /// 
    /// -------------------------------------------

    public void PlayEnemyProjectileImpact(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnEnemyProjectileImpact.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnEnemyProjectileImpact);
            AudioSource sound = _SFX_OnEnemyProjectileImpact[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }
    public void PlayPlayerProjectileImpact(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnPlayerProjectileImpact.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnPlayerProjectileImpact);
            AudioSource sound = _SFX_OnPlayerProjectileImpact[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    public void PlayFireProjectileDefault(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnFireProjectileDefault.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnFireProjectileDefault);
            AudioSource sound = _SFX_OnFireProjectileDefault[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    public void PlayFireProjectileRapidFire(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnFireProjectileRapidFire.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnFireProjectileRapidFire);
            AudioSource sound = _SFX_OnFireProjectileRapidFire[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    public void PlayFireProjectileSpread(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnFireProjectileSpread.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnFireProjectileSpread);
            AudioSource sound = _SFX_OnFireProjectileSpread[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    /// -------------------------------------------
    /// 
    ///     Pickups
    /// 
    /// -------------------------------------------

    public void PlayPickupRapidFire(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnPickupRapidFire.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnPickupRapidFire);
            AudioSource sound = _SFX_OnPickupRapidFire[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    public void PlayPickupSpread(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnPickupSpread.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnPickupSpread);
            AudioSource sound = _SFX_OnPickupSpread[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

    public void PlayPickupHealthpack(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnPickupHealth.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnPickupHealth);
            AudioSource sound = _SFX_OnPickupHealth[i];

            // Play the sound
            sound.Play();
        }
    }

    public void PlayPickupSpeedBoost(float pitchMin, float pitchMax) {

        // Precautions
        if (_SFX_OnPickupSpeedBoost.Count > 0) {

            // Get random sound from list
            int i = RandomSoundInt(_SFX_OnPickupSpeedBoost);
            AudioSource sound = _SFX_OnPickupSpeedBoost[i];

            // Play the sound with a random pitch
            sound.pitch = Random.Range(pitchMin, pitchMax);
            sound.Play();
        }
    }

}