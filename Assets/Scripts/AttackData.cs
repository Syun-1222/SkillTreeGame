using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    public string attackName;
    public Vector2 startSize;
    public Vector2 maxSize;
    public Vector3 offset;

    public float startTime;
    public float maxTime;

    public int damage;
}