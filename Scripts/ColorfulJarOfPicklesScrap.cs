using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace ColorfulJarOfPickles.Scripts;

public class ColorfulJarOfPicklesScrap : PhysicsProp
{
    public GameObject jarGameObject;
    public List<GameObject> picklesGameObjects;
    public Light lightObject;

    public List<Renderer> jarRenderers;

    public Color actualColor;

    public UnityEvent<bool> onTriggerDance;
    public UnityEvent onGrabItem;

    public virtual void TriggerDance(bool dance)
    {
        if (onTriggerDance != null)
        {
            Debug.Log($"INVOKE DANCE {dance}");
            onTriggerDance.Invoke(dance);
        }
    }

    public override void GrabItem()
    {
        
        if (onGrabItem != null)
        {
            onGrabItem.Invoke();
        }

    }
    
    public void ChangeColor(Color color)
    {
        lightObject.color = color;
        jarRenderers.ForEach(r =>
        {
            r.material.color = SetColorAlpha(color, r.material.color.a);
            if (r.material.name.Contains("Pickle"))
            {
                r.material.SetColor("_EmissiveColor", color);
            }
        });
        actualColor = color;
    }
    
    public float RandomZeroToOne()
    {
        return Random.Range(0f, 1f);
    }
    
    public float RandomLightColorFloat()
    {
        return Random.Range(0.75f, 1f);
    }

    public Color SetColorAlpha(Color color, float alpha)
    {
        return new Color(color[0], color[1], color[2], alpha);
    }
    
    public Color GetRandomColor(float initialAlpha = 1f)
    {

        var baseColor = new Color(RandomZeroToOne(),
            RandomZeroToOne(), RandomZeroToOne(), initialAlpha);

        baseColor[Random.Range(0, 4)] = RandomLightColorFloat();

        return baseColor;
    }
    public override void Start()
    {
        base.Start();
        picklesGameObjects.ForEach(p =>
        {
            jarRenderers.Add(p.GetComponent<Renderer>());
        });
        
        
        var jarRendersFound = jarGameObject.GetComponents<Renderer>().ToList();
        jarRendersFound.ForEach(o =>
        {
            if (o.material.name.Contains("JarGlass"))
            {
                jarRenderers.Add(o);
            }
        });
        
        ChangeColor(GetRandomColor());
        
        if (IsServer)
        {
            StartCoroutine(ChangeColorCoroutine());
        }
        else
        {
            StartCoroutine(AskColorCoroutine());
        }

    }

    public IEnumerator ChangeColorCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        NetworkColorfulJar.ChangeJarColorClientRpc(NetworkObjectId, GetRandomColor());
        
 
    }

    public IEnumerator AskColorCoroutine()
    {
        yield return new WaitForSeconds(1f);
        
        NetworkColorfulJar.AskColorServerRpc(NetworkObjectId);
        
 
    }
}