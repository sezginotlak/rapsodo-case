using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCCanvas : MonoBehaviour
{
    public Image healthFillImage;

    public void UpdateHealthBar(float health)
    {
        healthFillImage.fillAmount = health;
    }
}
