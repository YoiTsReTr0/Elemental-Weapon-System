using System.Collections.Generic;
using UnityEngine;
using Elemental.Main;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elemental.WeaponSystem
{
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Create New Weapon")]
    public class WeaponTypeSO : ScriptableObject
    {
        public WeaponType WeaponType;
        public WeaponPowerType WeaponPowerType;
        public List<WeaponSlotType> WeaponSlotTypes;

        public string WeaponName;

        public Sprite WeaponImage;

        #region If Weapon is not melee

        public ProjectileType ProjectileType;

        public int MagezineSize;
        public float FireRate;
        public float ReloadTime;
        public float AttackRange;

        public int ProjectilesSpawnCount;
        public float DamagePerProjectile;
        public float AreaOfImpactRadius;
        public float ProjectileSpeed;

        public WeaponFireMode WeaponFireMode;

        #endregion

        #region If Weapon is melee

        public float HitRate;
        public float DamagePerHit;

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(WeaponTypeSO))]
    public class WeaponTypeSOEditor : Editor
    {
        #region Serialize Properties

        SerializedProperty _weaponType;
        SerializedProperty _weaponPowerType;

        SerializedProperty _weaponSlotTypes;

        SerializedProperty _weaponName;

        SerializedProperty _weaponImage;

        #region !Melee Vars

        SerializedProperty _projectileType;

        SerializedProperty _magezineSize;
        SerializedProperty _fireRate;
        SerializedProperty _reloadTime;
        SerializedProperty _attackRange;

        SerializedProperty _projectilesSpawn;
        SerializedProperty _damagePerProjectile;
        SerializedProperty _areaOfImpactRadius;
        SerializedProperty _projectileSpeed;

        SerializedProperty _weaponFireMode;

        #endregion

        #region Melee Vars

        SerializedProperty _hitRate;
        SerializedProperty _hitRange;
        SerializedProperty _damagePerHit;

        #endregion

        #endregion

        private void OnEnable()
        {
            _weaponType = serializedObject.FindProperty("WeaponType");
            _weaponPowerType = serializedObject.FindProperty("WeaponPowerType");
            _weaponSlotTypes = serializedObject.FindProperty("WeaponSlotTypes");
            _weaponName = serializedObject.FindProperty("WeaponName");
            _weaponImage = serializedObject.FindProperty("WeaponImage");

            _projectileType = serializedObject.FindProperty("ProjectileType");

            _magezineSize = serializedObject.FindProperty("MagezineSize");
            _fireRate = serializedObject.FindProperty("FireRate");
            _reloadTime = serializedObject.FindProperty("ReloadTime");
            _attackRange = serializedObject.FindProperty("AttackRange");

            _projectilesSpawn = serializedObject.FindProperty("ProjectilesSpawnCount");
            _damagePerProjectile = serializedObject.FindProperty("DamagePerProjectile");
            _areaOfImpactRadius = serializedObject.FindProperty("AreaOfImpactRadius");
            _projectileSpeed = serializedObject.FindProperty("ProjectileSpeed");

            _weaponFireMode = serializedObject.FindProperty("WeaponFireMode");

            _hitRate = serializedObject.FindProperty("HitRate");
            _damagePerHit = serializedObject.FindProperty("DamagePerHit");
        }

        public override void OnInspectorGUI()
        {
            WeaponTypeSO scriptableObject = (WeaponTypeSO)target;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_weaponType);
            EditorGUILayout.PropertyField(_weaponPowerType);
            EditorGUILayout.PropertyField(_weaponSlotTypes);
            EditorGUILayout.PropertyField(_weaponName);
            EditorGUILayout.PropertyField(_weaponImage);

            if (scriptableObject.WeaponType == WeaponType.DefaultMelee)
            {
                EditorGUILayout.PropertyField(_hitRate);
                EditorGUILayout.PropertyField(_damagePerHit);
            }

            else
            {
                EditorGUILayout.PropertyField(_weaponFireMode);
                EditorGUILayout.PropertyField(_projectileType);

                EditorGUILayout.PropertyField(_magezineSize);
                EditorGUILayout.PropertyField(_fireRate);
                EditorGUILayout.PropertyField(_reloadTime);
                EditorGUILayout.PropertyField(_attackRange);

                EditorGUILayout.PropertyField(_projectilesSpawn);
                EditorGUILayout.PropertyField(_damagePerProjectile);
                EditorGUILayout.PropertyField(_areaOfImpactRadius);
                EditorGUILayout.PropertyField(_projectileSpeed);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}