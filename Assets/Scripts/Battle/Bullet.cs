using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    Transform parentTransform;

    Unit unit;
    Enemy enemy;

    public Bullet(int damage, Unit unit, Transform parent)
    {
        this.damage = damage;
        this.unit = unit;
        parentTransform = parent;
    }

    public Bullet(int damage, Enemy enemy, Transform parent)
    {
        this.damage = damage;
        this.enemy = enemy;
        parentTransform = parent;
    }

    private void Update()
    {
        transform.Translate(transform.forward * GameSystem.Instance.bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unit != null)//유닛이 공격
        {
            Enemy hit = collision.GetComponent<Enemy>();

            if (hit != null)
            {
                hit.GetDamaged(damage);
                gameObject.SetActive(false);
            }
        }
        else if (enemy != null)//에너미가 공격
        {
            Unit hit = collision.GetComponent<Unit>();

            if (hit != null)
            {
                hit.GetDamaged(damage);
                gameObject.SetActive(false);
            }
        }
    }
}
