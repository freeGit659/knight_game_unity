using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject SmallEnemy;
    public float spawnTime = 3;
    float m_spawnTime;
    public Transform Enemy;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnTime -= Time.deltaTime;
        if (m_spawnTime <= 0 && num < 1)
        {
            SpawnSmallEnemy();
            num++;
            m_spawnTime = spawnTime;
        }
        
    }
    public void SpawnSmallEnemy()
    {
        float randXPos = Random.Range(1f, 8f);
        Vector2 spawnEnemyPos = new Vector2(randXPos, -4.18f);
        if (SmallEnemy)
        {
            Instantiate(SmallEnemy, spawnEnemyPos, Quaternion.identity, Enemy);
        }
    }
}
