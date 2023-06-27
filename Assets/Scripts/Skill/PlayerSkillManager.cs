using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillManager : MonoBehaviour
{
    public Image fillSkill;
    public PlayerController playerController;
    private float timeCoolDown;
    void Update()
    {
        if (playerController.isSkill)
        {
            timeCoolDown -= Time.deltaTime;
            UpdateBar(timeCoolDown, playerController.skillCoolDown + playerController.skillTime);
            return;
        }
        timeCoolDown = playerController.skillCoolDown + playerController.skillTime;
    }
    public void UpdateBar(float currentValue, float maxValue)
    {
        fillSkill.fillAmount = currentValue / maxValue;
    }
}
