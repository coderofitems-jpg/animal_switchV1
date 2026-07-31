using UnityEngine;

public class OpponentRework : MonoBehaviour {

    [SerializeField] private float _health = 25f;
    [SerializeField] private float _attackDamage = 5f;
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _cooldown = 2f;
    
    [SerializeField] private GameObject _bear;
    [SerializeField] private GameObject _squirrel;
    [SerializeField] private CombatManager _combatManager;
    
    private float currentCooldown;
    private float currentHp;
    private EnemyHealthBar healthBar;

    public void OnEnable() {
        currentHp = _health;

        // Baut sich selbst auf und bleibt bis zum ersten Treffer unsichtbar,
        // dieselbe Leiste die auch Enemy benutzt.
        if (healthBar == null) {
            healthBar = GetComponent<EnemyHealthBar>();

            if (healthBar == null) {
                healthBar = gameObject.AddComponent<EnemyHealthBar>();
            }
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        if (healthBar == null) {
            return;
        }

        // _health 0 waere sonst eine Division durch null und damit NaN.
        healthBar.SetPercent(_health > 0f ? currentHp / _health : 0f);
    }
    
    public void Update() {
        float bearDistance = Vector3.Distance(transform.position, _bear.transform.position);
        float squirrelDistance = Vector3.Distance(transform.position, _squirrel.transform.position);
        
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (_bear.transform.position.x > transform.position.x) {
            sprite.flipX = false;
        }
        else {
            sprite.flipX = true;
        }
        
        if (squirrelDistance <= _range ||  bearDistance <= _range) {
            if (currentCooldown <= 0f) {
                Attack();
                currentCooldown = _cooldown;
            }
            currentCooldown -= Time.deltaTime;
        }
    }

    public void Attack() {
        Debug.Log("Opponent attacking");
        _combatManager.SubtractHp(_attackDamage);
    }

    public void SubtractHp(float hp) {
        Debug.Log("Subtracting opponent hp: " + currentHp);
        currentHp -= hp;
        if (currentHp < 0f) {
            currentHp = 0f;
        }

        UpdateHealthBar();

        if (currentHp <= 0f) {
            Debug.Log("Opponent dead");
            gameObject.SetActive(false);
        }
    }
}