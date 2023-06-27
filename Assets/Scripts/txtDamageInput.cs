using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class txtDamageInput : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }
    public void SetText(int damage)
    {
        GetComponent<TextMesh>().text = "-" + damage;
    }
}
