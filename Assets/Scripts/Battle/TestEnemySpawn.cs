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
            SelectMapType(MapType.firstStage_one);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            SelectMapType(MapType.firstStage_two);
        }
    }

    public void SelectMapType(MapType index)
    {
        currentMapType = index;
        UpdateEnemySpawner();
    }

    public void UpdateEnemySpawner()
    {
        int index = (int)currentMapType;
        switch (index)
        {
            case 0:
                enemySpawner.SpawnEnemies(DataManager.enemySpawners[MapType.firstStage_one]);
                break;

            case 1:
                enemySpawner.SpawnEnemies(DataManager.enemySpawners[MapType.firstStage_two]);
                break;

        }
    }
}
