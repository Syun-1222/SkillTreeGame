using UnityEngine;

public enum MotionDirectionType
{
    Forward,
    Backward
}

[CreateAssetMenu(menuName = "Motion/Motion Data")]
public class MotionPreviewData : ScriptableObject
{
    [Header("Animation")]
    public AnimationClip clip;

    [Header("Timing")]
    public float duration = 0.3f;

    [Header("Movement")]
    public float distanceX = 5f;
    public float distanceY = 0f;

    [Header("Input Lock")]
    public float inputLockTime = 0.2f;

    [Header("Motion Curve")]
    public AnimationCurve curveX = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve curveY = AnimationCurve.Linear(0, 0, 1, 0);

    [Header("Direction")]
    public MotionDirectionType directionType = MotionDirectionType.Forward;
}