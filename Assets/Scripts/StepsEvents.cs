using UnityEngine;

public class StepsEvents : MonoBehaviour
{
    private AudioManager audioManager;

    [SerializeField]
    private AudioClip farawayStep1, farawayStep2, farawayStep3;
    [SerializeField]
    private AudioClip rushStep;

    void Awake()
    {
        audioManager = Object.FindFirstObjectByType<AudioManager>();
    }

    public void Step()
    {
        int type = Random.Range(1, 15);
        switch (type)
        {
            case 1:
                audioManager.PlaySFX(farawayStep1, transform.position, Random.Range(0.5f, 0.65f));
                break;
            case 2:
                audioManager.PlaySFX(farawayStep2, transform.position, Random.Range(0.5f, 0.65f));
                break;
            case 3:
                audioManager.PlaySFX(farawayStep3, transform.position, Random.Range(0.5f, 0.65f));
                break;
        }
    }
}
