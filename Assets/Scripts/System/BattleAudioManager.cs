using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    private static BattleAudioManager _instance;

    // 💡 Properti publik dengan validasi null check bawaan Unity
    public static BattleAudioManager Instance
    {
        get
        {
            // Operator == null di Unity otomatis mendeteksi jika objek sudah dihancurkan (destroyed)
            if (_instance == null) return null;
            return _instance;
        }
    }

    [Header("Audio Sources Channel")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Looping Gameplay Audio (BGM)")]
    [SerializeField] private AudioClip explorationBgm;
    [SerializeField] private AudioClip battleBgm;

    [Header("One-Shot Gameplay SFX")]
    [SerializeField] private AudioClip uiButtonClickClip;
    [SerializeField] private AudioClip uiButtonCancelClip;
    [SerializeField] private AudioClip playerStepClip;
    [SerializeField] private AudioClip basicAttackClip;
    [SerializeField] private AudioClip heavyAttackClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip encounterClip;

    private void Awake()
    {
        // Konfigurasi Singleton yang aman
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Paksa konfigurasi optimal untuk kompatibilitas WebGL
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void Start()
    {
        // Memulai game dengan musik eksplorasi
        PlayExplorationBGM();
    }

    // ================= LOOPING BGM CONTROLLERS =================

    public void PlayExplorationBGM() => SwitchBGM(explorationBgm);
    public void PlayBattleBGM() => SwitchBGM(battleBgm);

    private void SwitchBGM(AudioClip targetBgm)
    {
        if (bgmSource.clip == targetBgm) return;

        bgmSource.Stop();
        bgmSource.clip = targetBgm;
        bgmSource.Play();
    }

    // ================= ONE-SHOT SFX CONTROLLERS =================

    public void PlayUIClickSFX() => sfxSource.PlayOneShot(uiButtonClickClip);
    public void CancelUIClickSFX() => sfxSource.PlayOneShot(uiButtonCancelClip);
    public void PlayStepSFX() => sfxSource.PlayOneShot(playerStepClip);
    public void PlayTakeDamageSFX() => sfxSource.PlayOneShot(takeDamageClip);
    public void PlayDeathSFX() => sfxSource.PlayOneShot(deathClip);
    public void PlayDashSFX() => sfxSource.PlayOneShot(dashClip);
    public void PlayEncounterSFX() => sfxSource.PlayOneShot(encounterClip);

    public void PlayActionSFX(PlayerActionType actionType)
    {
        switch (actionType)
        {
            case PlayerActionType.BasicAttack:
                sfxSource.PlayOneShot(basicAttackClip);
                break;
            case PlayerActionType.HeavyAttack:
                sfxSource.PlayOneShot(heavyAttackClip);
                break;
            case PlayerActionType.Heal:
                sfxSource.PlayOneShot(healClip);
                break;
        }
    }
}