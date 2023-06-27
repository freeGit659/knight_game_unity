using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    public Image fillHPBar;
    public Image fillMPBar;

    // Start is called before the first frame update

    public void UpdateBar(string taget, int currentValue, int maxValue)
    {
        if (taget == "HP") fillHPBar.fillAmount = (float)currentValue / (float)maxValue;
        if (taget == "MP")  fillMPBar.fillAmount = (float)currentValue / (float)maxValue;
    }
}
