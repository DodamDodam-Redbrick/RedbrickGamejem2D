using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerCharacter
{
    firstCharacter,
    secondCharacter,
}

public class Player : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public PlayerCharacter playerCharacter = PlayerCharacter.firstCharacter;

    Coroutine coMoveRoom;
    private void Start()
    {
        DontDestroyOnLoad(this);

        GameSystem.Instance.MyPlayer = this;
    }

    public void MoveRoom(Room room)
    {
        if(coMoveRoom != null)
        {
            StopCoroutine(coMoveRoom);
            coMoveRoom = null;
        }

        coMoveRoom = StartCoroutine(CoMoveRoom(room));
    }

    IEnumerator CoMoveRoom(Room room)
    {
        Vector3 firstPosition = transform.position;
        float runtime = 0f;
        float duration = GameSystem.Instance.MapMoveTime;

        //�ִϸ��̼� �޸��� ���

        while (runtime <= duration)
        {
            runtime += Time.deltaTime;
            transform.position = Vector2.Lerp(firstPosition, room.transform.position, runtime/duration);
            yield return null;
        }

        coMoveRoom = null;
        //�ִϸ��̼� �����ִ� ���
    }
}
