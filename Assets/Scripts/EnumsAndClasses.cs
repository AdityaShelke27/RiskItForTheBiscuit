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
    public short armAngle;
    public bool didShoot;
    public bool isRight;

    public RecordFrame(Vector2 _pos, short _armAngle, bool _didShoot, bool _isRight)
    {
        pos = _pos;
        armAngle = _armAngle;
        didShoot = _didShoot;
        isRight = _isRight;
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