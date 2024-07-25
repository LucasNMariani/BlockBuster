
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{

    
    Vector2 inputVector;

    
    public float GetInputVector()
    {
        return inputVector.x;
    }
    public void OnDrag(PointerEventData eventData)
    {
        inputVector = eventData.position - (Vector2)transform.position;
        inputVector /= GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        
        transform.localPosition = inputVector * (GetComponent<RectTransform>().sizeDelta.x * 0.25f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        inputVector = Vector2.zero;
    }

}


