using UnityEngine;

public class CombatManager : MonoBehaviour {
    
    [Header("Spawning")]
    [SerializeField] private GameObject _squirrel;
    [SerializeField] private GameObject _bear;
    
    [Header("Combat")] 
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private Vector3 _originalSpawnpoint;
    
    private Vector3 _currentSpawnpoint;
    private float currentHp;

    public void OnEnable() {
        _currentSpawnpoint = _originalSpawnpoint;
        currentHp = _maxHp;
    }

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
        Debug.Log("Adding player hp" + currentHp);
        currentHp += hp;
        if (currentHp > _maxHp) {
            currentHp = _maxHp;
        }
    }
    
    public void SubtractHp(float hp) {
        Debug.Log("Subtracting player hp: " + currentHp);
        currentHp -= hp;
        if (currentHp <= 0) {
            Debug.Log("Player dead");
            Die();
        }
    }
    
    public void Die() {
        _squirrel.transform.position = _currentSpawnpoint;
        _bear.transform.position = _currentSpawnpoint;
        currentHp = _maxHp;
    }
    
    public void SetCurrentSpawnpoint(Vector3 pos) {
        _currentSpawnpoint = pos;
    }
}
