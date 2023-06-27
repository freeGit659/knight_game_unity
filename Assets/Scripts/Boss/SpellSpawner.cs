using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    public GameObject Spell;
    public BossManager bossManager;
    public Transform player;
    public Transform SpellParent;
    public int num;
    // Start is called before the first frame update
    void OnEnable()
    {
        num = bossManager.numSpell;
        SpawnSpell();
    }
    public void SpawnSpell()
    {
        for (int i = 0; i <= num; i++)
        {
            float playerPosx = player.position.x;
            float randXPos = Random.Range(playerPosx - 10f, playerPosx + 10f);
            Vector2 spawnEnemyPos = new Vector2(randXPos, -3.1f);
            if (Spell)
            {
                Instantiate(Spell, spawnEnemyPos, Quaternion.identity, SpellParent);
            }
            i++;
        }

    }
}
