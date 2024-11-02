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

    [SerializeField] //����� ������ ������ ��
    List<Vector3> wayPoints;

    List<Node> myWay;

    Coroutine coMove;
    Coroutine coMoveToWayPoint;

    MapGrid mapGrid;

    [SerializeField] //������
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
                    //�ִϸ��̼�
                    break;
                case EnemyState.move:
                    //anim.SetFloat("RunState", 0f);
                    //�ִϸ��̼�
                    break;

                case EnemyState.attack:
                    //�ִϸ��̼�
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
        int damage = (int)enemyInfo.entityStats.damage; //���⼭ ��� ����� ���� �߰�

        attackTime = 0;

        //���� �ִϸ��̼�
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
            //�ҷ� �����ؼ� ������ �׳� �ڱ� ������ �ҷ����� �Ѱ��ְ� �� ������ ��ŭ 

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
        //�״� �ִϸ��̼��ϰ� ���������� ��ٸ��� ���ֱ�
        GameSystem.Instance.battleMap.CountKillCount();
        //��Ȱ�� �ȵ�
        Destroy(gameObject);

        coDie = null;

        yield return null;
    }

    public void GetDamaged(int damage)
    {
        enemyInfo.entityStats.hp -= (damage - enemyInfo.entityStats.def); //���⼭ �����̳� �����v���ɷ� �� ����
        //�ǰ� �ִϸ��̼�

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
                    Debug.Log($"{wayPoint.x}, {wayPoint.y}�� �ɾ �� ���� ��ġ�Դϴ�");
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
            {//��������Ʈ���� �����ϸ� ���� ��������Ʈ�� ����
                index++;
                if (index >= myWay.Count)
                {
                    myWay = null;
                    yield break;
                }

                targetNode = myWay[index];
            }

            if (!inBoundUnits.Contains(vsUnit)) //�ִϸ��̼� �߿��� �������
            {
                transform.position = Vector2.MoveTowards(transform.position, targetNode.myPos, Time.deltaTime * enemyInfo.entityStats.moveSpeed);
            }

            //���� ����
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

