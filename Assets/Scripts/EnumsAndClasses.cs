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
public class RecordFrame
{
    public Vector2 pos;
    public float armAngle;
    public bool didShoot;

    public RecordFrame(Vector2 _pos, float _armAngle, bool _didShoot)
    {
        pos = _pos;
        armAngle = _armAngle;
        didShoot = _didShoot;
    }
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