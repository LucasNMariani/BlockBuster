using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour, IDataPersistence
{
    //[SerializeField]
    //private List<int> levelsCompleted = new List<int>();
    public int _levelsCompletedIndex;

    //private void Start()
    //{
    //    _levelsCompletedIndex = GameDataManager.instance.gameData.levelsAvailable; //Igualar la variable guardada en json
    //    //for (int i = 1; i < GameManager.instance.LevelsIndex + 1; i++)
    //    //{
    //    //    UpdateLevelsUnlocked(i);
    //    //}
    //    //UpdateLevelsUnlocked(_levelsCompletedIndex);
    //    //LevelsUnlockedMenu();
    //}

    //private void UpdateLevelsUnlocked(int levelNumber)
    //{
    //    if (!levelsCompleted.Contains(levelNumber))
    //        levelsCompleted.Add(levelNumber);
    //    else return;
    //}

    public void LevelsUnlockedMenu()
    {
        UIManager.instance.SetActiveUnlockedLevel(_levelsCompletedIndex);
    }

    public void LoadData(GameData data)
    {
        _levelsCompletedIndex = data.levelsAvailable;
    }

    public void SaveData(ref GameData data)
    {
        data.levelsAvailable = _levelsCompletedIndex;
    }
}
