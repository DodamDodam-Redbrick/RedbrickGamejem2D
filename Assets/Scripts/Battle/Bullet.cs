using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    Transform target;

    Unit unit;

    Enemy enemy;

    public void Init(Unit unit, Transform target, float damage)
    {
        this.unit = unit;
        this.target = target;
        this.damage = damage;
    }

    public void Init(Enemy enemy, Transform target, float damage)
    {
        this.enemy = enemy;
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.LookAt(target);
            transform.Translate(transform.forward * GameSystem.Instance.bulletSpeed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unit != null)//유닛이 공격
        {
            Enemy hit = collision.GetComponent<Enemy>();

            if (hit != null)
            {
                hit.GetDamaged((int)unit.unitInfo.entityStats.damage);
                gameObject.SetActive(false);
            }
        }
        else if (enemy != null)//에너미가 공격
        {
            Unit hit = collision.GetComponent<Unit>();

            if (hit != null)
            {
                hit.GetDamaged((int)enemy.enemyInfo.entityStats.damage);
                gameObject.SetActive(false);
            }
        }
    }
}
