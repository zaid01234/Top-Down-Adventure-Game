using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHandler : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        _boxCollider2D.enabled = false;
    }
    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }
    public void OnSwing()
    {
        _boxCollider2D.enabled = true;
    }
    public void OnRetract()
    {
        _boxCollider2D.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(50);
        }
    }
}
