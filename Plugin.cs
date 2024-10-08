﻿using System.Collections.Generic;
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
        const string VERSION = "1.0.5";

        public static ColorfulJarOfPicklesPlugin instance;

        public ConfigEntry<string> spawnMoonRarity;
        public ConfigEntry<string> bigJarSpawnMoonRarity;
        public ConfigEntry<string> smallJarSpawnMoonRarity;
        public ConfigEntry<string> longJarSpawnMoonRarity;
        public ConfigEntry<string> flatJarSpawnMoonRarity;
        public ConfigEntry<string> stackSmallJarSpawnMoonRarity;
        public ConfigEntry<string> roundJarSpawnMoonRarity;
        
        public ConfigEntry<string> rainbowSmallJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowBigJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowSpawnMoonRarity;
        public ConfigEntry<string> rainbowLongJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowFlatJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowStackSmallJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowRoundJarSpawnMoonRarity;

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

        string RarityString(int rarity)
        {
            return
                $"Modded:{rarity},ExperimentationLevel:{rarity},AssuranceLevel:{rarity},VowLevel:{rarity},OffenseLevel:{rarity},MarchLevel:{rarity},RendLevel:{rarity},DineLevel:{rarity},TitanLevel:{rarity},Adamance:{rarity},Embrion:{rarity},Artifice:{rarity}";

        }

        void LoadConfigs()
        {
            
            //GENERAL
            spawnMoonRarity = Config.Bind("General", "ScrapSpawnRarity", 
                RarityString(40),           
                "Chance for scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(spawnMoonRarity, true);
            
            bigJarSpawnMoonRarity = Config.Bind("General", "BigJarScrapSpawnRarity", 
                RarityString(20) ,
                "Chance for big jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(bigJarSpawnMoonRarity, true);
            
            smallJarSpawnMoonRarity = Config.Bind("General", "SmallJarScrapSpawnRarity", 
                RarityString(30) ,                
                "Chance for small jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(smallJarSpawnMoonRarity, true);
            
            longJarSpawnMoonRarity = Config.Bind("General", "LongJarSpawnMoonRarity", 
                RarityString(15) ,       
                "Chance for long jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(longJarSpawnMoonRarity, true);
            
            flatJarSpawnMoonRarity = Config.Bind("General", "FlatJarSpawnMoonRarity", 
                RarityString(15) ,       
                "Chance for flat jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(flatJarSpawnMoonRarity, true);
            
            stackSmallJarSpawnMoonRarity = Config.Bind("General", "StackSmallJarSpawnMoonRarity", 
                RarityString(15) ,       
                "Chance for stack of jars scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(stackSmallJarSpawnMoonRarity, true);
            
            roundJarSpawnMoonRarity = Config.Bind("General", "RoundJarSpawnMoonRarity", 
                RarityString(30) ,       
                "Chance for round jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(roundJarSpawnMoonRarity, true);
            
            //RAINBOWS
            
            rainbowSmallJarSpawnMoonRarity = Config.Bind("General", "RainbowSmallJarScrapSpawnRarity", 
                RarityString(5) ,   
                "Chance for rainbow small jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowSmallJarSpawnMoonRarity, true);
            
            rainbowBigJarSpawnMoonRarity = Config.Bind("General", "RainbowBigJarScrapSpawnRarity", 
                RarityString(3) ,          
                "Chance for rainbow big jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowBigJarSpawnMoonRarity, true);
            
            rainbowSpawnMoonRarity = Config.Bind("General", "RainbowScrapSpawnRarity", 
                RarityString(5) ,
                "Chance for rainbow scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowSpawnMoonRarity, true);
            
            rainbowLongJarSpawnMoonRarity = Config.Bind("General", "RainbowLongJarSpawnMoonRarity", 
                RarityString(3) ,       
                "Chance for rainbow long jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowLongJarSpawnMoonRarity, true);
            
            rainbowFlatJarSpawnMoonRarity = Config.Bind("General", "RainbowFlatJarSpawnMoonRarity", 
                RarityString(3) ,       
                "Chance for rainbow flat jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowFlatJarSpawnMoonRarity, true);
            
            rainbowStackSmallJarSpawnMoonRarity = Config.Bind("General", "RainbowStackSmallJarSpawnMoonRarity", 
                RarityString(3) ,       
                "Chance for rainbow stack of jars scrap to spawn for any moon, examp" +
                "le => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowStackSmallJarSpawnMoonRarity, true);
            
            rainbowRoundJarSpawnMoonRarity = Config.Bind("General", "RainbowRoundJarSpawnMoonRarity", 
                RarityString(5) ,       
                "Chance for rainbow round jar scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowRoundJarSpawnMoonRarity, true);
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
            
            //longColorfulJar
            Item longColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/LongColorfulPickleJar.asset");
            Logger.LogInfo($"{longColorfulJar.name} FOUND");
            Logger.LogInfo($"{longColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(longColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(longColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(longJarSpawnMoonRarity.Value, longColorfulJar ); 
            
            //flatColorfulJar
            Item flatColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/FlatPickleJar.asset");
            Logger.LogInfo($"{flatColorfulJar.name} FOUND");
            Logger.LogInfo($"{flatColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(flatColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(flatColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(flatJarSpawnMoonRarity.Value, flatColorfulJar ); 
            
            //stackColorfulJar
            Item stackColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/StackSmallPickleJars.asset");
            Logger.LogInfo($"{stackColorfulJar.name} FOUND");
            Logger.LogInfo($"{stackColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(stackColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(stackColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(stackSmallJarSpawnMoonRarity.Value, stackColorfulJar ); 
            
            //roundColorfulJar
            Item roundColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RoundPickleJar.asset");
            Logger.LogInfo($"{roundColorfulJar.name} FOUND");
            Logger.LogInfo($"{roundColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(roundColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(roundColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(roundJarSpawnMoonRarity.Value, roundColorfulJar ); 
            
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
            
            //rainbowLongColorfulJar
            Item rainbowLongColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowLongColorfulPickleJar.asset");
            Logger.LogInfo($"{rainbowLongColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowLongColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowLongColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowLongColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowLongJarSpawnMoonRarity.Value, rainbowLongColorfulJar ); 
            
            //rainbowFlatColorfulJar
            Item rainbowFlatColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowFlatPickleJar.asset");
            Logger.LogInfo($"{rainbowFlatColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowFlatColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowFlatColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowFlatColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowFlatJarSpawnMoonRarity.Value, rainbowFlatColorfulJar ); 
            
            //rainbowStackColorfulJar
            Item rainbowStackColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowStackSmallPickleJars.asset");
            Logger.LogInfo($"{rainbowStackColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowStackColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowStackColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowStackColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowStackSmallJarSpawnMoonRarity.Value, rainbowStackColorfulJar ); 
            
            //rainbowRoundColorfulJar
            Item rainbowRoundColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RainbowRoundPickleJar.asset");
            Logger.LogInfo($"{rainbowRoundColorfulJar.name} FOUND");
            Logger.LogInfo($"{rainbowRoundColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowRoundColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(rainbowRoundColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowRoundJarSpawnMoonRarity.Value, rainbowRoundColorfulJar ); 

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