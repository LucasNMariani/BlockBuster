using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class StaminaManager : MonoBehaviour, IDataPersistence
{
    public static StaminaManager instance { get; private set; }

    [SerializeField] int _maxStamina = 100;
    [SerializeField] float _timeToRechargeStamina = 5f;
    int _currentStamina;

    bool _recharging;

    DateTime nextStaminaTime;
    DateTime lastStaminaTime;

    [SerializeField] string notifTitle = "Full Stamina";
    [SerializeField] string notifText = "Se recarg� por completo la stamina del juego, ya volver a jugar y divertirte con los niveles";

    int id;
    TimeSpan timer;

    #region Old Stamina Variables
    //private int maxStamina = 100;
    //[SerializeField]
    //private int _currentStamina = 99;
    //public int currentStamina => _currentStamina;

    //[Header("Regain Stamina Variables")][SerializeField]
    //private float _secondsToRegainStamina = 30f;
    //[SerializeField]
    //private int _staminaEarnedAfterTimer = 5;
    //private float _timer;

    /*
    [SerializeField]
    private string maxStaminaString;*/
    #endregion

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        //_timer = _secondsToRegainStamina;
    }

    private void Start()
    {
        //if (!PlayerPrefs.HasKey("currentStamina"))
        //{
        //    PlayerPrefs.SetInt("currentStamina", _maxStamina);
        //}
        LoadPlayerPrefs();
        UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);
        StartCoroutine(RechargeStamina());

        if (_currentStamina < _maxStamina)
        {
            timer = nextStaminaTime - DateTime.Now;
            id = NotificationManager.Instance.DisplayNotification(notifTitle, notifText, AddDuration(DateTime.Now, ((_maxStamina - (_currentStamina) + 1) * _timeToRechargeStamina) + 1 + (float)timer.TotalSeconds));
        }
    }

    //Uso de lamda y goes to
    public bool HasEnoughStamina(int stamina) => _currentStamina - stamina >= 0;

    IEnumerator RechargeStamina()
    {
        UIManager.instance.UpdateStaminaTimer(_currentStamina, _maxStamina, nextStaminaTime);
        _recharging = true;

        while (_currentStamina < _maxStamina)
        {
            //chequeos de tiempo
            DateTime currentTime = DateTime.Now;
            DateTime nextTime = nextStaminaTime;

            bool staminaAdd = false;
            while (currentTime > nextTime)
            {
                //No quiero superar mi m�ximo de stamina
                if (_currentStamina >= _maxStamina) break;

                _currentStamina++;
                staminaAdd = true;
                UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);

                //Predecir cual va a ser mi pr�ximo momento a recargar stamina
                DateTime timeToAdd = nextTime;

                //Mas que nada para checkear tu estado de stamina en caso de que cerraste al app
                if (lastStaminaTime > nextTime)
                    timeToAdd = lastStaminaTime;

                //Creo una funcion para agregar el tiempo a nextTime
                nextTime = AddDuration(timeToAdd, _timeToRechargeStamina);
            }

            //Si se recargo stamina...
            if (staminaAdd)
            {
                nextStaminaTime = nextTime;
                lastStaminaTime = DateTime.Now;
            }

            UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);
            UIManager.instance.UpdateStaminaTimer(_currentStamina, _maxStamina, nextStaminaTime);
            SavePlayerPrefs();

            yield return new WaitForEndOfFrame();
        }

        NotificationManager.Instance.CancelNotification(id);
        _recharging = false;
    }

    DateTime AddDuration(DateTime date, float duration)
    {
        //En nuestro caso para testeo rapido se usa segundos
        return date.AddSeconds(duration);
        //return date.AddMinutes(duration); //Agregar minutos o horas en vez de segundos es cambiar el metodo nomas
    }

    void SavePlayerPrefs()
    {
        //PlayerPrefs.SetInt("currentStamina", _currentStamina);
        PlayerPrefs.SetString("nextStaminaTime", nextStaminaTime.ToString());
        PlayerPrefs.SetString("lastStaminaTime", lastStaminaTime.ToString());
    }

    void LoadPlayerPrefs()
    {
        //_currentStamina = PlayerPrefs.GetInt("currentStamina");
        nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("nextStaminaTime"));
        lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("lastStaminaTime"));
    }

    public void LoadData(GameData data)
    {
        _currentStamina = data.arkanoidCurrentStamina;
        UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);
    }

    public void SaveData(ref GameData data)
    {
        data.arkanoidCurrentStamina = _currentStamina;
    }

    DateTime StringToDateTime(string date)
    {
        if (string.IsNullOrEmpty(date))
        {
            return DateTime.Now; //Este mismo momento
            //DateTime.Today;    //Este mismo d�a a las 00:00hs
            //DateTime.UtcNow;   //Pasa el Universal Time Cordinated o (GTM) (Arg es GMT-3)
        }
        else return DateTime.Parse(date);
    }

    public void UseStamina(int staminaToUse)
    {
        if (_currentStamina - staminaToUse >= 0)
        {
            _currentStamina -= staminaToUse;
            UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);

            //NotificationManager.Instance.CancelNotification(id);
            //id = NotificationManager.Instance.DisplayNotification(notifTitle, notifText, AddDuration(DateTime.Now, ((_maxStamima - (_currentStamina) + 1) * _timeToRechargeStamina) + 1 + (float)timer.TotalSeconds));

            //Si no estoy recargando
            if (!_recharging)
            {
                //Settear el next Stamina Time y comienzo la carga.
                nextStaminaTime = AddDuration(DateTime.Now, _timeToRechargeStamina);
                StartCoroutine(RechargeStamina());
            }
            //Lleva al siguiente lvl o lo que sea
        }
        else
        {
            //Ofrecer un add para recargar stamina
            Debug.Log("No tengo Stamina!");
        }
    }

    public void AddStamina(int staminaToUse)
    {
        if (_currentStamina + staminaToUse <= _maxStamina)
        {
            _currentStamina += staminaToUse;
            UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);
        }
        else
        {
            _currentStamina = _maxStamina;
            UIManager.instance.UpdateStaminaText(_currentStamina, _maxStamina);
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerPrefs();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SavePlayerPrefs();
    }

    #region Old Stamina System
    //private void Start()
    //{
    //    _currentStamina = GameDataManager.instance.gameData.arkanoidCurrentStamina; //Igualar la variable guardada en json
    //    UIManager.instance.UpdateStaminaText(_currentStamina);
    //}

    //private void Update()
    //{
    //    if (!hasMaxStamina)
    //    {
    //        _timer -= Time.deltaTime;
    //        if (_timer <= 0)
    //        {
    //            _currentStamina += _staminaEarnedAfterTimer;
    //            UIManager.instance.UpdateStaminaText(_currentStamina);
    //            if (_currentStamina > maxStamina) _currentStamina = maxStamina;
    //            _timer = _secondsToRegainStamina;
    //        }
    //    }
    //}

    //public void ConsumeStamina(int amount)
    //{
    //    _currentStamina -= amount;
    //    GameDataManager.instance.gameData.arkanoidCurrentStamina = _currentStamina;
    //}

    //public void RechargeStamina(int amount)
    //{
    //    if (!hasMaxStamina)
    //    {
    //        _currentStamina += amount;
    //        if (_currentStamina > maxStamina) _currentStamina = maxStamina;
    //        GameDataManager.instance.gameData.arkanoidCurrentStamina = _currentStamina;
    //        UIManager.instance.UpdateStaminaText(_currentStamina);
    //    }
    //}

    //public bool HasStamina(int amount)
    //{
    //    return _currentStamina >= amount;
    //}

    //public bool hasMaxStamina
    //{
    //    get { return _currentStamina >= maxStamina; }
    //}
    #endregion
}
