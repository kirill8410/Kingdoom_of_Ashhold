using System;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class Calculator : MonoBehaviour
{
    [Header("Ввод")]
    [SerializeField] float damage;
    [SerializeField] float attackSpeed;
    [SerializeField] float distance;
    [SerializeField] float goblinHP;
    [SerializeField] float goblinDefence;
    [SerializeField] float goblinShild;
    [SerializeField] float goblinSpeed;


    [Space]
    [Header("Вывод")]
    [SerializeField] float dps;
    [SerializeField] float timeGoblinInDistance;
    [SerializeField] float attackPerGoblin;
    [SerializeField] float damagePerGoblin;
    [SerializeField] float lastGoblinHP;
    [SerializeField] float timeKilled;
    [SerializeField] float attackKilled;
    [SerializeField] float goblinMove;

    private void Update()
    {
        dps = damage * attackSpeed;
        timeGoblinInDistance = (distance * 1.9f) / goblinSpeed;
        attackPerGoblin = Convert.ToInt32(attackSpeed * timeGoblinInDistance);
        damagePerGoblin = (attackPerGoblin - goblinShild) * (damage - goblinDefence);
        if (damagePerGoblin < 0)
        {
            damagePerGoblin = 0;
        }
        if (damagePerGoblin > goblinHP)
        {
            lastGoblinHP = 0;
        }
        else
        {
            lastGoblinHP = goblinHP - damagePerGoblin;
        }
        float hp = goblinHP;
        float shild = goblinShild;
        float tk = 0;
        float gm = 0;
        for (float i = 1; i <= attackPerGoblin;  i++)
        {
            if (shild > 0)
            {
                shild -= 1;
            }
            else
            {
                if (damage - goblinDefence > 0)
                {
                    hp -= damage - goblinDefence;
                }
            }
            tk = i / attackSpeed;
            gm = tk * goblinSpeed;
            if (hp <= 0)
            {
                attackKilled = i;
                timeKilled = tk;
                goblinMove = (gm / (distance * 1.9f)) * 100f;
                break;
            }
            else
            {
                attackKilled = 0;
                timeKilled = 0;
                goblinMove = 100;
            }
        }
    }
}
