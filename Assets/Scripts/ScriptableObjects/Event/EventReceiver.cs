using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    [SerializeField] private List<GenericEventListener> _eventListeners;

    private void OnEnable()
    {
        foreach (var eventListener in _eventListeners)
        {
            eventListener.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var eventListener in _eventListeners)
        {
            eventListener.Disable();
        }
    }
}