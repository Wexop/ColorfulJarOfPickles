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
        const string VERSION = "1.0.2";

        public static ColorfulJarOfPicklesPlugin instance;

        public ConfigEntry<string> spawnMoonRarity;
        public ConfigEntry<string> rainbowSpawnMoonRarity;
        public ConfigEntry<string> bigJarSpawnMoonRarity;
        public ConfigEntry<string> smallJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowSmallJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowBigJarSpawnMoonRarity;

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
                "Modded:40,ExperimentationLevel:40,AssuranceLevel:40,VowLevel:40,OffenseLevel:40,MarchLevel:40,RendLevel:40,DineLevel:40,TitanLevel:40,Adamance:40,Embrion:40,Artifice:40", 
                "Chance for scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(spawnMoonRarity, true);
            
            bigJarSpawnMoonRarity = Config.Bind("General", "BigJarScrapSpawnRarity", 
                "Modded:20,ExperimentationLevel:20,AssuranceLevel:20,VowLevel:20,OffenseLevel:20,MarchLevel:20,RendLevel:20,DineLevel:20,TitanLevel:20,Adamance:20,Embrion:20,Artifice:20", 
                "Chance for big jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(bigJarSpawnMoonRarity, true);
            
            smallJarSpawnMoonRarity = Config.Bind("General", "SmallJarScrapSpawnRarity", 
                "Modded:30,ExperimentationLevel:30,AssuranceLevel:30,VowLevel:30,OffenseLevel:30,MarchLevel:30,RendLevel:30,DineLevel:30,TitanLevel:30,Adamance:30,Embrion:30,Artifice:30", 
                "Chance for small jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(smallJarSpawnMoonRarity, true);
            
            rainbowSmallJarSpawnMoonRarity = Config.Bind("General", "RainbowSmallJarScrapSpawnRarity", 
                "Modded:5,ExperimentationLevel:5,AssuranceLevel:5,VowLevel:5,OffenseLevel:5,MarchLevel:5,RendLevel:5,DineLevel:5,TitanLevel:5,Adamance:5,Embrion:5,Artifice:5", 
                "Chance for rainbow small jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowSmallJarSpawnMoonRarity, true);
            
            rainbowBigJarSpawnMoonRarity = Config.Bind("General", "RainbowBigJarScrapSpawnRarity", 
                "Modded:3,ExperimentationLevel:3,AssuranceLevel:3,VowLevel:3,OffenseLevel:3,MarchLevel:3,RendLevel:3,DineLevel:3,TitanLevel:3,Adamance:3,Embrion:5,Artifice:3", 
                "Chance for rainbow big jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowBigJarSpawnMoonRarity, true);
            
            rainbowSpawnMoonRarity = Config.Bind("General", "RainbowScrapSpawnRarity", 
                "Modded:5,ExperimentationLevel:5,AssuranceLevel:5,VowLevel:5,OffenseLevel:5,MarchLevel:5,RendLevel:5,DineLevel:5,TitanLevel:5,Adamance:5,Embrion:5,Artifice:5", 
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
            
            //bigColorfulJar
            Item bigColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/BigPickleJar.asset");
            Logger.LogInfo($"{bigColorfulJar.name} FOUND");
            Logger.LogInfo($"{bigColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(bigColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(bigColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(bigJarSpawnMoonRarity.Value, bigColorfulJar ); 
            
            //smallColorfulJar
            Item smallColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/SmallPickleJar.asset");
            Logger.LogInfo($"{smallColorfulJar.name} FOUND");
            Logger.LogInfo($"{smallColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(smallColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(smallColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(smallJarSpawnMoonRarity.Value, smallColorfulJar ); 
            
            //rainbowSmallColorfulJar
            Item rainbowSmallColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowSmallPickleJar.asset");
            Logger.LogInfo($"{rainbowSmallColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowSmallColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowSmallColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowSmallColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowSmallJarSpawnMoonRarity.Value, rainbowSmallColorfulJar ); 
            
            //rainbowBigColorfulJar
            Item rainbowBigColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowBigPickleJar.asset");
            Logger.LogInfo($"{rainbowBigColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowBigColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowBigColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowBigColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowBigJarSpawnMoonRarity.Value, rainbowBigColorfulJar ); 

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