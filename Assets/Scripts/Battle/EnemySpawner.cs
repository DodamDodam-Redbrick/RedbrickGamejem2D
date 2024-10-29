using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        //스폰 후 받은 개체(enemy 스크립트)에 wayPoint 지정해주고 startmove로 움직이게 해준다.
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
