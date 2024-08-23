using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

public class RainbowColorfulJarOfPickles : ColorfulJarOfPicklesScrap
{

    private Color nextColor;
    private float lightSpeed = 2f;

    private float lightSpeedTimer;
    private Color lastColorSaved;

    public override void Start()
    {
        base.Start();
        lastColorSaved = GetRandomColor();
        nextColor = GetRandomColor();
    }

    public override void Update()
    {
        base.Update();

        lightSpeedTimer += Time.deltaTime;

        if (lightSpeedTimer >= lightSpeed)
        {
            lightSpeedTimer = 0;
            lastColorSaved = nextColor;
            nextColor = GetRandomColor();
        }
        
        ChangeColor(Color.Lerp(lastColorSaved, nextColor, Mathf.Clamp( lightSpeedTimer / lightSpeed, 0f, 1f)));
        
    }
}