using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string Level;
    [SerializeField] private GameObject  MainMenuPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider qualityDropdown;
    [SerializeField] private AudioSource audioSource;
    

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
      
    }

    private void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void Play()
    {
        SceneManager.LoadScene(Level);
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
