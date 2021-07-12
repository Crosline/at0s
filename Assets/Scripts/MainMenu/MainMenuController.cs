using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Animator mainMenuAnimator;

    // menu camera
    public MainMenuCamera menuCam;
    public GameObject settingsPanel;

    public GameObject controlsPanel;

    public AudioMixer audioMixer;
    public GameObject audioPanel;

    public GameObject videoPanel;
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public AudioSource buttonSound;

    public GameObject howToPlayPanel;

    public GameObject creditsPanel;

    // music slider values;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float dialogueVolume;

    // music sliders
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // video values
    public int graphicQuality;
    public bool isFullscreen;

    // video ui
    public TMPro.TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        
    }

    

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void openSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuAnimator.Play("OpenOptions");
    }

    public void closeSettings()
    {
        mainMenuAnimator.Play("CloseOptions");
        settingsPanel.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void openControls()
    {

        menuCam.camBehav = 1;
        mainMenuAnimator.Play("OpenControls");
        controlsPanel.SetActive(true);
    }

    public void closeControls()
    {
        menuCam.camBehav = 4;
        mainMenuAnimator.Play("CloseControls");
        controlsPanel.SetActive(false);
    }

    public void openAudioSettings()
    {
        menuCam.camBehav = 2;
        mainMenuAnimator.Play("OpenAudioOptions");
        audioPanel.SetActive(true);
    }

    public void openHowToPlay()
    {
        menuCam.camBehav = 5;
        mainMenuAnimator.Play("OpenHowToPlay");
        howToPlayPanel.SetActive(true);
    }

    public void closeAudioSettings()
    {
        menuCam.camBehav = 4;
        //saveMenuSettings();
        mainMenuAnimator.Play("CloseAudioOptions");
        audioPanel.SetActive(false);
    }

    public void openVideoSettings()
    {
        menuCam.camBehav = 1;
        mainMenuAnimator.Play("OpenVideoOptions");
        videoPanel.SetActive(true);
    }

    public void closeVideoSettings()
    {
        menuCam.camBehav = 4;
        //saveMenuSettings();
        mainMenuAnimator.Play("CloseVideoOptions");
        videoPanel.SetActive(false);
    }

    public void closeHowToPlay()
    {
        menuCam.camBehav = 4;
        //saveMenuSettings();
        mainMenuAnimator.Play("CloseHowToPlay");
        howToPlayPanel.SetActive(false);
    }

    public void openCredits()
    {
        menuCam.camBehav = 3;
        mainMenuAnimator.Play("OpenCredits");
        creditsPanel.SetActive(true);
    }

    public void closeCredits()
    {
        menuCam.camBehav = 4;
        mainMenuAnimator.Play("CloseCredits");
        creditsPanel.SetActive(false);
    }

    public void setMasterVolume(float volume)
    {
        masterVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", masterVolume);
    }

    public void setSFXVolume(float volume)
    {
        sfxVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SFX", sfxVolume);
    }

    public void setMusicVolume(float volume)
    {
        musicVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("Music", musicVolume);
    }



    private IEnumerator waitControls(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        controlsPanel.SetActive(true);
    }

    public void setQuality(int quality)
    {
        graphicQuality = quality;
        QualitySettings.SetQualityLevel(quality);
    }

    public void setFullscreen(bool isFullscreen)
    {
        this.isFullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    public void setResolution(int resIndex)
    {
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Screen.fullScreen);
        saveMenuSettings();
    }

    public void buttonOnClickSound()
    {
        buttonSound.Play();
    }

    /* public void loadSettingsAwake()
     {
         MenuSettings menuSettings = SaveSystem.LoadMenuSettings();

         if (menuSettings != null)
         {
             // load audio values
             masterSlider.value = menuSettings.masterVolume;
             musicSlider.value = menuSettings.musicVolume;
             sfxSlider.value = menuSettings.sfxVolume;
             dialogueSlider.value = menuSettings.dialogueVolume;

             this.masterVolume = menuSettings.masterVolume;
             this.musicVolume = menuSettings.musicVolume;
             this.sfxVolume = menuSettings.sfxVolume;
             this.dialogueVolume = menuSettings.dialogueVolume;

             setMasterVolume(this.masterVolume);
             setMusicVolume(this.musicVolume);
             setSFXVolume(this.sfxVolume);
             setDialogueVolume(this.dialogueVolume);

             // load video settings
             qualityDropdown.value = menuSettings.graphicQuality;
             fullscreenToggle.isOn = menuSettings.fullscreen;

             this.graphicQuality = menuSettings.graphicQuality;
             this.isFullscreen = menuSettings.fullscreen;

             setQuality(this.graphicQuality);
             setFullscreen(this.isFullscreen);
         }

     }*/

     public void saveMenuSettings()
     {
         this.masterVolume = masterSlider.value;
         this.musicVolume = musicSlider.value;
         this.sfxVolume = sfxSlider.value;

         this.graphicQuality = qualityDropdown.value;
         this.isFullscreen = fullscreenToggle.isOn;

         //SaveSystem.SaveMenuSettings(this);
     }

}
