using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Room moveRoom;
    public Room MoveRoom
    {
        get { return moveRoom; }
        set
        {
            moveRoom = value;
        }
    }

    public void OnClickArrow()
    {
        GameSystem.Instance.MyPlayer.MoveRoom(moveRoom);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
