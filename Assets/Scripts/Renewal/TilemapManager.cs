using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Renewal
{
    public partial class TilemapManager : MonoBehaviour
    {
        private void Awake()
        {
            mapData = new Dictionary<string, MapData>();
            curTilemaps = GetComponentsInChildren<Tilemap>();
        }

        private void TilemapToJson()
        {
            List<TileData> tileDatas = new List<TileData>();
            List<PrefabData> prefabDatas = new List<PrefabData>();
            List<PortalData> portalDatas = new List<PortalData>();

            foreach (Tilemap tilemap in curTilemaps)
            {
                TilemapData tilemapData = CreateTilemapData(tilemap);

                foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
                {
                    Vector3Int localPlace = pos;
                    Vector3 place = tilemap.CellToWorld(localPlace);

                    if (tilemap.HasTile(localPlace))
                    {
                        var tile = tilemap.GetTile<Tile>(localPlace);
                        TileData tileData = new TileData
                        {
                            Name = tile.name,
                            WorldPlace = place,
                            LocalPlace = localPlace,
                            Matrix = tilemap.GetTransformMatrix(localPlace),
                            BaseTilemap = tilemapData
                        };
                        tileDatas.Add(tileData);
                    }
                }

                var polyCollider = tilemap.GetComponent<PolygonCollider2D>();
                PolygonCollider2DData polyColliderData = null;

                if (polyCollider != null)
                {
                    Debug.Log(gameObject.name);
                    List<Vector2[]> p = new List<Vector2[]>();
                    for (int i = 0; i < polyCollider.pathCount; i++)
                    {
                        p.Add(polyCollider.GetPath(i));
                    }
                    polyColliderData = new PolygonCollider2DData()
                    {
                        IsNotNull = true,
                        Material = polyCollider.sharedMaterial,
                        IsTrigger = polyCollider.isTrigger,
                        UsedByEffector = polyCollider.usedByEffector,
                        UsedByComposite = polyCollider.usedByComposite,
                        AutoTiling = polyCollider.autoTiling,
                        Offset = polyCollider.offset,
                        PathCount = polyCollider.pathCount,
                        Paths = p
                    };

                    PrefabData prefabData = new PrefabData
                    {
                        Name = polyCollider.name,
                        Position = polyCollider.transform.position,
                        Rotation = polyCollider.transform.rotation,
                        Scale = polyCollider.transform.lossyScale,
                        BaseTileMap = tilemapData,
                        Tag = polyCollider.tag,
                        BoxCollider2DData = null,
                        PolygonCollider = polyColliderData
                    };

                    prefabDatas.Add(prefabData);
                }

                foreach (Transform trans in tilemap.GetComponentsInChildren<Transform>())
                {
                    if (trans == null) { continue; }
                    if(trans.GetComponent<Tilemap>() != null) { continue; }

                    var boxCollider = trans.GetComponent<BoxCollider2D>();
                    BoxCollider2DData boxColliderData = null;

                    if (boxCollider != null)
                    {
                        boxColliderData = new BoxCollider2DData()
                        {
                            IsNotNull = true,
                            Material = boxCollider.sharedMaterial,
                            IsTrigger = boxCollider.isTrigger,
                            UsedByEffector = boxCollider.usedByEffector,
                            UsedByComposite = boxCollider.usedByComposite,
                            AutoTiling = boxCollider.autoTiling,
                            Offest = boxCollider.offset,
                            Size = boxCollider.size,
                            EdgeRadius = boxCollider.edgeRadius
                        };
                    }

                    Portal isPortal = trans.GetComponent<Portal>();
                    if (isPortal != null)
                    {
                        PortalData portalData = new PortalData
                        {
                            Name = isPortal.name,
                            Position = trans.position,
                            Rotation = trans.rotation,
                            Scale = trans.lossyScale,
                            BaseTileMap = tilemapData,
                            BoxCollider = boxColliderData,
                            LinkingPortalName = isPortal.linkingPortalName,
                            TargetMap = isPortal.targetMap
                        };

                        Debug.Log(isPortal.linkingPortalName);

                        portalDatas.Add(portalData);

                        continue;
                    }

                    PrefabData prefabData = new PrefabData
                    {
                        Name = trans.name,
                        Position = trans.position,
                        Rotation = trans.rotation,
                        Scale = trans.lossyScale,
                        BaseTileMap = tilemapData,
                        Tag = trans.tag,
                        BoxCollider2DData = boxColliderData,
                    };

                    prefabDatas.Add(prefabData);
                }
            }

            MapData mapData = new MapData
            {
                Tiles = tileDatas,
                Prefabs = prefabDatas,
                Portals = portalDatas
            };

            this.mapData.Add(mapName, mapData);
            JsonManager.ObjectToJsonWithCreate(Application.dataPath + JsonFilePath, fileName, new Serialization<string, MapData>(this.mapData));
        }
    }
}
