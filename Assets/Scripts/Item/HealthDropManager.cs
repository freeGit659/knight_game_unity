using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropManager : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    public Vector3 health;

    // Start is called before the first frame update
    void Start()
    {
        health = gameManager.keyPosition;
        transform.Translate(gameManager.keyPosition);
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.TakedHealth(10, 5f);
            gameObject.SetActive(false);
        }
    }
}
