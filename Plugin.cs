using System;
using System.Collections.Generic;
using BepInEx;
using System.IO;
using System.Linq;
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
    public class ColorfulJarOfPicklesPlugin : BaseUnityPlugin
    {

        const string GUID = "wexop.colorful_jar_of_pickles";
        const string NAME = "ColorfulJarOfPickles";
        const string VERSION = "1.1.0";

        public static ColorfulJarOfPicklesPlugin instance;

        public ConfigEntry<string> spawnMoonRarity;
        public ConfigEntry<string> bigJarSpawnMoonRarity;
        public ConfigEntry<string> smallJarSpawnMoonRarity;
        public ConfigEntry<string> longJarSpawnMoonRarity;
        public ConfigEntry<string> flatJarSpawnMoonRarity;
        public ConfigEntry<string> stackSmallJarSpawnMoonRarity;
        public ConfigEntry<string> roundJarSpawnMoonRarity;
        public ConfigEntry<string> caseOfPicklesRarity;
        public ConfigEntry<string> caseOfSmallPicklesRarity;
        public ConfigEntry<string> flaskPicklesRarity;
        public ConfigEntry<string> giantPicklesRarity;
        public ConfigEntry<string> dancingPicklesRarity;
        public ConfigEntry<string> cubePicklesRarity;
        public ConfigEntry<string> lonelyPicklesRarity;
        public ConfigEntry<string> popPicklesRarity;
        
        public ConfigEntry<string> rainbowSmallJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowBigJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowSpawnMoonRarity;
        public ConfigEntry<string> rainbowLongJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowFlatJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowStackSmallJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowRoundJarSpawnMoonRarity;
        public ConfigEntry<string> rainbowCaseOfPicklesRarity;
        public ConfigEntry<string> rainbowCaseOfSmallPicklesRarity;
        public ConfigEntry<string> rainbowFlaskPicklesRarity;
        public ConfigEntry<string> rainbowGiantPicklesRarity;
        public ConfigEntry<string> rainbowDancingPicklesRarity;
        public ConfigEntry<string> rainbowCubePicklesRarity;


        public ConfigEntry<float> dancingMusicVolume;

        void Awake()
        {
            instance = this;
            
            Logger.LogInfo($"ColorfulJarOfPickles starting....");

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "colorfuljarofpickles");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);
            
            Logger.LogInfo($"ColorfulJarOfPickles bundle found !");
            
            NetcodePatcher();
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
            
            caseOfPicklesRarity = Config.Bind("General", "CaseOfPicklesRarity", 
                RarityString(15) ,       
                "Chance for case of pickles to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(caseOfPicklesRarity, true);
            
            caseOfSmallPicklesRarity = Config.Bind("General", "CaseOfSmallPicklesRarity", 
                RarityString(15) ,       
                "Chance for case of small pickles to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(caseOfSmallPicklesRarity, true);
            
            flaskPicklesRarity = Config.Bind("General", "FlaskPicklesRarity", 
                RarityString(30) ,       
                "Chance for pickles in flask scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(flaskPicklesRarity, true);
            
            giantPicklesRarity = Config.Bind("General", "GiantPicklesRarity", 
                RarityString(5) ,       
                "Chance for giant jar of pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(giantPicklesRarity, true);
            
            dancingPicklesRarity = Config.Bind("General", "DancingPicklesRarity", 
                RarityString(10) ,       
                "Chance for dancing pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(dancingPicklesRarity, true);
            
            cubePicklesRarity = Config.Bind("General", "CubePicklesRarity", 
                RarityString(30) ,       
                "Chance for cube pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(cubePicklesRarity, true);
            
            lonelyPicklesRarity = Config.Bind("General", "LonelyPicklesRarity", 
                RarityString(12) ,       
                "Chance for lonely pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(lonelyPicklesRarity, true);
            
            popPicklesRarity = Config.Bind("General", "PopPicklesRarity", 
                RarityString(8) ,       
                "Chance for pop pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(popPicklesRarity, true);
            
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
            
            rainbowCaseOfSmallPicklesRarity = Config.Bind("General", "RainbowCaseOfSmallPicklesRarity", 
                RarityString(3) ,       
                "Chance for rainbow case of small pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowCaseOfSmallPicklesRarity, true);
            
            rainbowCaseOfPicklesRarity = Config.Bind("General", "RainbowCaseOfPicklesRarity", 
                RarityString(3) ,       
                "Chance for rainbow case of pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowCaseOfPicklesRarity, true);
            
            rainbowFlaskPicklesRarity = Config.Bind("General", "RainbowFlaskPicklesRarity", 
                RarityString(5) ,       
                "Chance for rainbow pickles in flask scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowFlaskPicklesRarity, true);
            
            rainbowGiantPicklesRarity = Config.Bind("General", "RainbowGiantPicklesRarity", 
                RarityString(1) ,       
                "Chance for rainbow giant jar of pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowGiantPicklesRarity, true);
            
            rainbowDancingPicklesRarity = Config.Bind("General", "RainbowDancingPicklesRarity", 
                RarityString(1) ,       
                "Chance for rainbow giant jar of pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowDancingPicklesRarity, true);
            
            rainbowCubePicklesRarity = Config.Bind("General", "RainbowCubePicklesRarity", 
                RarityString(5) ,       
                "Chance for rainbow cube pickles scrap to spawn for any moon, example => assurance:100,offense:50 . You need to restart the game.");
            CreateStringConfig(rainbowDancingPicklesRarity, true);
            
            //MUSIC
            dancingMusicVolume = Config.Bind("Sound", "DancingMusicVolume", 
                0.4f,       
                "Dancing Pickles music volume. You don't need to restart the game.");
            CreateFloatConfig(dancingMusicVolume, 0f, 1f);
 
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
            
            //caseOfPickles
            Item caseOfPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/CaseOfPickles.asset");
            Logger.LogInfo($"{caseOfPickles.name} FOUND");
            Logger.LogInfo($"{caseOfPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(caseOfPickles.spawnPrefab);
            Utilities.FixMixerGroups(caseOfPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(caseOfPicklesRarity.Value, caseOfPickles ); 
            
            //caseOfSmallPickles
            Item caseOfSmallPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/CaseOfSmallPickles.asset");
            Logger.LogInfo($"{caseOfSmallPickles.name} FOUND");
            Logger.LogInfo($"{caseOfSmallPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(caseOfSmallPickles.spawnPrefab);
            Utilities.FixMixerGroups(caseOfSmallPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(caseOfSmallPicklesRarity.Value, caseOfSmallPickles ); 
            
            //roundColorfulJar
            Item roundColorfulJar = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/RoundPickleJar.asset");
            Logger.LogInfo($"{roundColorfulJar.name} FOUND");
            Logger.LogInfo($"{roundColorfulJar.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(roundColorfulJar.spawnPrefab);
            Utilities.FixMixerGroups(roundColorfulJar.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(roundJarSpawnMoonRarity.Value, roundColorfulJar );
            
            //FlaskPickles
            Item flaskPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/FlaskPickles.asset");
            Logger.LogInfo($"{flaskPickles.name} FOUND");
            Logger.LogInfo($"{flaskPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(flaskPickles.spawnPrefab);
            Utilities.FixMixerGroups(flaskPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(flaskPicklesRarity.Value, flaskPickles );
            
            //CubePickles
            Item cubePickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/CubePickles.asset");
            Logger.LogInfo($"{cubePickles.name} FOUND");
            Logger.LogInfo($"{cubePickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(cubePickles.spawnPrefab);
            Utilities.FixMixerGroups(cubePickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(cubePicklesRarity.Value, cubePickles );
            
            //ColorfulPickleJarGiant
            Item colorfulPickleJarGiant = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/ColorfulPickleJarGiant.asset");
            Logger.LogInfo($"{colorfulPickleJarGiant.name} FOUND");
            Logger.LogInfo($"{colorfulPickleJarGiant.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(colorfulPickleJarGiant.spawnPrefab);
            Utilities.FixMixerGroups(colorfulPickleJarGiant.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(giantPicklesRarity.Value, colorfulPickleJarGiant ); 
            
            //dancingPickles
            Item dancingPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/DancingPickles.asset");
            Logger.LogInfo($"{dancingPickles.name} FOUND");
            Logger.LogInfo($"{dancingPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(dancingPickles.spawnPrefab);
            Utilities.FixMixerGroups(dancingPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(dancingPicklesRarity.Value, dancingPickles );
            
            //lonelyPickles
            Item lonelyPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/LonelyPickles.asset");
            Logger.LogInfo($"{lonelyPickles.name} FOUND");
            Logger.LogInfo($"{lonelyPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(lonelyPickles.spawnPrefab);
            Utilities.FixMixerGroups(lonelyPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(lonelyPicklesRarity.Value, lonelyPickles ); 
            
            //popPickles
            Item popPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/PopPickles.asset");
            Logger.LogInfo($"{popPickles.name} FOUND");
            Logger.LogInfo($"{popPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(popPickles.spawnPrefab);
            Utilities.FixMixerGroups(popPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(popPicklesRarity.Value, popPickles ); 
            
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
            
            //rainbowCaseOfPickles
            Item rainbowCaseOfPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/CaseOfPicklesRainbow.asset");
            Logger.LogInfo($"{rainbowCaseOfPickles.name} FOUND");
            Logger.LogInfo($"{rainbowCaseOfPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowCaseOfPickles.spawnPrefab);
            Utilities.FixMixerGroups(rainbowCaseOfPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowCaseOfPicklesRarity.Value, rainbowCaseOfPickles ); 
            
            //rainbowCaseOfSmallPickles
            Item rainbowCaseOfSmallPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/CaseOfSmallPicklesRainbow.asset");
            Logger.LogInfo($"{rainbowCaseOfSmallPickles.name} FOUND");
            Logger.LogInfo($"{rainbowCaseOfSmallPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowCaseOfSmallPickles.spawnPrefab);
            Utilities.FixMixerGroups(rainbowCaseOfSmallPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowCaseOfSmallPicklesRarity.Value, rainbowCaseOfSmallPickles ); 
            
            //RainbowFlaskPickles
            Item rainbowFlaskPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/FlaskPicklesRainbow.asset");
            Logger.LogInfo($"{rainbowFlaskPickles.name} FOUND");
            Logger.LogInfo($"{rainbowFlaskPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowFlaskPickles.spawnPrefab);
            Utilities.FixMixerGroups(rainbowFlaskPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowFlaskPicklesRarity.Value, rainbowFlaskPickles );
            
            //rainbowGiantPickles
            Item rainbowGiantPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/ColorfulPickleJarGiantRainbow.asset");
            Logger.LogInfo($"{rainbowGiantPickles.name} FOUND");
            Logger.LogInfo($"{rainbowGiantPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowGiantPickles.spawnPrefab);
            Utilities.FixMixerGroups(rainbowGiantPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowGiantPicklesRarity.Value, rainbowGiantPickles ); 
            
            //rainbowDancingPickles
            Item rainbowDancingPickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/DancingPicklesRainbow.asset");
            Logger.LogInfo($"{rainbowDancingPickles.name} FOUND");
            Logger.LogInfo($"{rainbowDancingPickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowDancingPickles.spawnPrefab);
            Utilities.FixMixerGroups(rainbowDancingPickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowDancingPicklesRarity.Value, rainbowDancingPickles ); 
            
            //rainbowCubePicklesRarity
            Item rainbowCubePickles = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/ColorfulJarOfPickles/CubePicklesRainbow.asset");
            Logger.LogInfo($"{rainbowCubePickles.name} FOUND");
            Logger.LogInfo($"{rainbowCubePickles.spawnPrefab} prefab");
            NetworkPrefabs.RegisterNetworkPrefab(rainbowCubePickles.spawnPrefab);
            Utilities.FixMixerGroups(rainbowCubePickles.spawnPrefab);
            RegisterUtil.RegisterScrapWithConfig(rainbowCubePicklesRarity.Value, rainbowCubePickles ); 

        }
        
        /// <summary>
        ///     Slightly modified version of: https://github.com/EvaisaDev/UnityNetcodePatcher?tab=readme-ov-file#preparing-mods-for-patching
        /// </summary>
        private static void NetcodePatcher()
        {
            Type[] types;
            try
            {
                types = Assembly.GetExecutingAssembly().GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                // This goofy try catch is needed here to be able to use soft dependencies in the future, though none are present at the moment.
                types = e.Types.Where(type => type != null).ToArray();
            }

            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false).Length > 0)
                    {
                        // Do weird magic...
                        _ = method.Invoke(null, null);
                    }
                }
            }
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