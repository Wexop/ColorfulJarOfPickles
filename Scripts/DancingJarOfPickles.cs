using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

public class DancingJarOfPickles : ColorfulJarOfPicklesScrap
{
    private static readonly int Playing = Animator.StringToHash("playing");


    public Animator animator;
    public AudioClip happySong;
    
    public AudioSource audioSource;
    
    private bool isPlaying;

    public IEnumerator onPlayingSong()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        isPlaying = false;
        animator.SetBool(Playing, false);
    }

    public void TriggerDance(bool dance)
    {
        Debug.Log($"TriggerDance {dance}");
        animator.SetBool(Playing, dance);

        if (dance)
        {
            audioSource.volume = ColorfulJarOfPicklesPlugin.instance.dancingMusicVolume.Value;
            audioSource.PlayOneShot(happySong);
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        StartCoroutine(onPlayingSong());
    }

    public override void ItemActivate(bool used, bool buttonDown = true)
    {
        base.ItemActivate(used, buttonDown);
        
        Debug.Log("Activate item");
        SetIsPlaying(!isPlaying);
        
    }

    private void SetIsPlaying(bool value)
    {
        
        SetIsPlayingServerRpc(value);
        SetIsPlayingOnLocalClient(value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetIsPlayingServerRpc(bool value, ServerRpcParams serverRpcParams = default)
    {
        var senderClientId = serverRpcParams.Receive.SenderClientId;
        if (!NetworkManager.ConnectedClients.ContainsKey(senderClientId)) return;

        SetIsPlayingClientRpc(value, senderClientId);
    }

    [ClientRpc]
    private void SetIsPlayingClientRpc(bool value, ulong senderClientId)
    {
        if (NetworkManager.Singleton.LocalClientId == senderClientId) return;

        SetIsPlayingOnLocalClient(value);
    }

    private void SetIsPlayingOnLocalClient(bool value)
    {
        isPlaying = value;
        TriggerDance(value);
    }

}