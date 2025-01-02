/* using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

public class NetworkColorfulJar
{

    private static ColorfulJarOfPicklesScrap GetPickles(ulong NetworkId)
    {
        var networkObjects = Object.FindObjectsByType<ColorfulJarOfPicklesScrap>(FindObjectsSortMode.None).ToList();

        ColorfulJarOfPicklesScrap colorfulJarOfPickles = null;
        
        foreach (var g in networkObjects)
        {
            if (g.NetworkObjectId == NetworkId) colorfulJarOfPickles = g;
        }
        
        if(colorfulJarOfPickles == null) Debug.LogError($"ColorfulJarOfPickles not found, network id : {NetworkId}");
        
        return colorfulJarOfPickles;
    }
    
    [ServerRpc]
    public static void AskColorServerRpc(ulong networkId)
    {
        ColorfulJarOfPicklesScrap colorfulJarOfPickles = GetPickles(networkId);

        ChangeJarColorClientRpc(networkId, colorfulJarOfPickles.actualColor);
    }
    
    [ClientRpc]
    public static void ChangeJarColorClientRpc(ulong networkId, Color color)
    {

        ColorfulJarOfPicklesScrap colorfulJarOfPickles = GetPickles(networkId);
        
        if (colorfulJarOfPickles != null)
        {
            Debug.Log($"COLOR CHANGE {networkId} SET VALUE {color}");
            colorfulJarOfPickles.ChangeColor(color);
        }
    }
    
        
    [ServerRpc]
    public static void DancePicklesServerRpc(ulong networkId, bool danse)
    {

        DancePicklesClientRpc(networkId, danse);
    }
    
    [ClientRpc]
    public static void DancePicklesClientRpc(ulong networkId, bool dance)
    {

        ColorfulJarOfPicklesScrap colorfulJarOfPickles = GetPickles(networkId);
        
        if (colorfulJarOfPickles != null)
        {
            colorfulJarOfPickles.TriggerDance(dance);
        }
    }

} */