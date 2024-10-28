using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 worldSize; //���� �� �� ũ��
    public float nodeSize; //����� ũ��
    [SerializeField]Node[,] myNode; //��ü ��带 ������ �迭
    int nodeCountX; //����� X���� ����
    int nodeCountY; //����� Y���� ����
    [SerializeField] LayerMask obstacle; //��ֹ����� ������ LayerMask
    public List<Node> path; // ���� ���
 
    int[,] dir = new int[4, 2]
    {
        { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }
    }; //Ž�� ������ ���� (����� 4����)

    private void Start()
    {
        nodeCountX = Mathf.CeilToInt(worldSize.x / nodeSize); //����� ũ�⿡ ���� ������ �޶�����
        //��尡 Ŀ���� ������ �۰�, ��Ȯ���� ������ ����� ������
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
                Vector3 pos = new Vector3(i * nodeSize, j * nodeSize); //����� ��ǥ
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
