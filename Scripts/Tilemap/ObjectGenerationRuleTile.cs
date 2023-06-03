#if TILEMAP2D && TILEMAP2D_EXTRAS

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Unidork.TilemapTools
{
	/// <summary>
	/// Overrides Unity tilemap's generic visual tile for creating different tilesets like terrain, pipeline, random or animated tiles.
	/// Uses the tile flag that allows us no to instantiate objects on the tilemap in the editor.
	/// Instead we will be using data from the tilemap to instantiate objects into the editor scene.
	/// </summary>
	[CreateAssetMenu(fileName = "NewObjectGenerationRuleTile", menuName = "Tilemap/Object Generation Rule Tile")]
    public class ObjectGenerationRuleTile : RuleTile
    {  
        /// <inheritdoc/>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            var iden = Matrix4x4.identity;

            tileData.sprite = m_DefaultSprite;
            tileData.gameObject = m_DefaultGameObject;
            tileData.colliderType = m_DefaultColliderType;
            tileData.flags = TileFlags.LockTransform | TileFlags.InstantiateGameObjectRuntimeOnly;
            tileData.transform = iden;

            foreach (TilingRule rule in m_TilingRules)
            {
                Matrix4x4 transform = iden;
                if (RuleMatches(rule, position, tilemap, ref transform))
                {
                    switch (rule.m_Output)
                    {
                        case TilingRuleOutput.OutputSprite.Single:
                        case TilingRuleOutput.OutputSprite.Animation:
                            tileData.sprite = rule.m_Sprites[0];
                            break;
                        case TilingRuleOutput.OutputSprite.Random:
                            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, rule.m_PerlinScale, 100000f) * rule.m_Sprites.Length), 0, rule.m_Sprites.Length - 1);
                            tileData.sprite = rule.m_Sprites[index];
                            if (rule.m_RandomTransform != TilingRuleOutput.Transform.Fixed)
                                transform = ApplyRandomTransform(rule.m_RandomTransform, transform, rule.m_PerlinScale, position);
                            break;
                    }
                    tileData.transform = transform;
                    tileData.gameObject = rule.m_GameObject;
                    tileData.colliderType = rule.m_ColliderType;
                    break;
                }
            }
        }
    }
}

#endif
