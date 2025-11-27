using HarmonyLib;
using UnityEngine;

namespace HighlightItem
{
    [HarmonyPatch(typeof(Scene), nameof(Scene.OnUpdate))]
    internal class KeybindPatch
    {
        private static void Postfix()
        {
            // Reload CSV
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Plugin.LoadCsv();
            }
        }
    }
}