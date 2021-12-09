using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    public float currentHealth;
    private float maxHealth = 6;
    Lvl20BF lvl20BF;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        lvl20BF = FindObjectOfType<Lvl20BF>();
    }

    private void FixedUpdate()
    {
        currentHealth = lvl20BF.hits + 1;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
