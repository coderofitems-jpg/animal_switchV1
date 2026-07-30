using UnityEngine;

public class CheckpointScript : MonoBehaviour {

    [SerializeField] CombatManager _combatManager;

    public void OnTriggerEnter2D(Collider2D collider2D) {
        Debug.Log("Set current spawnpoint");
        _combatManager.SetCurrentSpawnpoint(collider2D.transform.position);
    }
}