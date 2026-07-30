using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class BearAttack : MonoBehaviour {

    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _cooldown = 5f;
    
    [SerializeField] private InputAction _attackAction;
    
    [SerializeField] private List<GameObject> _opponents;

    private float currentCooldown = 0f;
    
    private void OnEnable() {
        // Register Input
        _attackAction.performed += Attack;
        _attackAction.Enable();
    }
    
    private void OnDisable() {
        // Unregister Input
        _attackAction.performed -= Attack;
        _attackAction.Disable();
    }

    public void Attack(InputAction.CallbackContext callbackContext) {
        Debug.Log("Try attack");
        
        // Check cooldown
        if (currentCooldown >  0f) {
            return;
        }
        
        Debug.Log("Cooldown inactive");
        
        // Iterate through every opponent
        for(int i = 0; i < _opponents.Count; i++) {
            GameObject opponent = _opponents[i];
            float distance = Vector3.Distance(transform.position, opponent.transform.position);
            
            // If in range damage opponent
            if (distance <= _range) {
                // Damage opponent
                Debug.Log("Damage opponent");
            }
        }
            
        // Set cooldown
        currentCooldown += _cooldown;
    }

    public void Update() {
        // Update cooldown
        if (currentCooldown > 0f) {
            currentCooldown -= Time.deltaTime;
        }
    }
}
