using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePriorityQueue
{
    private List<Node> heap = new List<Node>();

    public int Count => heap.Count;

    public bool Contains(Node node)
    {
        return heap.Contains(node);
    }

    public void Enqueue(Node data)
    {
        heap.Add(data);

        int now = heap.Count - 1;

        while(now > 0)
        {
            int next = (now - 1) / 2;
            if (heap[now].fcost > heap[next].fcost) break;

            Node tmp = heap[now];
            heap[now] = heap[next];
            heap[next] = tmp;

            now = next;
        }
    }

    public Node Dequeue()
    {
        Node ret = heap[0];

        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;

        int now = 0;
        while (true)
        {
            int left = 2 * now + 1;
            int right = 2 * now + 2;
            int next = now;

            if(left<=lastIndex && heap[next].fcost > heap[left].fcost)
                next = left;

            if(right <= lastIndex && heap[next].fcost > heap[right].fcost)
                next = right;

            if (next == now)
                break;

            Node tmp = heap[now];
            heap[now] = heap[next];
            heap[next] = tmp;

            now = next;
        }

        return ret;
    }
}

public static class PathFinding
{
    public static List<Node> PathFind(Vector3 startPos, Vector3 endPos, MapGrid mapGrid)
    {
        NodePriorityQueue openList = new NodePriorityQueue();
        HashSet<Node> closedList = new HashSet<Node>(); //closedList�� ���ԵǾ� �ִ����� Ȯ���ϱ� ������ hashSet���� (�ߺ�����)
        Node startNode = mapGrid.GetNodeFromVector(startPos);
        Node endNode = mapGrid.GetNodeFromVector(endPos);
        openList.Enqueue(startNode);
        while (openList.Count > 0)
        {
            Node curNode = openList.Dequeue(); //���縦 �������� ����
            closedList.Add(curNode);

            if (curNode == endNode) return Retrace(startNode, curNode, mapGrid); //�������� Ž���� �����ϰ� endNode���� ������ Ž�� ����

            foreach(Node neightborNode in mapGrid.SearchNeightborNode(curNode))
            {
                if(neightborNode.canWalk && !closedList.Contains(neightborNode))
                {
                    int x = curNode.myX - neightborNode.myX;
                    int y = curNode.myY - neightborNode.myY;
                    int newCost = curNode.gCost + GetDistance(neightborNode, curNode);
                    //getDistance�� ��� ���� �Ÿ� �� �� ���
                    if(newCost < neightborNode.gCost || !openList.Contains(neightborNode))
                    {//�湮�� ������ �ƴϰų� ���� ���� gCost�� �� ���� ��쿣 gCost�� �ٽ� ����� ������ �����Ѵ�.
                        neightborNode.gCost = newCost;
                        neightborNode.hCost = GetDistance(neightborNode, endNode);
                        neightborNode.parent = curNode;

                        if(!openList.Contains(neightborNode)) openList.Enqueue(neightborNode);
                    }
                }
            }
        }
        return null; //��� ���� Ž���ص� �������� ������ null ��ȯ
    }

    static List<Node> Retrace(Node start, Node end, MapGrid mapGrid)
    { //PathFind���� ���� ��忡 ���� ã�Ƴ��� �� ���� ��Ʈ�� Ȯ�����ִ� �۾�
        List<Node> path = new List<Node>();
        Node curNode = end;
        while(curNode != start)
        { //������ par���� ����ؼ� �湮���ְ� ����Ʈ�� ����
            path.Add(curNode);
            curNode = curNode.parent;
        }
        path.Reverse();
        mapGrid.path = path;
        return path;
    }

    static int GetDistance(Node aNode, Node bNode)
    {
        int x = Mathf.Abs(aNode.myX - bNode.myX);
        int y = Mathf.Abs(aNode.myY - bNode.myY);

        //���밪���� ��� ���ְ�, ���밪�� �� ���� ��ŭ�� �밢������ �̵��ϴ°� �ո����̰�, �ʰ�ġ�� �������� �̵��Ѵ�
        return 14 * ((x > y) ? y : x) + 10 * ((x > y) ? (x - y) : (y - x)); //14�� ��Ʈ2 �ٻ�ġ�� ����
    }
}
