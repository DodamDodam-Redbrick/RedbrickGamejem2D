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

    List<Enemy> vsEnemy = new List<Enemy>();

    List<Enemy> inBoundEnemies = new List<Enemy>();

    int vsCount;

    public int cardIndex;

    float attackTime;

    Coroutine coDie;

    List<Bullet> bulletPool = new List<Bullet>();

    [SerializeField]
    public SpriteRenderer rangeView;

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

        ChangeEdgeColor();
    }

    void ChangeEdgeColor()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        SpriteRenderer unitSpriteRender = transform.parent.GetComponent<SpriteRenderer>();

        unitSpriteRender.GetPropertyBlock(mpb);

        Color[] color = { Color.black, Color.gray, Color.yellow };

        int colorIndex = ((int)unitInfo.unitType) % 10;

        mpb.SetFloat("_Outline", 1f);

        mpb.SetColor("_OutlineColor", color[colorIndex]);

        mpb.SetFloat("_OutlineSize", GameSystem.Instance.outlineSize);

        transform.parent.GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
    }

    bool CheckVsCount()
    {
        return vsEnemy.Count < unitInfo.entityStats.weight;
    }

    private void Update()
    {
        if (CheckVsCount() && inBoundEnemies.Count > 0) //대치중이 없을 때 근처 적 중 가장 처음 만난 적을 대치
        { //적 지정은 유닛에서만 할 거
            //일기토 시작
            AddvsEnemy();
            foreach(Enemy enemy in vsEnemy)
            {
                enemy.SetvsUnit(this);
            }
        }

        attackTime += Time.deltaTime;

        if(attackTime >= unitInfo.entityStats.fireRate && vsEnemy.Count > 0)
        {
            Attack(vsEnemy[0]);
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
            bullet.transform.LookAt(vsEnemy[0].transform);
        }
        else
        {
            if(((int)unitInfo.unitType % 100)/10 == (int)EntityType.wizard)
            {
                foreach(Enemy e in inBoundEnemies)
                {
                    e.GetDamaged(damage);
                }
            }
            else
            {
                enemy.GetDamaged(damage);
            }
        }

    }

    public void ShowRange()
    {
        if(rangeView != null)
            rangeView.gameObject.SetActive(true);
    }

    public void HideRange()
    {
        if(rangeView != null)
            rangeView.gameObject.SetActive(false);
    }

    public void Die()
    {
        GameSystem.Instance.battleMap.mapGrid.GetNodeFromVector(transform.position).isUse = false;

        if(vsEnemy.Count > 0)
        {
            foreach (var enemy in vsEnemy)
            {
                enemy.UnsetvsUnit();
            }
        }


        if (coDie == null)
            coDie = StartCoroutine(CoDie());
    }

    IEnumerator CoDie()
    {
        //죽는 애니메이션하고 죽을때까지 기다리고 없애기

        coDie = null;

        //조금 기다렸다가 끝내기
        if(unitInfo.unitType == UnitType.mainCharacter)
        {
            GameSystem.Instance.DefeatedBattle();
        }
        else
        {
            //메인캐릭터는 없으니까
            Player.Instance.unitList.Remove(unitInfo);
        }

        Destroy(transform.parent.gameObject);

        yield return null;
    }

    public void AddvsEnemy()
    {
        foreach(Enemy enemy in inBoundEnemies)
        {
            if (!CheckVsCount())
                break;

            if (!vsEnemy.Contains(enemy))
            {
                vsEnemy.Add(enemy);
            }
        }
    }

    public void RemovevsEnemy(Enemy enemy)
    {
        vsEnemy.Remove(enemy);
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

