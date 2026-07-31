using UnityEngine;

public class CheckpointScript : MonoBehaviour {

    [SerializeField] CombatManager _combatManager;
    [SerializeField] Sprite _checkpointCollectedSprite;

    public void OnTriggerEnter2D(Collider2D collider2D) {
        Debug.Log("Set current spawnpoint");
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = _checkpointCollectedSprite;
        _combatManager.SetCurrentSpawnpoint(collider2D.transform.position);
    }
}