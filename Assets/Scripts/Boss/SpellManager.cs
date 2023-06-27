using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public BoxCollider2D boxCollider2d;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >=1.4f) boxCollider2d.enabled = true;
        if (time >= 1.9f) Destroy(gameObject);
    }
}
