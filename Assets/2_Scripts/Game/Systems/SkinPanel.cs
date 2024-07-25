using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanel : MonoBehaviour
{
    public string skinToEquip;
    [SerializeField] bool bought = false;
    [SerializeField] Button _buyButton;
    [SerializeField] Button _equipButton;
    private void Start()
    {
        if (Shop.instance.SkinsAlreadyBought.Contains(skinToEquip)) bought = true;
        if (bought)
        {
            if (_buyButton != null) _buyButton.interactable = false;
            if (_equipButton != null) _equipButton.interactable = true;
        }
        //_equipButton.interactable = true;
    }

    public void EquipSkin()
    {
        SkinLoader.skinToLoad = skinToEquip;
        Debug.Log("Skin " + skinToEquip + " equipped succesfully");
    }

    public void UnequipSkin() 
    { 
        SkinLoader.skinToLoad = null;
    }

    public void SkinBought()
    {
        if (_buyButton != null) _buyButton.interactable = false;
        if (_equipButton != null) _equipButton.interactable = true;
        if (!bought) bought = true;
        Shop.instance.SetSkinBought(skinToEquip);
    }

}
