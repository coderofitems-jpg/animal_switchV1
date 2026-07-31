using System;
using UnityEngine;

public class SumpfScript : MonoBehaviour {
    
    [SerializeField] private CombatManager _combatManager;
    
    public void OnTriggerEnter2D(Collider2D other) {
        _combatManager.Die();
    }
}