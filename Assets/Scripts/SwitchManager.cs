using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchManager : MonoBehaviour {

    [Header("Switching")]
    [SerializeField] private InputAction _switchAction;
    [SerializeField] private GameObject _squirrel;
    [SerializeField] private GameObject _bear;

    private bool isBear = true;
    
    private void OnEnable() {
        // In -/ Activate
        _squirrel.SetActive(false);
        _bear.SetActive(true);
        
        // Register Input
        _switchAction.performed += Switch;
        _switchAction.Enable();
    }

    private void OnDisable() {
        // Unregister Input
        _switchAction.performed -= Switch;
        _switchAction.Disable();
    }
    
    public void Switch(InputAction.CallbackContext obj) {
        if(isBear) {
            // In -/ Activate
            isBear = false;
            _squirrel.SetActive(true);
            _bear.SetActive(false);
            // Set position
            _squirrel.transform.position = _bear.transform.position;
        }
        else {
            // In -/ Activate
            isBear = true;
            _squirrel.SetActive(false);
            _bear.SetActive(true);
            // Set position
            _bear.transform.position = _squirrel.transform.position;
        }
    }
}
