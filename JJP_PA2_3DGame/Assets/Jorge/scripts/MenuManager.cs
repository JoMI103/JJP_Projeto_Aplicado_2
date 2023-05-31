using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string Level;
    [SerializeField] private GameObject  MainMenuPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    
    [SerializeField] private Toggle vsuncToggle;
    [SerializeField] private AudioSource audioSource;
    

    private void Start() {
        GetPlayerPrefs();
       
        turnOffOnVsync();
        SetQuality();
    }

    public void SetVolume()
    {
    
      //  AudioSettings.
       // audioSource.volume = volumeSlider.value;
        
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }
     public void turnOffOnVsync(){
        if(vsuncToggle.isOn){  QualitySettings.vSyncCount = 1; }
        else  {QualitySettings.vSyncCount = 0; Application.targetFrameRate = -1;}
    }
    
    public void Play()
    {
        setPlayerPrefs();
        SceneManager.LoadScene(Level);
    }

    private void GetPlayerPrefs(){
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 0); SetQuality();
        
        if(PlayerPrefs.GetInt("Vsync", 0) == 1) vsuncToggle.isOn = true;
        else vsuncToggle.isOn = false; turnOffOnVsync();
        
    }
     private void setPlayerPrefs(){
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);
        if(vsuncToggle.isOn) PlayerPrefs.SetInt("Vsync", 1); else PlayerPrefs.SetInt("Vsync",0);
        
    }

    public void OpenOptions()
    {
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);       
    }

    public void Quit()
    {
        Application.Quit();
    }
    
   
}
