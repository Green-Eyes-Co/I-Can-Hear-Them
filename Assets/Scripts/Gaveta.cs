using UnityEngine;

public class Gaveta : BaseInteractable
{
    bool IsOpen;
    Animator anim;
    
    AudioSource audioSource;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public override string GetMouseOverText()
    {
        return IsOpen ? "Press E to close" : "Press E to open";
    }

    public override string Interact()
    {
        anim.SetTrigger("Interact");
        anim.SetBool("IsOpen", IsOpen);

        audioSource.PlayOneShot(IsOpen ? closeSound : openSound);

        IsOpen = !IsOpen;
        return null;
    }
}
