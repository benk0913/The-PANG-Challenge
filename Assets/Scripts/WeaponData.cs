using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData", order = 2)]
[Serializable]
public class WeaponData : ScriptableObject
{
    public string ProjectilePrefabKey = "KnifeProjectile";

    public float WeaponCooldown = 1f;

    
}