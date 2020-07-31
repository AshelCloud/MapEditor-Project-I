using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/*
                                   맵에디터 주요 클래스
                                   후에 클래스 크기가 커지면 Util함수들 클래스 분할해서 사용
 */

public partial class TilemapManager : MonoBehaviour
{
    private void Awake()
    {
        mapDatas = new Dictionary<string, MapData>();

        tilemaps = transform.GetComponentsInChildren<Tilemap>();
        
        playerEndPositionFlags = new List<GameObject>();

        //GUI 표시
        fileName = "File Name";
        mapIndex = "Map Index";
        nextMapCount = "nextMapCount";
        
        previousMapName = "PreviousMapName";

        EndFlagCount = 1;
    }

    //Flag 생성코드
    //임시
    //필요시 수정
    private GameObject CreatePlayerFlag(bool isEndFlag = false)
    {
        var flagResource = Resources.Load<GameObject>(PrefabFilePath + "Flag");
        var go = Instantiate(flagResource, Vector3.zero, Quaternion.identity);
        if (isEndFlag)
        {
            Vector3 scale = go.transform.lossyScale;
            scale.x *= -1;

            go.transform.localScale = scale;
        }

        return go;
    }

    private void OnGUI()
    {
        fileName = GUI.TextField(new Rect(10, 10, 200, 20), fileName, 25);
        mapIndex = GUI.TextField(new Rect(210, 10, 200, 20), mapIndex, 25);
        nextMapCount = GUI.TextField(new Rect(210, 30, 150, 20), nextMapCount, 25);

        int nCount =  int.TryParse(nextMapCount, out int n) ? n : 0;
        if (nCount != nextMapNames.Count)
        {
            nextMapNames = new List<string>();

            for(int i = 0; i < nCount; i ++)
            {
                nextMapNames.Add("NextMapName_" + (i + 1).ToString());
            }
        }

        for(int i = 0; i < nextMapNames.Count; i ++)
        {
            nextMapNames[i] = GUI.TextField(new Rect(10, 70 + (i * 20), 200, 20), nextMapNames[i], 25);
        }
        previousMapName = GUI.TextField(new Rect(210, 70, 200, 20), previousMapName, 25);

        if (GUI.Button(new Rect(10, 30, 200, 20), "CreateJson"))
        {
            TilemapToJson();
        }

        if (GUI.Button(new Rect(10, 50, 200, 20), "LoadTilemap"))
        {
            StartCoroutine(JsonToTilemap());
        }

        PlayerStartPositionSettingMode = GUI.Toggle(new Rect(650, 10, 200, 30), PlayerStartPositionSettingMode, "PlayerStartPositionSettingMode");
        PlayerEndPositionSettingMode = GUI.Toggle(new Rect(650, 40, 200, 30), PlayerEndPositionSettingMode, "PlayerEndPositionSettingMode");
    }

    private void Update()
    {
        if (PlayerStartPositionSettingMode)
        {
            if(playerStartPositionFlag == null)
            {
                playerStartPositionFlag = CreatePlayerFlag();
                playerStartPositionFlag.GetComponentInChildren<TextMesh>().text = "1";
            }

            if (playerStartPositionFlag != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                playerStartPositionFlag.transform.position = mousePosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlayerStartPositionSettingMode = false;
            }
        }

        if (PlayerEndPositionSettingMode)
        {
            if( EndFlagCount > playerEndPositionFlags.Count )
            {
                GameObject flag = CreatePlayerFlag(true);
                flag.GetComponentInChildren<TextMesh>().text = EndFlagCount.ToString();
                flag.GetComponentInChildren<TextMesh>().transform.localScale = new Vector3(-1f, 1f, 1f);

                playerEndPositionFlags.Add(flag);
            }

            GameObject f = playerEndPositionFlags[EndFlagCount - 1];
            if(f != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                f.transform.position = mousePosition;
            }
            else
            {
                Debug.LogError("윤겨울에게 문의하세요");
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlayerEndPositionSettingMode = false;
                EndFlagCount++;
            }
        }

        //깃발 삭제 코드
        if(Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePosition, Vector2.zero);

            if(ray)
            {
                if(ray.collider.name.Contains("Flag"))
                {
                    Transform flag = ray.collider.transform;

                    if ( flag.lossyScale.x > 0f)
                    {
                        Destroy(playerStartPositionFlag);
                    }
                    else
                    {
                        EndFlagCount --;
                        playerEndPositionFlags.Remove(flag.gameObject);
                    }

                    for(int i = 0; i < playerEndPositionFlags.Count; i ++)
                    {
                        playerEndPositionFlags[i].GetComponentInChildren<TextMesh>().text = (i + 1).ToString();
                    }

                    Destroy(ray.transform.gameObject);
                }
            }
        }
    }

    public TilemapData CreateTilemapData(Tilemap tilemap)
    {
        var renderer = tilemap.GetComponent<TilemapRenderer>();

        TilemapRendererData rendererData = new TilemapRendererData()
        {
            IsNotNull = true,
            SortOrder = renderer.sortOrder,
            Mode = renderer.mode,
            DetectChunkCullingBounds = renderer.detectChunkCullingBounds,
            OrderinLayer = renderer.sortingOrder,
            SpriteMaskInteraction = renderer.maskInteraction
        };

        var tilemapCollider2D = tilemap.GetComponent<TilemapCollider2D>();
        TilemapCollider2DData tilemapCollider2DData = null;
        if (tilemapCollider2D != null)
        {
            tilemapCollider2DData = new TilemapCollider2DData()
            {
                IsNotNull = true,
                IsTrigger = tilemapCollider2D.isTrigger,
                UsedByEffector = tilemapCollider2D.usedByEffector,
                UsedByComposite = tilemapCollider2D.usedByComposite,
                Offset = tilemapCollider2D.offset
            };
        }

        var rigidbody2D = tilemap.GetComponent<Rigidbody2D>();
        Rigidbody2DData rigidbody2DData = null;
        if (rigidbody2D != null)
        {
            rigidbody2DData = new Rigidbody2DData()
            {
                IsNotNull = true,
                BodyType = rigidbody2D.bodyType,
                Simulated = rigidbody2D.simulated,
                UseAutoMass = rigidbody2D.useAutoMass,
                Mass = rigidbody2D.mass,
                LinearDrag = rigidbody2D.drag,
                AngularDrag = rigidbody2D.angularDrag,
                GravityScale = rigidbody2D.gravityScale,
                CollisionDetection = rigidbody2D.collisionDetectionMode,
                SleepingMode = rigidbody2D.sleepMode,
                Interpolate = rigidbody2D.interpolation,
                Constraints = rigidbody2D.constraints
            };
        }

        var compositeCollider2D = tilemap.GetComponent<CompositeCollider2D>();
        CompositeCollider2DData compositeCollider2DData = null;
        if (compositeCollider2D != null)
        {
            compositeCollider2DData = new CompositeCollider2DData()
            {
                IsNotNull = true,
                IsTrigger = compositeCollider2D.isTrigger,
                UsedByEffector = compositeCollider2D.usedByEffector,
                Offset = compositeCollider2D.offset,
                GeometryType = compositeCollider2D.geometryType,
                GenerationType = compositeCollider2D.generationType,
                VertexDistance = compositeCollider2D.vertexDistance,
                EdgeRadius = compositeCollider2D.edgeRadius
            };
        }

        var platformEffector = tilemap.GetComponent<PlatformEffector2D>();
        PlatformEffectorData platformEffectorData = null;
        if(platformEffector != null)
        {
            platformEffectorData = new PlatformEffectorData()
            {
                IsNotNull = true,
                UseColliderMask = platformEffector.useColliderMask,
                ColliderMask = platformEffector.colliderMask,
                RotationalOffset = platformEffector.rotationalOffset,
                UseOneWay = platformEffector.useOneWay,
                UseOneWayGroup = platformEffector.useOneWayGrouping,
                SurfaceArc = platformEffector.surfaceArc,
                UseSideFriction = platformEffector.useSideFriction,
                UseSideBounce = platformEffector.useSideBounce,
                SideArc = platformEffector.sideArc
            };
        }

        var boxCollider = tilemap.GetComponent<BoxCollider2D>();
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

        TilemapData tilemapData = new TilemapData()
        {
            Name = tilemap.name,
            Tag = tilemap.tag,
            Position = tilemap.transform.position,
            Rotation = tilemap.transform.rotation,
            Scale = tilemap.transform.lossyScale,
            AnimationFrameRate = tilemap.animationFrameRate,
            Color = tilemap.color,
            TileAnchor = tilemap.tileAnchor,
            Orientation = tilemap.orientation,
            TilemapRenderer = rendererData,
            TilemapCollider = tilemapCollider2DData,
            RigidBody2D = rigidbody2DData,
            CompositeCollider = compositeCollider2DData,
            PlatformEffector = platformEffectorData,
            BoxCollider = boxColliderData
        };

        return tilemapData;
    }

    //mapData를 Json으로 변환
    public void TilemapToJson()
    {
        List<TileData> tileDatas = new List<TileData>();
        List<PrefabData> prefabDatas = new List<PrefabData>();

        //데이터 생성
        //데이터테이블 변경시 같이 변경해야함
        foreach (var tilemap in tilemaps)
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

            foreach (Transform trans in tilemap.GetComponentsInChildren<Transform>())
            {
                if (trans == null) { continue; }
                if (trans.GetComponent<Tilemap>() != null) { continue; }

                var boxCollider = trans.GetComponent<BoxCollider2D>();
                BoxCollider2DData boxColliderData = null;

                if(boxCollider != null)
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

                PrefabData prefabData = new PrefabData
                {
                    Name = trans.name,
                    Position = trans.position,
                    Rotation = trans.rotation,
                    Scale = trans.lossyScale,
                    BaseTileMap = tilemapData,
                    Tag = trans.tag,
                    BoxCollider = boxColliderData
                };

                prefabDatas.Add(prefabData);
            }
        }

        //보정
        //플레이어가 땅에 박히는것을 방지
        //TODO: 매직넘버 교체
        Vector3 startPosition = playerStartPositionFlag.transform.position;

        List<Vector3> endPositions = new List<Vector3>();
        for(int i = 0; i < playerEndPositionFlags.Count; i ++)
        {
            Vector3 position = playerEndPositionFlags[i].transform.position;
            position.y += 2f;

            endPositions.Add(position);
        }

        MapData mapData = new MapData
        {
            Tiles = tileDatas,
            Prefabs = prefabDatas,
            PlayerStartPosition = startPosition,
            PlayerEndPosition = endPositions,
            NextMapName = nextMapNames,
            PreviousMapName = previousMapName
        };

        mapDatas.Add(mapIndex, mapData);

        JsonManager.ObjectToJsonWithCreate(Application.dataPath + JsonFilePath, fileName, new Serialization<string, MapData>(mapDatas));
    }
}