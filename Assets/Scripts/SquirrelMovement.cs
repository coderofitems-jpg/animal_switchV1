
using UnityEngine;
using UnityEngine.InputSystem;

public class SquirrelMovement : MonoBehaviour {
    
    [SerializeField] private Rigidbody2D _rigidbody;
    
    [Header("Movement")]
    [SerializeField] private InputAction _moveAction;
    [SerializeField] private float _movementSpeed = 10f;

    [Header("Jump")] 
    [SerializeField] private InputAction _jumpAction;
    [SerializeField] private float _jumpForce = 1f;
    private bool isGrounded;

    private void OnEnable() {
        _jumpAction.performed += Jump;
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void Jump(InputAction.CallbackContext obj) {
        if (!isGrounded) return;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode2D.Impulse);
    }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.CompareTag("Ground")) {
            isGrounded = true;
        }
    }
    
    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.CompareTag("Ground")) {
            isGrounded = false;
        }
    }
    
    private void OnDisable() {
        _jumpAction.performed -= Jump;
        _moveAction.Disable();
        _jumpAction.Disable();
    }
    
    private void Update() {
        float movementDirection = _moveAction.ReadValue<float>();
        Move(movementDirection);
    }
    
    public void Move(float  moveDirection) {
        float xMovement = moveDirection * Time.deltaTime * _movementSpeed;
        transform.Translate(new  Vector3(xMovement, 0f, 0f));
    }
}
