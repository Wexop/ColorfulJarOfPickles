using StaticNetcodeLib;
using Unity.Netcode;
using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

[StaticNetcode]
public class NetworkColorfulJar
{
    [ClientRpc]
    public static void ChangeJarColorClientRpc(ulong networkId, Color color)
    {
        var networkObjects = Object.FindObjectsByType<ColorfulJarOfPicklesScrap>(FindObjectsSortMode.None);

        ColorfulJarOfPicklesScrap colorfulJarOfPickles = null;
        
        foreach (var g in networkObjects)
        {
            if (g.NetworkObjectId == networkId) colorfulJarOfPickles = g;
        }
        
        if (colorfulJarOfPickles != null)
        {
            Debug.Log($"COLOR CHANGE {networkId} SET VALUE {color}");
            colorfulJarOfPickles.ChangeColor(color);
        }
    }
}