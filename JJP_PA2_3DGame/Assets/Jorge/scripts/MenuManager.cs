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
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float sensitivity;

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    private void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    private void SetSensitivity(float value)
    {
        sensitivity = value;
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
