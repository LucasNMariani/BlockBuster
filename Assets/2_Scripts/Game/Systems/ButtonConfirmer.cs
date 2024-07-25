using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonConfirmer : MonoBehaviour
{
    public GameObject confirmationPanelPrefab;
    public UnityEvent onConfirmEvent;
    [SerializeField] string _personalizedText;

    public void OnButtonClick()
    {
        GameObject confirmationPanel = Instantiate(confirmationPanelPrefab, GameObject.Find("Canvas").transform);

        ConfirmationWindowSpawner confirmationScript = confirmationPanel.GetComponent<ConfirmationWindowSpawner>();
        if (confirmationScript != null)
        {

            confirmationScript.SetText(_personalizedText);


            confirmationScript.onConfirmEvent = onConfirmEvent;
            

        }
    }

}
