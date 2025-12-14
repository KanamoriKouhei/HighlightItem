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

            // 食べ物アイテムを除外
            if (card.IsFood)
                return;

            // エンチャントが無いアイテムを除外
            if (card.elements == null)
                return;

            foreach (var userFilter in Plugin.UserFilterList)
            {
                // CSVファイルのエンチャント名が空白の場合を除外
                if (string.IsNullOrEmpty(userFilter.EnchantName))
                    continue;

                foreach (var element in card.elements.dict.Values)
                {
                    if (Plugin.CheckIsMatch(element, userFilter, card))
                        // アイテム外枠をハイライト
                        __0.Attach("searched", false);
                }
            }
        }
    }
}