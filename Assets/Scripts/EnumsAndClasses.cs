using UnityEngine;
using System;
public enum EnemyState
{
    None,
    Chase,
    Attack,
    Die
}
public enum EnemyType
{
    Chaser,
    Shooter
}
[Serializable]
public class Wave
{
    public EnemySpawnPointPair[] enemies;
    public float timeInterval;
}
[Serializable]
public struct EnemySpawnPointPair
{
    public EnemyType enemyType;
    public int spawnIdx;
}