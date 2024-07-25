
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using static GameManager;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [Header("References")]
    [SerializeField]
    private List<GameObject> worldsButtonList = new List<GameObject>();
    [SerializeField]
    private GameObject tutorialPanel;
    [SerializeField]
    private TextMeshProUGUI _coinsTextUI;
    [SerializeField]
    private TextMeshProUGUI _staminaTextUI;
    [SerializeField]
    private TextMeshProUGUI _staminaTimerTextUI;
    [SerializeField] Slider _musicSlider, _sfxSlider;
    public Toggle _musicMute;

    [Header("WinPanel")]
    [SerializeField] 
    private TextMeshProUGUI _winCoinsText;
    [SerializeField] 
    private GameObject _winPanel;
    [Header("LosePanel")]
    [SerializeField] 
    private GameObject _losePanel;
    [Header("StaminaPanel")]
    [SerializeField]
    private GameObject _offerAdForStamina;

    [Header("ControllersUI")]
    [SerializeField] GameObject _buttons;
    [SerializeField] GameObject _joystick;
    
    

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            EventManager.Subscribe(TypeEvent.CompleteLevel, CompleteLevelUI);
            EventManager.Subscribe(TypeEvent.GameOver, GameOverUI);
            
        }
       
        if(_musicSlider != null && _sfxSlider != null)
        {
            _musicSlider.value = AudioManager.Instance.musicSource.volume;
            _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        }

        SetControllerUI();
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(TypeEvent.CompleteLevel, CompleteLevelUI);
        EventManager.UnSubscribe(TypeEvent.GameOver, GameOverUI);
    }

    public void SetActiveUnlockedLevel(int levelsCompleted)
    {
        for (int i = 0; i < worldsButtonList.Count; i++)
        {
            if (i <= levelsCompleted - 1)
            {
                var b = worldsButtonList[i].GetComponentInParent<Button>();
                b.interactable = true;
                worldsButtonList[i].SetActive(false);
            }
            else break;
        }
    }

   public void GameOverUI(params object[] parameters)
    {
        _losePanel.SetActive(true);
        Debug.Log("Termino el nivel pibe");
        AudioManager.Instance.PlaySFX("LoseSound");
        EventManager.UnSubscribe(TypeEvent.GameOver, GameOverUI);
    }
    
    public void CompleteLevelUI(params object[] parameters)
    {
        _winCoinsText.text = "Ganaste " + (string)parameters[0] + " coins!!";
        _winPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("WinSound");
        Debug.Log("Ganaste pibe");
        EventManager.UnSubscribe(TypeEvent.CompleteLevel, CompleteLevelUI);
    }

    public void UpdateStaminaText(int currentStamina, int maxStamina)
    {
        if(_staminaTextUI != null) _staminaTextUI.text = currentStamina.ToString() + " / " + maxStamina.ToString();
    }

    public void UpdateStaminaTimer(int currentStamina, int maxStamina, DateTime nextStaminaTime)
    {
        if (_staminaTimerTextUI != null)
        {
            if (currentStamina >= maxStamina)
            {
                //Stamina completa 
                _staminaTimerTextUI.text = "Stamina Completa";
                return;
            }
            //Estructura que nos da un intervalo de tiempo
            TimeSpan timer = nextStaminaTime - DateTime.Now;
            timer = nextStaminaTime - DateTime.Now;
            //Formato "00" para representar el horario con 2 digitos
            _staminaTimerTextUI.text = timer.Minutes.ToString("00") + ":" + timer.Seconds.ToString("00");
        }
    }

    public void UpdateCoinsText(int amount)
    {
        if (_coinsTextUI != null) _coinsTextUI.text = amount.ToString();
    }

    public void TutorialFirstTimePlaying()
    {
        tutorialPanel.SetActive(true);
    }

    public void NoStaminaOfferAdPanel()
    {
        _offerAdForStamina.SetActive(true);
    }

    public void ToggleMusicMute()
    {
        AudioManager.Instance.MuteMusic();
    }

    public void MusicSlideVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXSliderVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    public void UpdateVolume(float musicVolume, float sfxVolume, bool muteMusic)
    {
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;
    }

    public void SetControllerUI()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && _buttons != null && _joystick != null)
        {
            switch (GameManager.instance.controls)
            {
                case Controls.Multitouch: _buttons.SetActive(false); _joystick.SetActive(false); break;
                case Controls.Buttons: _buttons.SetActive(true); _joystick.SetActive(false); break;
                case Controls.VirtualJoystick: _buttons.SetActive(false); _joystick.SetActive(true); break;
            }
        }
    }



    

}
