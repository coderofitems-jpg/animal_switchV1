using UnityEngine;

public class CombatManager : MonoBehaviour {
    
    [Header("Combat")] 
    [SerializeField] private float _maxHp = 100f;
    private float currentHp;

    public float GetHp() {
        return currentHp;
    }
    
    public float GetMaxHp() {
        return _maxHp;
    }
    
    public void SetMaxHp(float value) {
        _maxHp = value;
    }
    
    public void AddHp(float hp) {
        Debug.Log("Adding hp");
        currentHp += hp;
        if (currentHp > _maxHp) {
            currentHp = _maxHp;
        }
    }
}
