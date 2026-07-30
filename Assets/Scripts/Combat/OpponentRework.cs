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

    public void OnEnable() {
        currentHp = _health;
    }
    
    public void Update() {
        float bearDistance = Vector3.Distance(transform.position, _bear.transform.position);
        float squirrelDistance = Vector3.Distance(transform.position, _squirrel.transform.position);
        
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
        if (currentHp <= 0f) {
            Debug.Log("Opponent dead");
            gameObject.SetActive(false);
        }
    }
}