using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawn : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    MapType currentMapType;
    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            {
            SelectMapType(2);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            SelectMapType(1);
        }
    }

    public void SelectMapType(int index)
    {
        if (index == 2)
        {
            currentMapType = MapType.stage_two;
            UpdateEnemySpawner();
        }
        if(index == 1)
        {
            currentMapType = MapType.stage_one;
            UpdateEnemySpawner();
        }
      
    }
    public void UpdateEnemySpawner()
    {
        int index = (int)currentMapType;
        switch (index)
        {
            case 0:
                enemySpawner.SpawnEnemies(DataManager.enemySpawners[MapType.stage_one]);
                break;

            case 1:
                enemySpawner.SpawnEnemies(DataManager.enemySpawners[MapType.stage_two]);
                break;

        }
    }
}
