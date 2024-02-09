using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _swordRight;
    [SerializeField] private GameObject _swordLeft;

    private SwordHandler _swordHandler_Right;
    private SwordHandler _swordHandler_Left;
    private SwordHandler _activeSword;

    [SerializeField] private float _maxHealth = 100;
    private float _currentHealth;
    [SerializeField] private Image _playerHealthFillBar;


    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _moveDirection;

    private bool _horizontalFacing;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _swordHandler_Right = _swordRight.transform.GetChild(0).GetComponent<SwordHandler>();
        _swordHandler_Left = _swordLeft.transform.GetChild(0).GetComponent<SwordHandler>();

        int swordIndex = _horizontalFacing == false ? 0 : 1;
        EnableSwords(swordIndex);
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    void EnableSwords(int index)
    {
        if (index == 0)
        {
            _swordRight.SetActive(true);
            _swordLeft.SetActive(false);
            if (_activeSword != _swordHandler_Right)
                _activeSword = _swordHandler_Right;
        }
        else if (index == 1)
        {
            _swordRight.SetActive(false);
            _swordLeft.SetActive(true);
            if (_activeSword != _swordHandler_Left)
                _activeSword = _swordHandler_Left;
        }
    }
    public void Damage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        float value = _currentHealth / _maxHealth;
        _playerHealthFillBar.fillAmount = value;
        if (_currentHealth <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
    // Update is called once per frame
    void Update()
    {
        #region Input
        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(KeyCode.W))
            vertical = 1;
        if (Input.GetKey(KeyCode.S))
            vertical = -1;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
            _horizontalFacing = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            _horizontalFacing = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _activeSword.Attack();
        }
        #endregion
        //flip player sprite based on horizontal input
        _spriteRenderer.flipX = _horizontalFacing;

        int swordIndex = _horizontalFacing == false ? 0 : 1;
        EnableSwords(swordIndex);

        _moveDirection = new Vector2(horizontal, vertical).normalized;

        if (_moveDirection.normalized != Vector2.zero)
            _animator.SetBool("Walk", true);
        else
            _animator.SetBool("Walk", false);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _moveDirection * _moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            if (!GameManager.instance.IsKeyCollected)
                GameManager.instance.ShowCollectKeyMessageToPlayer();
            else
                GameManager.instance.OpenDoor();
        }
        if (collision.CompareTag("Key"))
        {
            collision.gameObject.SetActive(false);
            GameManager.instance.KeyCollected();
        }
        if (collision.CompareTag("Health"))
        {
            Destroy(collision.gameObject);
            _currentHealth = _maxHealth;
            _playerHealthFillBar.fillAmount = 1;
        }
    }
}
