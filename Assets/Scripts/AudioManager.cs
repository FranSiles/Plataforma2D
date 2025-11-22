using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource deathEnemy;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource damageEnemyAudio;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    public void PlayDamageSound()
    {
        damageAudio.Play();
    }

    public void PlayDeathSound()
    {
        deathEnemy.Play();
    }
    public void PlayDamageEnemySound()
    {
        damageEnemyAudio.Play();
    }
    public void PlayAttackSound()
    {
        attackAudio.Play();
    }
}
