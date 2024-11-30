using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GAD210_Enemy_Base;
using UnityEngine.UI;
using TMPro;

public class StatUI : MonoBehaviour
{

    public TMP_Text playerHealth;
    public TMP_Text enemyHealth;

    PlayerManager playerManager;
    EnemyBase enemyBase;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        enemyBase = FindObjectOfType<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthStats();
    }

    public void UpdateHealthStats()
    {
        float roundedPlayerHealth = Mathf.Round(playerManager.playerHealth);
        playerHealth.text = $"{roundedPlayerHealth}";

        float roundedEnemyHealth = Mathf.Round(enemyBase.enemyHealth);
        enemyHealth.text = $"{roundedEnemyHealth}";
    }
}
