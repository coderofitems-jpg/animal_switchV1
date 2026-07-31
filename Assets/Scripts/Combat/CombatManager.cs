using UnityEngine;

public class CombatManager : MonoBehaviour {
    
    [Header("Spawning")]
    [SerializeField] private GameObject _squirrel;
    [SerializeField] private GameObject _bear;
    
    [Header("Combat")] 
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private Vector3 _originalSpawnpoint;
    
    [Header("UI")]
    [SerializeField] private HealthBar healthBar;
    
    private Vector3 _currentSpawnpoint;
    private float currentHp;

    public void OnEnable() {
        _currentSpawnpoint = _originalSpawnpoint;
        currentHp = _maxHp;

        // Fallback, falls das Feld im Inspector nicht zugewiesen ist: sonst laufen
        // alle UpdateHealthBar-Aufrufe still ins Leere und die Leiste bleibt voll.
        if (healthBar == null) {
            healthBar = FindFirstObjectByType<HealthBar>(FindObjectsInactive.Include);

            if (healthBar == null) {
                Debug.LogWarning("CombatManager: keine HealthBar gefunden - die Leiste wird nicht aktualisiert.", this);
            }
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHp, _maxHp);
        }
    }
    
    public float GetHp() {
        return currentHp;
    }
    
    public float GetMaxHp() {
        return _maxHp;
    }
    
    public void SetMaxHp(float value) {
        _maxHp = value;
        
        if (currentHp > _maxHp) {
            currentHp = _maxHp;
        }

        UpdateHealthBar();
    }
    
    public void AddHp(float hp) {
        Debug.Log("Adding player hp" + currentHp);
        currentHp += hp;
        if (currentHp > _maxHp) {
            currentHp = _maxHp;
        }
        
        UpdateHealthBar();
    }
    
    public void SubtractHp(float hp) {
        Debug.Log("Subtracting player hp: " + currentHp);
        currentHp -= hp;
        if (currentHp < 0) {
            currentHp = 0;
        }
        
        UpdateHealthBar();
        
        if (currentHp <= 0) {
            Debug.Log("Player dead");
            Die();
        }
    }
    
    public void Die() {
        _squirrel.transform.position = _currentSpawnpoint;
        _bear.transform.position = _currentSpawnpoint;
        currentHp = _maxHp;
        
        UpdateHealthBar();
    }
    
    public void SetCurrentSpawnpoint(Vector3 pos) {
        _currentSpawnpoint = pos;
    }
}

// ============================================================================
// --- Achtung beim Mergen ---
// 1.  Branch enemy-rework ("Started Level Design") baut ein Tilemap-Level
//     (Grid mit Background/Foreground + Assets/Tilemap/*, MapPalette.prefab)
//     direkt in der GameScene, nicht in Level.unity. Diese GameScene-Version
//     kennt Canvas/HealthBar/EventSystem noch nicht -> Merge-Konflikt.
//     Vorher klaeren welche Level-Pipeline gilt: Tilemap oder die 456
//     Einzelsprites in Level.unity.
// ============================================================================