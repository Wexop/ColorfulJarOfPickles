using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

namespace ColorfulJarOfPickles.Scripts;

public class RainbowColorfulJarOfPickles : ColorfulJarOfPicklesScrap
{
    private Color nextColor;
    private float lightSpeed = 1.5f;
    private float lightSpeedTimer;
    private int seed;
    private Random random = new Random();
    private Color lastColorSaved;

    public override void Start()
    {
        base.Start();
        seed = random.Next(0, 1000);
        SetRandomServerRpc(seed);
        lastColorSaved = GetRandomColor();
        nextColor = GetRandomColor();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetRandomServerRpc(int sharedSeed)
    {
        SetRandomClientRpc(sharedSeed);
    }

    [ClientRpc]
    public void SetRandomClientRpc(int sharedSeed)
    {
        SetRandomLocalClient(sharedSeed);
    }

    private void SetRandomLocalClient(int sharedSeed)
    {
        if(sharedSeed == seed) return;
        
        seed = sharedSeed;
        random = new Random(seed);
    }

    public override Color GetRandomColor(float initialAlpha = 1)
    {
        float r = (float)random.NextDouble();
        float g = (float)random.NextDouble();
        float b = (float)random.NextDouble();
        return new Color(r, g, b, initialAlpha);
    }

    public override void Update()
    {
        base.Update();
        lightSpeedTimer += Time.deltaTime;

        if (lightSpeedTimer >= lightSpeed)
        {
            lightSpeedTimer = 0;
            int step = Mathf.FloorToInt(Time.time / lightSpeed);
            random = new Random(seed + step);
            lastColorSaved = nextColor;
            nextColor = GetRandomColor();
        }

        float t = Mathf.Clamp(lightSpeedTimer / lightSpeed, 0f, 1f);
        ChangeColor(Color.Lerp(lastColorSaved, nextColor, t));
    }
}