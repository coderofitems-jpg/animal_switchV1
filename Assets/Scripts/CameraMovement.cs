using UnityEngine;

public class CameraMovement : MonoBehaviour{
        
    [Header("Player")]
    [SerializeField] private GameObject _bear;
    [SerializeField] private GameObject _squirrel;
    
    [Header("Offset from Player")]
    [SerializeField] private Vector3 _offset = new  Vector3(3f, 0f, 0f);

    public void Update() {
        if (_bear.activeSelf) {
            transform.position = new Vector3(_bear.transform.position.x + _offset.x, _bear.transform.position.y + _offset.y,
                transform.position.z);
        }
        if (_squirrel.activeSelf) {
            transform.position = new Vector3(_squirrel.transform.position.x + _offset.x, _squirrel.transform.position.y + _offset.y,
                transform.position.z);
        }
    }
}