using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum EnemyState
{
    idle,
    move,
    attack,
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    Animator anim;

    [SerializeField] //디버깅 끝나면 지워도 됨
    List<Vector3> wayPoints;

    List<Node> myWay;

    Coroutine coMove;
    Coroutine coMoveToWayPoint;

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

    private void Start()
    {
        //StartMove(); //디버깅용
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
        foreach (Vector3 wayPoint in wayPoints)
        {
            if (!Grid.Instance.GetNodeFromVector(wayPoint).canWalk)
            {
                Debug.Log($"{wayPoint.x}, {wayPoint.y}는 걸어갈 수 없는 위치입니다");
                continue;
            }
            SetPathByPosition(wayPoint);
            while(myWay != null)
                yield return null;
        }
    }

    public void SetPathByPosition(Vector3 goalPosition)
    {
        List<Node> newWay = null;

        if (Grid.Instance.IsOutOfBind(goalPosition) && Grid.Instance.GetNodeFromVector(transform.position) != Grid.Instance.GetNodeFromVector(goalPosition))
            newWay = PathFinding.PathFind(transform.position, goalPosition);

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
            transform.position = Vector2.MoveTowards(transform.position, targetNode.myPos, Time.deltaTime * speed);

            //보는 방향
            int characterX = Grid.Instance.GetNodeFromVector(transform.position).myX;

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
}
