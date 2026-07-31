
using UnityEngine;
using UnityEngine.InputSystem;

public class BearMovement : MonoBehaviour {
    
    [SerializeField] private Rigidbody2D _rigidbody;
    
    [Header("Movement")]
    [SerializeField] private InputAction _moveAction;
    [SerializeField] private float _movementSpeed = 10f;

    [Header("Jump")] 
    [SerializeField] private InputAction _jumpAction;
    [SerializeField] private float _jumpForce = 1f;
    
    [Header("Textures")] 
    [SerializeField] private Sprite _bearWalking;
    [SerializeField] private Sprite _bearStanding;
    
    private bool isGrounded;

    private void OnEnable() {
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
        float movementDirection = _moveAction.ReadValue<float>();
        Move(movementDirection);
    }
    
    public void Move(float  moveDirection) {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _bearStanding;
        if (moveDirection == 0f) return;
        
        if (moveDirection > 0f) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }
        
        float xMovement = moveDirection * Time.deltaTime * _movementSpeed;
        transform.Translate(new  Vector3(xMovement, 0f, 0f));
        spriteRenderer.sprite = _bearWalking;
    }
}
