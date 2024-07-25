using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    public static string skinToLoad;
    public GameObject defaultSkin;
    public GameObject[] skinsPrefabs;

    private void Awake()
    {
        if (skinToLoad == null) defaultSkin.gameObject.SetActive(true);
        else
        {
            foreach (var skin in skinsPrefabs)
            {
                if (skinToLoad == skin.gameObject.name)
                {
                    skin.gameObject.SetActive(true);
                }
            }
        }
        //if (skinToLoad != null && skinToLoad != defaultSkin)
        //{
        //    defaultSkin.gameObject.SetActive(false);
        //    skinToLoad.gameObject.SetActive(true);
        //    defaultSkin = skinToLoad;
        //    //defaultSkin.gameObject.SetActive(false);
        //    //Instantiate(skinToLoad, transform);
        //}
    }
}
