using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx;
using CsvHelper;
using HarmonyLib;
using UnityEngine;

namespace HighlightItem
{
    public static class ModInfo
    {
        public const string Guid = "Elin.HighlightItem";
        public const string Name = "Highlight Item";
        public const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class Plugin : BaseUnityPlugin
    {
        internal static Plugin? Instance;

        private const string CsvFileName = "UserFilter.csv";

        internal static List<Filter> UserFilterList = [];
        
        private void Awake()
        {
            Instance = this;
            LoadCsv();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ModInfo.Guid);
        }

        internal static void LogDebug(object message, [CallerMemberName] string caller = "")
        {
            Instance?.Logger.LogDebug($"[{caller}] {message}");
        }

        internal static void LogInfo(object message)
        {
            Instance?.Logger.LogInfo(message);
        }

        internal static void LogError(object message)
        {
            Instance?.Logger.LogError(message);
        }
        
        // CSVファイルを読み込む    
        internal static void LoadCsv()
        {
            try
            {
                // ファイルパス作成
                var csvFilePath =
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, CsvFileName);
                
                using (var reader = new StreamReader(csvFilePath))
                {
                    using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    UserFilterList = csvReader.GetRecords<Filter>().ToList();
                }
                
                // 成功メッセージ
                EClass.ui.Say($"[{ModInfo.Name}] Success load CSV File");
            }
            catch (Exception ex)
            {
                // 失敗メッセージ
                EClass.ui.Say($"[{ModInfo.Name}] Failed load CSV File");
                Debug.Log(ex.Message);
            }
        }
        
        // エンチャントがCSV条件に合うかを返す
        internal static bool CheckIsMatch(Element element, Filter filter)
        {
            // エレメント名がnullならfalse
            if (string.IsNullOrEmpty(element.Name))
                return false;

            if (element.IsFoodTrait)
                return false;

            return element.Name.Equals(filter.EnchantName) && element.Value >= (filter.Value ?? 0);
        }
    }
}