using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UserInputModule : MonoBehaviour
{
    [SerializeField]
    SerializedDictionary<InputActionReference, UnityEvent<InputAction.CallbackContext>> _inputActions;
    private Dictionary<InputAction, Action<InputAction.CallbackContext>> _handlers;

    void OnEnable() {
        if(_handlers == null) {
            _handlers = new();

            foreach(var action in _inputActions) {
                _handlers.Add(action.Key, (e) => action.Value?.Invoke(e));
            }
        }

        foreach(var action in _inputActions) {
            action.Key.action.performed += _handlers[action.Key];
        }
    }

    void OnDisable() {
        foreach(var action in _inputActions) {
            action.Key.action.performed -= _handlers[action.Key];
        }
    }
}