using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToturialManager : MonoBehaviour
{
    [SerializeField] private SmallEnemyManager[] smallEnemyManager;
    // Start is called before the first frame update
    void Update()
    {
        for (int i = 0; i < smallEnemyManager.Length; i++)
        {
            if (smallEnemyManager[i].healhtPoint == 0) gameObject.SetActive(false);
        }
    }
}
