
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IUnityAdsListener, IDataPersistence
{
    public static Shop instance { get; private set; }
    [SerializeField] private int _currentCoins;
    [SerializeField] string androidGameID = "5317410", adID = "Rewarded_Android";

    [SerializeField] int _skinPrize = 50;
    [SerializeField] List<string> _skinsAlreadyBought;
    public List<string> SkinsAlreadyBought => _skinsAlreadyBought;
    //private int _skin1;
    //private int _skin2;
    // private int _skin3;

    int numOfAdTrigger;
    int _amount;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        //_currentCoins = GameDataManager.instance.gameData.arkanoidCurrentCoins;

        Advertisement.AddListener(this);
        Advertisement.Initialize(androidGameID);

        //_skin1 = PlayerPrefs.GetInt("skin1");
        //_skin2 = PlayerPrefs.GetInt("skin2");
       // _skin3 = PlayerPrefs.GetInt("skin3");
    }

    public void LoadData(GameData data)
    {
        _skinsAlreadyBought = data.skinsAlreadyBought;
        _currentCoins = data.arkanoidCurrentCoins;
        UIManager.instance.UpdateCoinsText(_currentCoins);
    }

    public void SaveData(ref GameData data)
    {
        data.skinsAlreadyBought = _skinsAlreadyBought;
        data.arkanoidCurrentCoins = _currentCoins;
    }

    //public void SaveSkin(string skinToSave)
    //{
    //    if (!PlayerPrefs.HasKey(skinToSave))
    //    {
    //        PlayerPrefs.SetInt(skinToSave, 1);
    //        _skin1 = PlayerPrefs.GetInt(skinToSave);
    //        _skin2 = PlayerPrefs.GetInt(skinToSave);
    //        _skin3 = PlayerPrefs.GetInt(skinToSave);

    //    }
    //}

    public void BuyStamina(int amount)
    {
        ShowRewardAd();
        numOfAdTrigger = 0;
        _amount = amount;
        
    }

    public void ConsumeStamina(int amount)
    {
        if (StaminaManager.instance.HasEnoughStamina(amount)) StaminaManager.instance.UseStamina(amount);
    }

    

    public void BuyCoins(int amount)
    {
        ShowRewardAd();
        numOfAdTrigger = 1;
        _amount = amount;
    }

    public void AddCoins(int amount)
    {
        _currentCoins += amount;
    }

    public void ConsumeCoins(int amount)
    {
        if (!CanPurchase(amount))
        {
            Debug.Log("No hay suficientes monedas");
            return;
        }

        _currentCoins -= amount;
        UIManager.instance.UpdateCoinsText(_currentCoins);
        //GameDataManager.instance.gameData.arkanoidCurrentCoins = _currentCoins;
    }

    #region Skins Methods
    public void UnlockedSkin(SkinPanel skinPanel)
    {
        if (CanPurchase(_skinPrize))
        {
            ConsumeCoins(_skinPrize);
            skinPanel.SkinBought();
            Debug.Log("Skin purchased");
        }
        else
            Debug.Log("Cant purchase this skin");
    }

    public void SetSkinBought(string skinName)
    {
        if(!_skinsAlreadyBought.Contains(skinName)) _skinsAlreadyBought.Add(skinName);
    }
    #endregion

    void ShowRewardAd()
    {
        if (!Advertisement.IsReady()) return;

        Advertisement.Show(adID);
        
    }


    private bool CanPurchase(int amount) => amount <= _currentCoins;

    #region UnusedCallbacks
    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
       
    }

    #endregion

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != adID) return;
        

        if (showResult == ShowResult.Finished && numOfAdTrigger == 0)
        {
            StaminaManager.instance.AddStamina(_amount);
            return;
        }

        if(showResult == ShowResult.Finished && numOfAdTrigger == 1)
        {
            _currentCoins += _amount;
            UIManager.instance.UpdateCoinsText(_currentCoins);
            //GameDataManager.instance.gameData.arkanoidCurrentCoins = _currentCoins;
            return;
        }

        if(showResult == ShowResult.Skipped && numOfAdTrigger == 0)
        {
            StaminaManager.instance.AddStamina(_amount / 2);
            return;
        }

        if (showResult == ShowResult.Skipped && numOfAdTrigger == 1)
        {
            _currentCoins += _amount / 2;
            UIManager.instance.UpdateCoinsText(_currentCoins);
            //GameDataManager.instance.gameData.arkanoidCurrentCoins = _currentCoins;
            return;
        }


    }

}
