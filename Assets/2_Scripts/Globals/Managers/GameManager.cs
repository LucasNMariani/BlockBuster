using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Controls { Multitouch, Buttons, VirtualJoystick }
public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance { get; private set; }
    [SerializeField]
    private int _staminaCostPerLevel = 5;
    private bool _isGameStarted = false;
    public bool isGameStarted => _isGameStarted;
    private bool _isFirstTimePlaying = true;
    public bool firstTimePlaying { get { return _isFirstTimePlaying; } set { _isFirstTimePlaying = value; } }
    private int _levelsAvailable = 1; 

    [SerializeField]
    private DragController _gameBar;
    public Transform gameBarPos => _gameBar.transform;
    private List<Bricks> _levelBrickReference = new List<Bricks>();
    [SerializeField] private List<BallBehabiour> _levelBallsReference = new List<BallBehabiour>();

    [SerializeField]
    private int _coinsEarnedPerLevel = 50;
    public int maxLifesPerLevel = 3;
    private int _currentLevelLifes;

    static Controls _controls;
    public Controls controls
    {
        get { return _controls; }

        set
        {
            if(_controls != value)
            {
                _controls = value;
                UIManager.instance.SetControllerUI();
                AudioManager.Instance.PlaySFX("MenuSound3");
            }
        }
    }

    float _timer;
    void RandomSound()
    {
        _timer += Time.deltaTime;

        if (_timer >= Random.Range(15f, 30f))
        {
            _timer = 0;
            Debug.Log("SPACESOUND");
            if (Random.Range(0, 101) < 50) AudioManager.Instance.PlaySFX("SpaceSound1");
            else AudioManager.Instance.PlaySFX("SpaceSound2");
        }
    }

    private void Update()
    {
        RandomSound();
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        _isGameStarted = false;
    }

    private void Start()
    {
        if (_gameBar == null) _gameBar = FindObjectOfType<DragController>();
        //_isFirstTimePlaying = GameDataManager.instance.gameData.isFirstTimePlayingGD; //Igualar la variable guardada en json
        _currentLevelLifes = maxLifesPerLevel;
        if (_levelBallsReference.Count >= 1) _levelBallsReference.Remove(_levelBallsReference[0]);
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            EventManager.Subscribe(TypeEvent.GameOver, GameOverGM);
            EventManager.Subscribe(TypeEvent.CompleteLevel, CompleteLevelGM);
        }
        AudioManager.Instance.PlayMusic(SceneManager.GetActiveScene().name);
        //EventManager.Subscribe(TypeEvent.ResetLevel, ResetLevelGM);

        
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(TypeEvent.GameOver, GameOverGM);
        EventManager.UnSubscribe(TypeEvent.CompleteLevel, CompleteLevelGM);
        //EventManager.Subscribe(TypeEvent.ResetLevel, ResetLevelGM);
    }

    #region Data
    public void LoadData(GameData data)
    {
        _isFirstTimePlaying = data.isFirstTimePlayingGD;
        _levelsAvailable = data.levelsAvailable;
        _controls = (Controls)data.controllerIndex;
    }

    public void SaveData(ref GameData data)
    {
        data.isFirstTimePlayingGD = _isFirstTimePlaying;
        data.levelsAvailable = _levelsAvailable;
        data.controllerIndex = (int)_controls;
    }

    #endregion

    #region Scenes
    public void LoadScene(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            GameDataManager.instance.SaveGame();
            SceneChanger.Instance.LoadAsyncSceneByName(sceneName);
            Time.timeScale = 1;
            return;
        }

        if (!StaminaManager.instance.HasEnoughStamina(_staminaCostPerLevel))
        {
            Debug.Log("Insuficient stamina");
            UIManager.instance.NoStaminaOfferAdPanel();
            return;
        }

        StaminaManager.instance.UseStamina(_staminaCostPerLevel);
        GameDataManager.instance.SaveGame();
        SceneChanger.Instance.LoadAsyncSceneByName(sceneName);
        Time.timeScale = 1;
    }
    

    public void ReloadScene()
    {
        
        if (!StaminaManager.instance.HasEnoughStamina(_staminaCostPerLevel))
        {
            Debug.Log("Insuficient stamina");
            UIManager.instance.NoStaminaOfferAdPanel();
            return;
        }
        

        StaminaManager.instance.UseStamina(_staminaCostPerLevel);
        GameDataManager.instance.SaveGame();
        Time.timeScale = 1;
        SceneChanger.Instance.LoadAsyncSceneByName(SceneManager.GetActiveScene().name);


    }

    public void NextScene()
    {
        if (!StaminaManager.instance.HasEnoughStamina(_staminaCostPerLevel))
        {
            Debug.Log("Insuficient stamina");
            UIManager.instance.NoStaminaOfferAdPanel();
            return;
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            StaminaManager.instance.UseStamina(_staminaCostPerLevel);
            GameDataManager.instance.SaveGame();
            Time.timeScale = 1;
            SceneChanger.Instance.LoadAsyncSceneByIndex(nextSceneIndex);
        }
        else 
        {
            Debug.LogWarning("Error en la siguiente escena o no hay siguiente escena");
        }
        
    }

    public void GoToMainMenu()
    {
        SceneChanger.Instance.LoadAsyncSceneByName("MainMenu");
    }

    #endregion

    #region Actions
    public void ExitGame()
    {
        GameDataManager.instance.SaveGame();
        Application.Quit();
    }

    

    public void SetIfGameIsStarted(bool start)
    {
        _isGameStarted = start;
    }

    public void SetTimeScale(int amount)
    {
        Time.timeScale = amount;
    }

    public bool IsInPause()
    {
        if (Time.timeScale == 0) return true;

        return false;
    }

    public void AddBrick(Bricks bk)
    {
        if (!_levelBrickReference.Contains(bk)) _levelBrickReference.Add(bk);
        else return;
    }

    public void RemoveBrick(Bricks bk)
    {
        if (_levelBrickReference.Contains(bk)) _levelBrickReference.Remove(bk);
        
        if (_levelBrickReference.Count <= 0) EventManager.Trigger(TypeEvent.CompleteLevel, _coinsEarnedPerLevel.ToString());
    }

    public void AddBall(BallBehabiour ball)
    {
        if (!_levelBallsReference.Contains(ball)) _levelBallsReference.Add(ball);
        else return;
    }

    public void RemoveBall(BallBehabiour ball)
    {
        if (_levelBallsReference.Contains(ball)) _levelBallsReference.Remove(ball);
        //if (_levelBallsReference.Count == 1) _levelBallsReference.Remove(_levelBallsReference[0]);
        
        if (_levelBallsReference.Count <= 0) EventManager.Trigger(TypeEvent.GameOver);
    }

    private void ClearListReferences()
    {
        _levelBrickReference.Clear();
        _levelBallsReference.Clear();
    }

    public void ChangeControllerStyleToJoystick()
    {
        controls = Controls.VirtualJoystick;
    }
    public void ChangeControllerStyleToTouch()
    {
        controls = Controls.Multitouch;
    }
    public void ChangeControllerStyleToButtons()
    {
        controls = Controls.Buttons;
    }

    #endregion

    #region GameEvents
    private void CompleteLevelGM(params object[] parameters)
    {
        Shop.instance.AddCoins(_coinsEarnedPerLevel);
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == _levelsAvailable) _levelsAvailable++;
        ClearListReferences();
        //GameDataManager.instance.SaveGame();
        Time.timeScale = 0;
        EventManager.UnSubscribe(TypeEvent.CompleteLevel, CompleteLevelGM);
    }
    
    private void GameOverGM(params object[] parameters)
    {
        ClearListReferences();
        Time.timeScale = 0;
        EventManager.UnSubscribe(TypeEvent.GameOver, GameOverGM);
    }

    private void ResetLevelGM(params object[] parameters)
    {
        _currentLevelLifes--;
        if (_currentLevelLifes <= 0) EventManager.Trigger(TypeEvent.GameOver);
    }

    #endregion

}