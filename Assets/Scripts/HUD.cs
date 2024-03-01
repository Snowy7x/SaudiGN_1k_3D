using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [Header("Menus")] 
    [SerializeField] private Volume volume;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject pausePanel;

    [Header("Health Settings")]
    [SerializeField] private CanvasGroup damageImg;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float stayTime = 0.5f;
    private bool _fade = false;

        [Header("Info Settings")]
    [SerializeField] TMP_Text infoTxt;
    [SerializeField] TMP_Text titleText;
    private bool canInfo = true;

    private void Awake()
    {
        instance = this;
        canInfo = true;
    }

    private void Start()
    {
        SetInfo("Get to the control room");
        titleText.text = "";
        damageImg.alpha = 0;
        Continue();
    }

    private void Update()
    {
        if (_fade)
        {
            damageImg.alpha -= Time.deltaTime * fadeSpeed;
            if (damageImg.alpha <= 0)
            {
                _fade = false;
            } 
        }
    }

    public void SetInfo(string info)
    {
        if (canInfo) infoTxt.text = info;
    }
    
    public void SetInfo(string info, float duration)
    {
        if (canInfo) StartCoroutine(SetInfo_(info, duration));
    }

    public void SetTitle(string title, float duration = 1f, Color color = default)
    {
        StopCoroutine(nameof(SetTitle));
        StartCoroutine(SetTitle(title, color, duration));
    }

    IEnumerator SetTitle(string title, Color color, float time)
    {
        titleText.text = title;
        titleText.color = color;
        yield return new WaitForSeconds(time);
        titleText.text = "";
        titleText.color = Color.white;
    }
    
    IEnumerator SetInfo_(string title, float time)
    {
        infoTxt.text = title;
        canInfo = false;
        yield return new WaitForSeconds(time);
        infoTxt.text = "";
        canInfo = true;
    }

    public void GotHit()
    {
        _fade = false;
        damageImg.alpha = 1;
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(stayTime);
        _fade = true;
    }

    public void DeathPanel()
    {
        FOV(true);
        deathPanel.SetActive(true);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        loadingPanel.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void Pause()
    {
        if (deathPanel.activeSelf) return;
        if (pausePanel.activeSelf)
        {
            Continue();
            return;
        }
        FOV(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        deathPanel.SetActive(false);
        pausePanel.SetActive(true);
        winPanel.SetActive(false);
        loadingPanel.SetActive(false);
        Time.timeScale = 0;
    }

    public void Win()
    {
        if (deathPanel.activeSelf) return;
        FOV(true);
        deathPanel.SetActive(false);
        pausePanel.SetActive(false);
        loadingPanel.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void Reload()
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        FOV(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        deathPanel.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        loadingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void FOV(bool value)
    {
        foreach (var component in volume.profile.components)
        {
            if (component is DepthOfField)
            {
                component.active = value;
            }
        }
    }

    public bool IsPause()
    {
        return pausePanel.activeSelf;
    }
    
}
