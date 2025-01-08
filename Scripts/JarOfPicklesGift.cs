using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ColorfulJarOfPickles.Scripts;

public class JarOfPicklesGift: NetworkBehaviour
{
    public ColorfulJarOfPicklesScrap scrap;
    
    public ParticleSystem PoofParticle;
    public AudioSource presentAudio;
    public AudioClip openGiftAudio;
    private GameObject objectInPresent;
    private bool hasUsedGift;
    public bool isRainbow;

    public void Start()
    {
        if (!isRainbow)
        {
            var gameObjetsList = ColorfulJarOfPicklesPlugin.instance.ColorfulJarOfPicklesGameObjects;
            objectInPresent = gameObjetsList[Random.RandomRangeInt(0, gameObjetsList.Count)];
        }
        else
        {
            var gameObjetsList = ColorfulJarOfPicklesPlugin.instance.RainbowColorfulJarOfPicklesGameObjects;
            objectInPresent = gameObjetsList[Random.RandomRangeInt(0, gameObjetsList.Count)];
        }
    }


    public void OnActiveItem()
    {
        if(hasUsedGift) return;
        if(scrap.playerHeldBy) scrap.playerHeldBy.DiscardHeldObject();
        SpawnItemServerRpc();
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemServerRpc()
    {
        hasUsedGift = true;
        Vector3 vector3 = Vector3.zero;

        Transform parent = (!((UnityEngine.Object) scrap.playerHeldBy != (UnityEngine.Object) null) || !scrap.playerHeldBy.isInElevator) && !StartOfRound.Instance.inShipPhase || !((UnityEngine.Object) RoundManager.Instance.spawnedScrapContainer != (UnityEngine.Object) null) ? StartOfRound.Instance.elevatorTransform : RoundManager.Instance.spawnedScrapContainer;
        vector3 = transform.position + Vector3.up * 0.25f;
        GameObject gameObject = Instantiate(objectInPresent, vector3, Quaternion.identity, parent);
        gameObject.GetComponent<NetworkObject>().Spawn();
        var colorful = gameObject.GetComponent<ColorfulJarOfPicklesScrap>();
        colorful.SetValueClientRpc(Mathf.RoundToInt(Random.Range(colorful.itemProperties.minValue + 25, colorful.itemProperties.maxValue + 35 ) * RoundManager.Instance.scrapValueMultiplier));
        SpawnItemClientRpc();
    }

    [ClientRpc]
    public void SpawnItemClientRpc()
    {
        hasUsedGift = true;
        presentAudio.PlayOneShot(openGiftAudio);
        PoofParticle.Play();
        if (IsServer) StartCoroutine(DespawnCoroutine());
    }

    private IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        if(IsServer) NetworkObject.Despawn();
    }
}