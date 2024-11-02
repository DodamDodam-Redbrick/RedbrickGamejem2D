using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInfo : Entity
{
    public UnitInfo DeepCopy()
    {
        UnitInfo other = (UnitInfo)this.MemberwiseClone();
        other.entityStats = entityStats.DeepCopy();

        return other;
    }

    public UnitInfo(EntityStats entityStats, UnitType unitType, Sprite thumbnail, GameObject unitPrefab, int cost, NodeType nodeType, GameObject bulletPrefab = null)
    {
        base.entityStats = entityStats;
        originDamage = entityStats.damage;

        this.unitType = unitType;
        this.thumbnail = thumbnail;
        this.cost = cost;
        this.placeNodeType = nodeType;

        base.entityPrefab = unitPrefab;
        base.bulletPrefab = bulletPrefab;
    }

    public int cost;
    public float originDamage;

    public UnitType unitType;

    public NodeType placeNodeType;

    [HideInInspector]
    public Sprite thumbnail;

}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo;

    public Node placedNode;

    Enemy vsEnemy;

    List<Enemy> inBoundEnemies = new List<Enemy>();

    int vsCount;

    int cardIndex;

    float attackTime;

    Coroutine coDie;

    List<Bullet> bulletPool = new List<Bullet>();

    [HideInInspector]
    public bool isSpawning;

    public void Init(UnitInfo unitInfo)
    {//메인캐릭터용
        this.unitInfo = unitInfo;

        attackTime = unitInfo.entityStats.fireRate;
    }

    public void Init(UnitInfo unitInfo, int cardIndex)
    {
        this.unitInfo = unitInfo;

        this.cardIndex = cardIndex;

        attackTime = unitInfo.entityStats.fireRate;
    }

    private void Update()
    {
        if (vsEnemy == null) //대치중이 없을 때 근처 적 중 가장 처음 만난 적을 대치
        { //적 지정은 유닛에서만 할 거
            if (inBoundEnemies.Count > 0)
            {
                //일기토 시작
                SetvsEnemy(inBoundEnemies[0]);
                vsEnemy.SetvsUnit(this);
            }
        }

        attackTime += Time.deltaTime;

        if(attackTime >= unitInfo.entityStats.fireRate && vsEnemy)
        {
            Attack(vsEnemy);
        }
    }

    public void Attack(Enemy enemy)
    {
        //여기서 장비나 디버프 전부 추가
        int damage = (int)(unitInfo.entityStats.damage * (1 - GameSystem.Instance.deBuffColdPercent));

        attackTime = 0;

        //어택 애니메이션
        if(unitInfo.bulletPrefab != null)
        {
            //RangeAttack
            Bullet bullet = GetUnUseBulletPool();
            if (bullet == null)
            {
                bullet = Instantiate(unitInfo.bulletPrefab, this.transform).GetComponent<Bullet>();
                bulletPool.Add(bullet);
            }
            else
            {
                bullet.gameObject.SetActive(true);
            }
            //불렛 구현해서 날리자 그냥 자기 데미지 불렛한테 넘겨주고 그 데미지 만큼 

            bullet.transform.position = Vector3.zero;
            bullet.transform.LookAt(vsEnemy.transform);

        }
        else
        {
            enemy.GetDamaged(damage);
        }

    }
  

    public void Die()
    {
        GameSystem.Instance.battleMap.mapGrid.GetNodeFromVector(transform.position).isUse = false;

        if(vsEnemy != null)
            vsEnemy.UnSetvsUnit();

        if (coDie == null)
            coDie = StartCoroutine(CoDie());
    }

    IEnumerator CoDie()
    {
        //죽는 애니메이션하고 죽을때까지 기다리고 없애기
        

        Destroy(transform.parent.gameObject);

        coDie = null;

        yield return null;
    }

    public void SetvsEnemy(Enemy enemy)
    {
        vsEnemy = enemy;
    }

    public void UnSetvsEnemy()
    {
        vsEnemy = null;
    }

    public void GetDamaged(int damage)
    {
        unitInfo.entityStats.hp -= (damage - unitInfo.entityStats.def);

        //피격 애니메이션

        if (unitInfo.entityStats.hp <= 0)
        {
            Die();
        }
    }

    void AddInBoundEnemy(Enemy enemy)
    {
        if (!inBoundEnemies.Contains(enemy))
        {
            inBoundEnemies.Add(enemy);
        }
    }

    void RemoveInBoundEnemy(Enemy enemy)
    {
        if (inBoundEnemies.Contains(enemy))
        {
            inBoundEnemies.Remove(enemy);
        }
    }

    Bullet GetUnUseBulletPool()
    {
        foreach (Bullet bullet in bulletPool)
        {
            if (bullet.gameObject.activeInHierarchy == false)
            {
                return bullet;
            }
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSpawning)
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                AddInBoundEnemy(enemy);
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!isSpawning)
        {            
            Enemy enemy = collision.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                RemoveInBoundEnemy(enemy);
            }
        }
    }
}

