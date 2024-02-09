using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private SpriteRenderer _bodySpriteRenderer;
    [SerializeField] private Sprite[] _enemySprites;

    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _healthPickup;
    [SerializeField] private float _rateOfFire = 0.5f;
    [SerializeField] private float _attackRange = 7f;
    [SerializeField] private float _stopDistance = 2f;
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    public bool IsStuckInHook { get; private set; }

    private float _timeToShoot;

    private Animator _animtor;

    private Seeker _seeker;
    private AILerp _aiLerp;
    private AIDestinationSetter _aiDestinationSetter;

    private GameObject _temp;
    private void Awake()
    {
        _animtor = GetComponent<Animator>();
        _seeker = GetComponent<Seeker>();
        _aiLerp = GetComponent<AILerp>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
        _animtor.SetBool("Walk", true);

        SetRandomEnemySprite();
    }
    void SetRandomEnemySprite()
    {
        int random = Random.Range(0, _enemySprites.Length);
        _bodySpriteRenderer.sprite = _enemySprites[random];
    }
    public void GetStuckByHook()
    {
        IsStuckInHook = true;
        Disable_AI_Components();
    }
    void Disable_AI_Components()
    {
        _seeker.enabled = false;
        _aiLerp.enabled = false;
        _aiDestinationSetter.enabled = false;
    }
    void Fire()
    {
        _temp = Instantiate(_projectile, transform.position, Quaternion.identity);
        Vector3 direction = GameManager.instance._player.transform.position - transform.position;
        direction = direction.normalized;
        _temp.GetComponent<EnemyProjectile>().AddVelocity(direction);
    }
    private void Update()
    {
        if (!GameManager.instance || !GameManager.instance._player)
            return;

        if (IsStuckInHook)
            return;

        float distance = Vector3.Distance(GameManager.instance._player.transform.position, transform.position);

        if (distance <= _stopDistance)
        {
            _aiLerp.enabled = false;
            _animtor.SetBool("Walk", false);
        }
        else
        {
            _aiLerp.enabled = true;
            _animtor.SetBool("Walk", true);
        }

        _timeToShoot += Time.deltaTime;

        if (_timeToShoot > _rateOfFire && distance <= _attackRange)
        {
            _timeToShoot = 0;
            Fire();
        }
    }
    public void Damage(int amount)
    {
        _animtor.SetTrigger("Damage");
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            EnemySpawner.instance.DecrementEnemy();
            int random = Random.Range(0, 2);
            if(random == 1)
                Instantiate(_healthPickup, transform.position + new Vector3(0,-0.5f,0), Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (IsStuckInHook)
                Damage(1000);
            else
            {
                GameManager.instance._player.Damage(10);
            }
        }
    }
}
