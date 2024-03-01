using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    public GameObject loadScreen;
    private UIDocument _doc;
    private Button _playBtn;
    private Button _settingsBtn;
    private Button _creditsBtn;
    private Button _quitBtn;

    private void Awake()
    {
        loadScreen.SetActive(false);
        _doc = GetComponent<UIDocument>();
        _playBtn = _doc.rootVisualElement.Q<Button>("play");
        _playBtn.clicked += Play;
        
        _settingsBtn = _doc.rootVisualElement.Q<Button>("settings");
        _settingsBtn.clicked += Settings;
        
        _creditsBtn = _doc.rootVisualElement.Q<Button>("credits");
        _creditsBtn.clicked += Credits;
        
        _quitBtn = _doc.rootVisualElement.Q<Button>("quit");
        _quitBtn.clicked += Quit;
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void Settings()
    {
        PlayerPrefs.SetInt("Mode", 1);
        loadScreen.SetActive(true);
        SceneManager.LoadScene(1);
    }

    private void Credits()
    {
        PlayerPrefs.SetInt("Mode", 2);
        loadScreen.SetActive(true);
        SceneManager.LoadScene(1);
    }

    private void Play()
    {
        PlayerPrefs.SetInt("Mode", 0);
        loadScreen.SetActive(true);
        SceneManager.LoadScene(1);
    }
}
