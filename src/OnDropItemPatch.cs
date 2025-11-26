using HarmonyLib;

namespace HighlightItem
{
    [HarmonyPatch(typeof(Map), nameof(Map.OnCardAddedToZone))]
    internal class OnDropItemPatch
    {
        private static void Postfix(Map __instance, Card t)
        {
            // 変数tがThing型にキャストできない場合、またはそのplaceStateがroamingでない場合は処理を終了する
            if (t is not Thing thing || thing.placeState is not PlaceState.roaming)
                return;

            if (Plugin.UserFilterList.Count == 0)
                return;

            if (thing.elements == null)
                return;

            foreach (var userFilter in Plugin.UserFilterList)
            {
                if (string.IsNullOrEmpty(userFilter.EnchantName))
                    continue;

                foreach (var element in thing.elements.dict.Values)
                {
                    if (Plugin.CheckIsMatch(element, userFilter))
                    {
                        // サウンド再生
                        SE.Play("jingle_lvup");

                        // エフェクト再生
                        var effect = Effect.Get("aura_heaven");
                        if (effect != null)
                            effect.Play(thing.pos);
                    }
                }
            }
        }
    }
}