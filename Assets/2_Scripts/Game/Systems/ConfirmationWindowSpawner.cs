
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationWindowSpawner : MonoBehaviour
{
    public UnityEvent onConfirmEvent;
    
    [SerializeField] Text _confirmationText;
    public void SetText(string personalizedText)
    {
        _confirmationText.text = personalizedText;
    }

    //void DeleteText()
    //{
    //    _confirmationText.text = "";
    //}

    //public void SetConfirmEvent(UnityAction listener)
    //{
    //    confirmListener = listener;
    //}

    //public void SetCancelEvent(UnityAction listener)
    //{
    //    cancelListener = listener;
    //}
    public void BTN_Confirm()
    {
        Debug.Log("Confirmacion aceptada");
        onConfirmEvent?.Invoke();
        Destroy(gameObject);
    }

    public void BTN_Cancel()
    {
        Debug.Log("Confirmacion cancelada");
        Destroy(gameObject);
    }
}
