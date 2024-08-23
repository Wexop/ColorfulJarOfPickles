using System.Collections.Generic;
using BepInEx;
using System.IO;
using System.Reflection;
using BepInEx.Configuration;
using ColorfulJarOfPickles.Utils;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using UnityEngine;
using LethalLib.Modules;

 namespace ColorfulJarOfPickles
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(StaticNetcodeLib.StaticNetcodeLib.Guid, BepInDependency.DependencyFlags.HardDependency)]
    public class ColorfulJarOfPicklesPlugin : BaseUnityPlugin
    {

        const string GUID = "wexop.colorful_jar_of_pickles";
        const string NAME = "ColorfulJarOfPickles";
        const string VERSION = "1.0.0";

        public static ColorfulJarOfPicklesPlugin instance;

        public ConfigEntry<string> spawnMoonRarity;
        public ConfigEntry<string> rainbowSpawnMoonRarity;

        void Awake()
        {
            instance = this;
            
            Logger.LogInfo($"ColorfulJarOfPickles starting....");

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "colorfuljarofpickles");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);
            
            Logger.LogInfo($"ColorfulJarOfPickles bundle found !");
            
            LoadConfigs();
            RegisterScrap(bundle);
            
            
            Logger.LogInfo($"ColorfulJarOfPickles is ready!");
        }

        void LoadConfigs()
        {
            
            //GENERAL
            spawnMoonRarity = Config.Bind("General", "ScrapSpawnRarity", 
                "Modded:65,ExperimentationLevel:40,AssuranceLevel:40,VowLevel:40,OffenseLevel:40,MarchLevel:40,RendLevel:50,DineLevel:50,TitanLevel:50,Adamance:50,Embrion:50,Artifice:60", 
                "Chance for scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(spawnMoonRarity, true);
            rainbowSpawnMoonRarity = Config.Bind("General", "RainbowScrapSpawnRarity", 
                "Modded:10,ExperimentationLevel:5,AssuranceLevel:5,VowLevel:5,OffenseLevel:10,MarchLevel:10,RendLevel:10,DineLevel:10,TitanLevel:15,Adamance:15,Embrion:15,Artifice:15", 
                "Chance for rainbow scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowSpawnMoonRarity, true);
        }
        
        void RegisterScrap(AssetBundle bundle)
        {
            //colorfulJar
            Item colorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/ColorfulPickleJar.asset");
            Logger.LogInfo($"{colorfulJar.name} FOUND");
            Logger.LogInfo($"{colorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(colorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(colorfulJar.spawnPrefab);


            RegisterUtil.RegisterScrapWithConfig(spawnMoonRarity.Value, colorfulJar ); 
            //rainbowColorfulJar
            Item rainbowColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowPickleJar.asset");
            Logger.LogInfo($"{rainbowColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowColorfulJar.spawnPrefab);


            RegisterUtil.RegisterScrapWithConfig(rainbowSpawnMoonRarity.Value, rainbowColorfulJar ); 

        }
        
        private void CreateFloatConfig(ConfigEntry<float> configEntry, float min = 0f, float max = 100f)
        {
            var exampleSlider = new FloatSliderConfigItem(configEntry, new FloatSliderOptions
            {
                Min = min,
                Max = max,
                RequiresRestart = false
            });
            LethalConfigManager.AddConfigItem(exampleSlider);
        }
        
        private void CreateIntConfig(ConfigEntry<int> configEntry, int min = 0, int max = 100)
        {
            var exampleSlider = new IntSliderConfigItem(configEntry, new IntSliderOptions()
            {
                Min = min,
                Max = max,
                RequiresRestart = false
            });
            LethalConfigManager.AddConfigItem(exampleSlider);
        }
        
        private void CreateStringConfig(ConfigEntry<string> configEntry, bool requireRestart = false)
        {
            var exampleSlider = new TextInputFieldConfigItem(configEntry, new TextInputFieldOptions()
            {
                RequiresRestart = requireRestart
            });
            LethalConfigManager.AddConfigItem(exampleSlider);
        }


    }
}