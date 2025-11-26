using HarmonyLib;
using HighlightItem;

[HarmonyPatch(typeof(Trait), nameof(Trait.OnSetCardGrid))]
internal class OnSetCardGridPatch
{
    private static void Postfix(Trait __instance, ButtonGrid __0)
    {
        // Check if the inventory belongs to the player (PC)
        if (__0.invOwner?.owner?.IsPC != true)
            return;

        if (Plugin.UserFilterList.Count == 0)
            return;

        var card = __0.Card;
        if (card == null || card.elements == null)
            return;

        foreach (var userFilter in Plugin.UserFilterList)
        {
            if (string.IsNullOrEmpty(userFilter.EnchantName))
                continue;

            foreach (var element in card.elements.dict.Values)
            {
                if (Plugin.CheckIsMatch(element, userFilter))
                    // アイテム外枠をハイライト
                    __0.Attach("guide", false);
            }
        }
    }
}