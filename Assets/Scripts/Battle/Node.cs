using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    low,
    high,
    nothing,
}

public class Node
{
    public NodeType type;

    public bool isUse; //���� ���� �� true�� ���ְ� ���� �װų� ���� false�� ���ֱ�

    public bool canWalk;
    public Vector3 myPos;
    public int myX;
    public int myY;
    public int gCost;
    public int hCost;

    public Node parent;
    public Node(bool walk, Vector3 pos, int X, int Y)
    {
        canWalk = walk;
        myPos = pos;
        myX = X;
        myY = Y;
    }

    public int fcost
    {
        get { return gCost + hCost; }
    }
}