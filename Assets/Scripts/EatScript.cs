using UnityEngine;

public class EatScript : MonoBehaviour {
    
    [SerializeField] private CombatManager _combatManager;
    
    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.CompareTag("Food")) {
            _combatManager.AddHp(collider2D.GetComponent<FoodScript>().GetHealthValue());
            collider2D.gameObject.SetActive(false);
        }
    }
}