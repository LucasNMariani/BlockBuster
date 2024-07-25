using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;
    public void Pausa()
    {
        
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
        GameManager.instance.SetTimeScale(0);

    }

    

    public void Reanudar()
    {
        
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
        GameManager.instance.SetTimeScale(1);
    }
}
