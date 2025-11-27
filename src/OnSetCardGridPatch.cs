using HarmonyLib;

namespace HighlightItem
{
    [HarmonyPatch(typeof(Trait), nameof(Trait.OnSetCardGrid))]
    internal class OnSetCardGridPatch
    {
        private static void Postfix(Trait __instance, ButtonGrid __0)
        {
            // 商人のハイライトを有効化するためにコメントアウト
            // プレイヤーキャラクターのアイテムであるか確認
            // if (__0.invOwner?.owner?.IsPC != true)
            //     return;

            // CSVの検索条件数を確認
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
}