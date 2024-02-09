using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField] private Sprite[] _weaponSprites;
    [SerializeField] private float _force = 100f;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        SetRandomWeaponSprite();
    }
    void SetRandomWeaponSprite()
    {
        int random = Random.Range(0, _weaponSprites.Length);
        _weaponSpriteRenderer.sprite = _weaponSprites[random];
    }
    public void AddVelocity(Vector3 direction)
    {
        _rigidbody2D.velocity = direction * _force * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.instance._player.Damage(3);
        }
        Destroy(gameObject);
    }
}
