using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    PlayerController playerCtr;
    // Start is called before the first frame update
    void Start()
    {
        playerCtr = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
