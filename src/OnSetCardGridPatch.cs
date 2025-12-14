using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HighlightItem
{
    [HarmonyPatch(typeof(Trait), nameof(Trait.OnSetCardGrid))]
    internal class OnSetCardGridPatch
    {
        private static void Postfix(Trait __instance, ButtonGrid __0)
        {
            // 商人のイベントリ内のハイライトを有効化するためにコメントアウト
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
                    {
                        // アイコンを表示
                        // __0.Attach("searched", false);

                        // アイテム画像の外周を黄色にする (Add Outline)
                        // ButtonGrid自身か、そのImageコンポーネントを取得
                        var images = __0.GetComponentsInChildren<Image>(true);

                        // 1. 外周 (Main Image or Button Target Graphic)
                        // 通常、ButtonGridの一番親にあるImageか、最初に見つかるImageが背景/枠
                        if (images.Length > 0)
                        {
                            var targetImage = images[0]; // 親あるいは主要なImageと仮定
                            var outline = targetImage.GetComponent<Outline>();
                            if (outline == null)
                                outline = targetImage.gameObject.AddComponent<Outline>();

                            outline.effectColor = Color.white;
                            outline.effectDistance = new Vector2(1, -1);
                            outline.enabled = true;
                        }
                    }
                }
            }
        }
    }
}