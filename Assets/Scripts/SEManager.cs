using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Armor")]
    [Range(0, 100)] [SerializeField] private float armorVolume = 100f;
    [SerializeField] private AudioClip[] armorClips;

    [Header("Footstep")]
    [Range(0, 100)] [SerializeField] private float runVolume = 100f;
    [SerializeField] private AudioClip[] runRightClips;
    [SerializeField] private AudioClip[] runLeftClips;

    [Header("Other")]
    [Range(0, 100)] [SerializeField] private float jumpVolume = 100f;
    [SerializeField] private AudioClip jump;
    [Range(0, 100)] [SerializeField] private float landVolume = 100f;
    [SerializeField] private AudioClip land;
    [Range(0, 100)] [SerializeField] private float attackVolume = 100f;
    [SerializeField] private AudioClip attack;
    
    // 鳴っている回数を数える
    private int armorCount;
    private int runCount;
    private int jumpCount;
    private int landCount;
    private int attackCount;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // --------------------
    // ランダム再生
    // --------------------
    private void PlayRandom(AudioClip[] clips, float volume)
    {
        if (clips == null || clips.Length == 0)
            return;

        var clip = clips[Random.Range(0, clips.Length)];

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip, volume);
    }

    // 鎧
    public void PlayArmor()
    {
        armorCount++;
        Debug.Log($"[SE] Armor: {armorCount}");
        PlayRandom(armorClips, armorVolume / 100f);
    }

    // 足音
    public void PlayFootstep(bool isRightFoot)
    {
        runCount++;
        Debug.Log($"[SE] Run: {runCount}");
        var clips = isRightFoot ? runRightClips : runLeftClips;
        PlayRandom(clips, runVolume / 100f);
    }

    // 他SE
    public void Play(SEType type)
    {
        switch (type)
        {
            case SEType.Jump:
                jumpCount++;
                Debug.Log($"[SE] Jump: {jumpCount}");
                audioSource.PlayOneShot(jump, jumpVolume / 100f);
                break;

            case SEType.Land:
                landCount++;
                Debug.Log($"[SE] Land: {landCount}");
                audioSource.PlayOneShot(land, landVolume / 100f);
                break;

            case SEType.Attack:
                attackCount++;
                Debug.Log($"[SE] Attack: {attackCount}");
                audioSource.PlayOneShot(attack, attackVolume / 100f);
                break;
        }
    }
}