using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected GameObject gateCave;
    [SerializeField] protected GameObject ActiveHealthBoss;
    [SerializeField] protected GameObject ActiveHeadBoss;
    public Vector3 key;

    // Start is called before the first frame update
    void Start()
    {
        key = gameManager.keyPosition;
        transform.Translate(gameManager.keyPosition);
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.TakedKey();
            gateCave.SetActive(false);
            ActiveHealthBoss.SetActive(true);
            ActiveHeadBoss.SetActive(true);
            gameObject.SetActive(false);
            gameManager.SetCamera(2);
        }
    }
}
