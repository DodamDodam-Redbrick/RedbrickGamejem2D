using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        //���� �� ���� ��ü(enemy ��ũ��Ʈ)�� wayPoint �������ְ� startmove�� �����̰� ���ش�.
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
