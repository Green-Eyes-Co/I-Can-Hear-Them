using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int TotalItems = 0;
    public int CurrentItems = 0;

    [SerializeField] GameObject winScreen;
    [SerializeField] AudioClip itemPickupSound;

    AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ItemGot()
    {
        CurrentItems++;
        audioSource.PlayOneShot(itemPickupSound);
        if (CurrentItems >= TotalItems)
        {
            winScreen.SetActive(true);
        }
    }
}
