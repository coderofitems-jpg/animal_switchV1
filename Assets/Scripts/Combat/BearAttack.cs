using System.Collections.Generic;
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
        _attackAction.AddBinding("<Mouse>/leftButton");
        _attackAction.performed += Attack;
        _attackAction.Enable();
    }
    
    private void OnDisable() {
        // Unregister Input
        _attackAction.performed -= Attack;
        _attackAction.Disable();
    }

    public void Attack(InputAction.CallbackContext callbackContext) {
        // Check cooldown
        if (currentCooldown >  0f) {
            return;
        }
        Debug.Log("Looking for opponents");
        
        // Iterate through every opponent
        for(int i = 0; i < _opponents.Count; i++) {
            GameObject opponent = _opponents[i];
            float distance = Vector3.Distance(transform.position, opponent.transform.position);
            
            // If in range damage opponent
            if (distance <= _range) {
                // TODO Damage opponent
                OpponentRework opponentRework = opponent.GetComponent<OpponentRework>();
                opponentRework.SubtractHp(_damage);
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
