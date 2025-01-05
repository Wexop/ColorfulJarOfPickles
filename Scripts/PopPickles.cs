using Unity.Netcode;
using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

public class PopPickles: NetworkBehaviour
{
    private static readonly int Pop = Animator.StringToHash("pop");

    public AudioClip popSound;
    public AudioSource audioSource;
    public Animator animator;

    private float _cooldown;
    
    public void OnItemActivate()
    {
        if (_cooldown <= 0)
        {
            PlayPopSoundServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayPopSoundServerRpc()
    {
        if (_cooldown <= 0)
        {
            PlayPopSoundClientRpc();
        }
    }

    [ClientRpc]
    public void PlayPopSoundClientRpc()
    {
        _cooldown = 1f;
        PlayPop();
    }

    private void PlayPop()
    {
        audioSource.PlayOneShot(popSound);
        animator.SetTrigger(Pop);
    }

    public void Update()
    {
        _cooldown -= Time.deltaTime;
    }
}