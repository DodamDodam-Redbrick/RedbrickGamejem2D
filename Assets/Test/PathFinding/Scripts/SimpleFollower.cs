using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollower : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    Animator anim;

    [SerializeField]
    PathFinding path;

    [SerializeField]
    Grid grid;

    List<Node> myWay;
    Node targetNode;
    Vector3 mousePosition;
    Coroutine coMove;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
            List<Node> newWay = null;

            if (grid.IsOutOfBind(mousePosition) && grid.GetNodeFromVector(transform.position) != grid.GetNodeFromVector(mousePosition))
                newWay = path.PathFind(transform.position, mousePosition);

            if (newWay != null) //길을 못찾으면 이동시도조차 안함
            {
                if (coMove != null)
                {
                    StopCoroutine(coMove);
                    coMove = null;
                }

                anim.SetFloat("RunState", 0.5f);
                myWay = newWay;

                coMove = StartCoroutine("CoMove");
                
            }
        }

        if(myWay != null)
        {
            int characterX = grid.GetNodeFromVector(transform.position).myX;
            int goalX = grid.GetNodeFromVector(mousePosition).myX;

            if (characterX < goalX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(characterX > goalX)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

        }
        else
        {
            anim.SetFloat("RunState", 0f);

        }

    }

    IEnumerator CoMove()
    {
        int index = 0;
        targetNode = myWay[0];
        while (true)
        {
            if(transform.position == targetNode.myPos)
            {//웨이포인트까지 도착하면 다음 웨이포인트로 간다
                index++;
                if(index >= myWay.Count)
                {
                    myWay = null;
                    yield break;
                }

                targetNode = myWay[index];
            }
            transform.position = Vector2.MoveTowards(transform.position, targetNode.myPos, Time.deltaTime * speed);
            yield return null;
        }
    }
}
