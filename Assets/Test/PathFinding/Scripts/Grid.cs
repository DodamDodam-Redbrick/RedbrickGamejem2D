using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 worldSize; //게임 상 맵 크기
    public float nodeSize; //노드의 크기
    [SerializeField]Node[,] myNode; //전체 노드를 관리할 배열
    int nodeCountX; //노드의 X방향 갯수
    int nodeCountY; //노드의 Y방향 갯수
    [SerializeField] LayerMask obstacle; //장애물인지 구분할 LayerMask
    public List<Node> path; // 예상 경로
 
    int[,] dir = new int[4, 2]
    {
        { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }
    }; //탐색 가능한 방향 (현재는 4방향)

    private void Start()
    {
        nodeCountX = Mathf.CeilToInt(worldSize.x / nodeSize); //노드의 크기에 따라서 개수가 달라진다
        //노드가 커지면 개수가 작고, 정확도는 낮지만 계산이 빨라짐
        nodeCountY = Mathf.CeilToInt(worldSize.y / nodeSize);

        CreateGrid();
    }

    void CreateGrid()
    {
        myNode = new Node[nodeCountX, nodeCountY];
        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountY; j++)
            {
                Vector3 pos = new Vector3(i * nodeSize, j * nodeSize); //노드의 좌표
                Collider2D hit = Physics2D.OverlapBox(pos, new Vector2(nodeSize / 2, nodeSize / 2), 0, obstacle);
                myNode[i, j] = new Node(hit == null, pos, i, j);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, worldSize.y, 1));
        if (myNode != null)
        {
            foreach(Node no in myNode)
            {
                Gizmos.color = (no.canWalk) ? Color.white : Color.red;

                if(path != null && path.Contains(no)) Gizmos.color = Color.black;

                Gizmos.DrawCube(no.myPos, Vector3.one * (nodeSize / 2));
            }

        }
    }

    public bool IsOutOfBind(Vector3 position)
    {
        if (position.x >= 0 && position.y >= 0 && position.x < nodeCountX && position.y < nodeCountY) return true;
        return false;
    }

    public List<Node> SearchNeightborNode(Node node)
    {
        List<Node> nodeList = new List<Node>();
        Vector3 newVec = new Vector3();

        for(int i=0; i<dir.GetLength(0); i++)
        {
            newVec.x = node.myX + dir[i, 0];
            newVec.y = node.myY + dir[i, 1];

            if (IsOutOfBind(newVec)) nodeList.Add(myNode[(int)newVec.x, (int)newVec.y]);
        }

        return nodeList;
    }

    public Node GetNodeFromVector(Vector3 vector)
    {
        int posX = Mathf.RoundToInt(vector.x / nodeSize);
        int posY = Mathf.RoundToInt(vector.y / nodeSize);
        return myNode[posX, posY];
    }
}
