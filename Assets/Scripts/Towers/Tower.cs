using System.Collections;
using System.IO;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Tower : MonoBehaviour// Общие пораметры всех башен
{
    [SerializeField] protected TowerParameters _parameters;

    protected float _attackDistance;
    protected float _attackSpeed;
    protected float _damage;

    public enum DamageTypes
    {
        Physical, Magic, True
    }
    public enum TowerTypes
    {
        SimpleBallist, DoubleBallist, BigBallist, PoisonBallist, SniperBallist,
        SimpleMage, IceMage, GodMage, FireMage, DeathMage,
        SimpleMortar, FireMortar, RoketMortar, ShrapnelMortar
    }

    protected DamageTypes _damageType;
    protected TowerTypes _towerType;

    protected GameObject _distancePrefab;

    [SerializeField] protected Transform _attackPoint; // Точка появления снаряда

    private void Awake()
    {
        _distancePrefab = gameObject.GetNamedChild("Distance");

        _damage = _parameters.Damage_1;
        _attackSpeed = _parameters.AttackSpeed_1;
        _attackDistance = _parameters.AttackDistance_1;

        _damageType = _parameters.DamageType;
        _towerType = _parameters.TowerType;
    }
}

public interface TowerFunctions // Общие функции башен
{
    public IEnumerator Attack(); // Атака башни 
    public IEnumerator SearchTarget(); // Поиск цели
    public void LevelUp(); // Увеничение параметров при повышении уровня
    public TowerParameters GetParameters();
    public int PriceLevelUp { get; set; }
    public int Towerlevel { get; set; }
    public bool isAttack { get; set; }
    public GameObject gm { get; set; }
}
