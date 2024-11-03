using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum EnemyState
{
    idle,
    move,
    attack,
}

[System.Serializable]
public class EnemyInfo : Entity
{
    public EnemyInfo DeepCopy()
    {
        EnemyInfo other = (EnemyInfo)this.MemberwiseClone();
        other.entityStats = entityStats.DeepCopy();
        return other;
    }

    public EnemyInfo(EntityStats enemyStat, EnemyType enemyType, GameObject enemyPrefab, GameObject bulletPrefab = null)
    {
        base.entityStats = entityStats;
        this.enemyType = enemyType;

        base.entityPrefab = enemyPrefab;
        base.bulletPrefab = bulletPrefab;
    }

    public EnemyType enemyType;

}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Animator anim;

    [SerializeField] //디버깅 끝나면 지워도 됨
    List<Vector3> wayPoints;

    List<Node> myWay;

    Coroutine coMove;
    Coroutine coMoveToWayPoint;

    MapGrid mapGrid;

    [SerializeField] //디버깅용
    EnemyInfo enemyInfo;

    Coroutine coDie;

    EnemyState state;
    public EnemyState State
    {
        get { return state; }
        set { state = value;
            switch (value)
            {
                case EnemyState.idle:
                    //애니메이션
                    break;
                case EnemyState.move:
                    //anim.SetFloat("RunState", 0f);
                    //애니메이션
                    break;

                case EnemyState.attack:
                    //애니메이션
                    break;
            }
        
        }

    }

    Unit vsUnit;

    List<Unit> inBoundUnits = new List<Unit>();

    int cardIndex;

    float attackTime;

    List<Bullet> bulletPool = new List<Bullet>();

    private void Start()
    {
#if UNITY_EDITOR
        SetMapGrid();
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

            SetPathByPosition(mousePosition);
        }
#endif

        attackTime += Time.deltaTime;

        if (attackTime >= enemyInfo.entityStats.fireRate)
        {
            if (vsUnit != null)
            {
                if(inBoundUnits.Contains(vsUnit))
                    Attack(vsUnit);
                else
                {
                    if(inBoundUnits.Count > 0)
                        Attack(inBoundUnits[0]);
                }
            }
            else
            {
                if(inBoundUnits.Count > 0)
                    Attack(inBoundUnits[0]);
            }
        }
    }

    public void Init(EnemyInfo enemyInfo)
    {
        this.enemyInfo = enemyInfo;

        attackTime = enemyInfo.entityStats.fireRate;
    }

    public void Attack(Unit unit)
    {
        int damage = (int)enemyInfo.entityStats.damage; //여기서 장비나 디버프 전부 추가

        attackTime = 0;

        //어택 애니메이션
        if (enemyInfo.bulletPrefab != null)
        {
            //RangeAttack
            Bullet bullet = GetUnUseBulletPool();
            if (bullet == null)
            {
                bullet = Instantiate(enemyInfo.bulletPrefab, this.transform).GetComponent<Bullet>();
                bulletPool.Add(bullet);
            }
            else
            {
                bullet.gameObject.SetActive(true);
            }
            //불렛 구현해서 날리자 그냥 자기 데미지 불렛한테 넘겨주고 그 데미지 만큼 

            bullet.transform.position = Vector3.zero;

            if (vsUnit != null)
            {
                bullet.transform.LookAt(vsUnit.transform);
            }
            else
            {
                bullet.transform.LookAt(inBoundUnits[0].transform);
            }
        }
        else
        {
            unit.GetDamaged(damage);
        }
    }

    public void Die()
    {
        if(vsUnit != null)
            vsUnit.RemovevsEnemy(this);

        if(coDie == null)
            coDie = StartCoroutine(CoDie());

    }

    IEnumerator CoDie()
    {
        //죽는 애니메이션하고 죽을때까지 기다리고 없애기
        GameSystem.Instance.battleMap.CountKillCount();
        //재활용 안됨
        Destroy(gameObject);

        coDie = null;

        yield return null;
    }

    public void GetDamaged(int damage)
    {
        enemyInfo.entityStats.hp -= (damage - enemyInfo.entityStats.def); //여기서 방어력이나 방어구가틍ㄴ걸로 값 변경
        //피격 애니메이션

        if(enemyInfo.entityStats.hp <= 0)
        {
            Die();
        }
    }

    public void SetvsUnit(Unit unit)
    {
        vsUnit = unit;
    }

    public void UnsetvsUnit()
    {
        vsUnit = null;
    }

    public void SetMapGrid()
    {
        mapGrid = transform.parent.GetComponent<BattleManager>().mapGrid;
    }

    public void SetWayPoints(List<Vector3> wayPoints)
    {
        this.wayPoints = wayPoints;
    }

    public void StartMove()
    {
        if (coMoveToWayPoint != null)
        {
            StopCoroutine(coMoveToWayPoint);
            coMoveToWayPoint = null;
        }

        coMoveToWayPoint = StartCoroutine(CoMoveToWayPoint());
    }

    IEnumerator CoMoveToWayPoint()
    {
        yield return null;

        foreach (Vector3 wayPoint in wayPoints)
        {
            Node node = mapGrid.GetNodeFromVector(wayPoint);
            if (node != null)
            {
                if (!node.canWalk)
                {
                    Debug.Log($"{wayPoint.x}, {wayPoint.y}는 걸어갈 수 없는 위치입니다");
                    continue;
                }
                SetPathByPosition(wayPoint);
                while(myWay != null)
                    yield return null;

            }
        }
    }

    public void SetPathByPosition(Vector3 goalPosition)
    {
        List<Node> newWay = null;

        if (mapGrid.IsOutOfBind(goalPosition) && mapGrid.GetNodeFromVector(transform.position) != mapGrid.GetNodeFromVector(goalPosition))
            newWay = PathFinding.PathFind(transform.position, goalPosition, mapGrid);

        if (newWay != null)
        {
            if (coMove != null)
            {
                StopCoroutine(coMove);
                coMove = null;
            }

            //anim.SetFloat("RunState", 0.5f);
            myWay = newWay;

            coMove = StartCoroutine(CoMove());
        }
    }

    IEnumerator CoMove()
    {
        int index = 0;
        Node targetNode = myWay[0];
        while (true)
        {
            if (transform.position == targetNode.myPos)
            {//웨이포인트까지 도착하면 다음 웨이포인트로 간다
                index++;
                if (index >= myWay.Count)
                {
                    myWay = null;
                    yield break;
                }

                targetNode = myWay[index];
            }

            if (!inBoundUnits.Contains(vsUnit)) //애니메이션 중에도 멈춰야함
            {
                transform.position = Vector2.MoveTowards(transform.position, targetNode.myPos, Time.deltaTime * enemyInfo.entityStats.moveSpeed);
            }

            //보는 방향
            int characterX = mapGrid.GetNodeFromVector(transform.position).myX;

            if (characterX < targetNode.myX)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (characterX > targetNode.myX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            yield return null;
        }
    }

    void AddInBoundUnit(Unit unit)
    {
        if (!inBoundUnits.Contains(unit))
        {
            inBoundUnits.Add(unit);
        }
    }

    void RemoveInBoundUnit(Unit unit)
    {
        if (inBoundUnits.Contains(unit))
        {
            inBoundUnits.Remove(unit);
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
        Unit unit = collision.GetComponentInParent<Unit>();

        if (unit != null)
        {
            if (!unit.isSpawning)
            {
                AddInBoundUnit(unit);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Unit unit = collision.GetComponentInParent<Unit>();

        if (unit != null)
        {
            if (!unit.isSpawning)
            {
                RemoveInBoundUnit(unit);
            }
        }
    }

}

