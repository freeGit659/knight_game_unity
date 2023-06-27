using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDropManager : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected GameObject MP;
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
            gameManager.TakedSkill();
            MP.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
