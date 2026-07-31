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
    
    [Header("Textures")] 
    [SerializeField] private Sprite _flying;
    [SerializeField] private Sprite _normal;
    
    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpAction.performed += Jump;
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void Jump(InputAction.CallbackContext obj) {
        if (!IsGrounded()) return;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded() {
        return _rigidbody.linearVelocityY == 0f;
    }
    
    private void OnDisable() {
        _jumpAction.performed -= Jump;
        _moveAction.Disable();
        _jumpAction.Disable();
    }
    
    private void Update() {
        if (IsGrounded()) {
            Debug.Log("Sprite normal");
            spriteRenderer.sprite = _normal;
        }
        else {
            Debug.Log("Sprite flying");
            spriteRenderer.sprite = _flying;
        }
        float movementDirection = _moveAction.ReadValue<float>();
        Move(movementDirection);
    }
    
    public void Move(float  moveDirection) {
        if (moveDirection == 0f) return;
        
        if (moveDirection > 0f) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }
        
        float xMovement = moveDirection * Time.deltaTime * _movementSpeed;
        transform.Translate(new  Vector3(xMovement, 0f, 0f));
    }
}