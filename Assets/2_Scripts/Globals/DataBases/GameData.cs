using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [Header("GameStates")]
    public bool isFirstTimePlayingGD;
    public int levelsAvailable;
    [Header("Currency & Stamina")]
    public int arkanoidCurrentCoins;
    public int arkanoidCurrentStamina;
    [Header("AudioManager")]
    public bool muteMusic;
    [Range(0,1f)] public float sfxVolume;
    [Range(0,1f)] public float musicVolume;
    [Header("Skins")]
    public List<string> skinsAlreadyBought;
    [Header("Controller Type")]
    [Range(0,2)]public int controllerIndex;

    //Este constructor tiene el valor con el que se inicializan todas las variables
    public GameData()
    {
        isFirstTimePlayingGD = true;
        levelsAvailable = 1;
        arkanoidCurrentCoins = 0;
        arkanoidCurrentStamina = 100;
        muteMusic = false;
        sfxVolume = 1f;
        musicVolume = 1f;
        skinsAlreadyBought = new List<string>();
        controllerIndex = 0;
    }
}