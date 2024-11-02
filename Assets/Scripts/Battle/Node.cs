using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    wall,
    lowRoad,
    highRoad,
    onlyWalk,
}

public class Node
{
    public NodeType type;

    public bool isUse; //유닛 놓을 때 true로 해주고 유닛 죽거나 빼면 false로 해주기

    public bool canWalk;
    public Vector3 myPos;
    public int myX;
    public int myY;
    public int gCost;
    public int hCost;

    public Node parent;
    public Node(bool walk, Vector3 pos, int X, int Y, NodeType type)
    {
        canWalk = walk;
        myPos = pos;
        myX = X;
        myY = Y;
        this.type = type;
    }

    public int fcost
    {
        get { return gCost + hCost; }
    }
}
