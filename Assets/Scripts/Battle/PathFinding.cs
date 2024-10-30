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
        HashSet<Node> closedList = new HashSet<Node>(); //closedList는 포함되어 있는지만 확인하기 때문에 hashSet으로 (중복없음)
        Node startNode = mapGrid.GetNodeFromVector(startPos);
        Node endNode = mapGrid.GetNodeFromVector(endPos);
        openList.Enqueue(startNode);
        while (openList.Count > 0)
        {
            Node curNode = openList.Dequeue(); //현재를 시작으로 지정
            closedList.Add(curNode);

            if (curNode == endNode) return Retrace(startNode, curNode, mapGrid); //도착지면 탐색을 종료하고 endNode부터 역방향 탐색 시작

            foreach(Node neightborNode in mapGrid.SearchNeightborNode(curNode))
            {
                if(neightborNode.canWalk && !closedList.Contains(neightborNode))
                {
                    int x = curNode.myX - neightborNode.myX;
                    int y = curNode.myY - neightborNode.myY;
                    int newCost = curNode.gCost + GetDistance(neightborNode, curNode);
                    //getDistance는 노드 사이 거리 잴 때 사용
                    if(newCost < neightborNode.gCost || !openList.Contains(neightborNode))
                    {//방문할 예정이 아니거나 새로 구한 gCost가 더 작을 경우엔 gCost를 다시 계산한 값으로 설정한다.
                        neightborNode.gCost = newCost;
                        neightborNode.hCost = GetDistance(neightborNode, endNode);
                        neightborNode.parent = curNode;

                        if(!openList.Contains(neightborNode)) openList.Enqueue(neightborNode);
                    }
                }
            }
        }
        return null; //모든 길을 탐색해도 목적지가 없으면 null 반환
    }

    static List<Node> Retrace(Node start, Node end, MapGrid mapGrid)
    { //PathFind에서 각각 노드에 길을 찾아놓은 걸 토대로 루트를 확립해주는 작업
        List<Node> path = new List<Node>();
        Node curNode = end;
        while(curNode != start)
        { //각각의 par들을 계속해서 방문해주고 리스트에 넣음
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

        //절대값으로 계산 해주고, 절대값이 더 작은 만큼은 대각선으로 이동하는게 합리적이고, 초과치는 직선으로 이동한다
        return 14 * ((x > y) ? y : x) + 10 * ((x > y) ? (x - y) : (y - x)); //14는 루트2 근사치로 생각
    }
}
