using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;

/*
                                   맵에디터 주요 클래스
                                   후에 클래스 크기가 커지면 Util함수들 클래스 분할해서 사용
 */

public class TilemapManager : MonoBehaviour
{
    //경로
    //웬만해서는 건드리지않기
    private const string TileAssetFilePath = "TileAssets/";
    private const string PrefabFilePath = "Prefabs/";

    //Application.dataPath와 연동해서 사용
    private const string JsonFilePath = "/Resources/MapJsons";

    //저장될 Json파일이름, 타일맵들, 플레이어위치 GUI용 오브젝트
    [Header("Settings")]
    public string fileName;
    public string mapIndex;
    public Tilemap[] tilemaps;
    public GameObject playerStartPositionFlag;
    public GameObject playerEndPositionFlag;

    //데이터 저장 Dictionary
    private Dictionary<string, MapData> mapDatas;

    //체크용 Property
    private bool PlayerStartPositionSettingMode { get; set; }
    private bool PlayerEndPositionSettingMode { get; set; }

    private void Awake()
    {
        mapDatas = new Dictionary<string, MapData>();

        tilemaps = transform.GetComponentsInChildren<Tilemap>();

        //GUI용 플레이어 위치 오브젝트 로드
        if(playerStartPositionFlag == null)
        {
            playerStartPositionFlag = CreatePlayerFlag();
        }
        if(playerEndPositionFlag == null)
        {
            playerEndPositionFlag = CreatePlayerFlag(true);
        }

        //GUI 표시
        fileName = "File Name";
        mapIndex = "Map Index";
    }

    //Flag 생성코드
    //임시
    //필요시 수정
    //isFliped는 EndPosition을 구별하기 위해 사용
    private GameObject CreatePlayerFlag(bool isFliped = false)
    {
        var flagResource = Resources.Load<GameObject>(PrefabFilePath + "Flag");
        var go = Instantiate(flagResource, Vector3.zero, Quaternion.identity);
        if(isFliped)
        {
            Vector3 scale = go.transform.lossyScale;
            scale.x *= -1;

            go.transform.localScale = scale;
        }

        go.SetActive(false);

        return go;
    }

    private void OnGUI()
    {
        fileName = GUI.TextField(new Rect(10, 10, 200, 20), fileName, 25);
        mapIndex = GUI.TextField(new Rect(210, 10, 200, 20), mapIndex, 25);

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
        if(PlayerStartPositionSettingMode)
        {
            FlagMove(playerStartPositionFlag.transform);

            if(Input.GetMouseButtonDown(0))
            {
                PlayerStartPositionSettingMode = false;
            }
        }

        if(PlayerEndPositionSettingMode)
        {
            FlagMove(playerEndPositionFlag.transform);

            if (Input.GetMouseButtonDown(0))
            {
                PlayerEndPositionSettingMode = false;
            }
        }
    }

    //플레이어 깃발 움직이는 함수
    //임시
    //필요시 수정
    private void FlagMove(Transform flag)
    {
        flag.gameObject.SetActive(true);

        //Input.mousePosition은 z가 -10으로 고정되기때문에 후처리
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        flag.position = mousePosition;
        //맵에디터 편의상 Grid에 딱맞게 Vector3Int로 변환
        //현재 필요없다고 판단
        //flag.position = Vector3ToVector3Int(mousePosition);
    }

    //Tilemap 특성상 Vector3을 Vector3Int으로 바꿀일이 많아서 따로 함수작성
    private Vector3Int Vector3ToVector3Int(Vector3 v)
    {
        Vector3Int vi = Vector3Int.zero;

        vi.x = Mathf.FloorToInt(v.x);
        vi.y = Mathf.FloorToInt(v.y);
        vi.z = Mathf.FloorToInt(v.z);
            
        return vi;
    }

    public TilemapData CreateTilemapData(Tilemap tilemap)
    {
        var renderer = tilemap.GetComponent<TilemapRenderer>();

        TilemapRendererData rendererData = new TilemapRendererData()
        {
            SortOrder = renderer.sortOrder,
            Mode = renderer.mode,
            DetectChunkCullingBounds = renderer.detectChunkCullingBounds,
            OrderinLayer = renderer.sortingOrder,
            SpriteMaskInteraction = renderer.maskInteraction
        };

        var tilemapCollider2D = tilemap.GetComponent<TilemapCollider2D>();
        TilemapCollider2DData tilemapCollider2DData = null;
        if(tilemapCollider2D != null)
        {
            tilemapCollider2DData = new TilemapCollider2DData()
            {
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
        if(compositeCollider2D != null)
        {
            compositeCollider2DData = new CompositeCollider2DData()
            {
                IsTrigger = compositeCollider2D.isTrigger,
                UsedByEffector = compositeCollider2D.usedByEffector,
                Offset = compositeCollider2D.offset,
                GeometryType = compositeCollider2D.geometryType,
                GenerationType = compositeCollider2D.generationType,
                VertexDistance = compositeCollider2D.vertexDistance,
                EdgeRadius = compositeCollider2D.edgeRadius
            };
        }

        TilemapData tilemapData = new TilemapData()
        {
            Name = tilemap.name,
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
            CompositeCollider = compositeCollider2DData
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

                if(tilemap.HasTile(localPlace))
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

                //Debug.Log(tilemap.GetTile<Tile>(localPlace).sprite.name);
            }

            foreach (Transform trans in tilemap.GetComponentsInChildren<Transform>())
            {
                if (trans == null) { continue; }
                if (trans.GetComponent<Tilemap>() != null) { continue; }

                PrefabData prefabData = new PrefabData
                {
                    Name = trans.name,
                    Position = trans.position,
                    Rotation = trans.rotation,
                    Scale = trans.lossyScale,
                    BaseTileMap = tilemapData
                };

                prefabDatas.Add(prefabData);
            }
        }
        
        //보정
        //플레이어가 땅에 박히는것을 방지
        //TODO: 매직넘버 교체
        Vector3 startPosition = playerStartPositionFlag.transform.position;
        startPosition.y += 2f;
        Vector3 endPosition = playerEndPositionFlag.transform.position;
        endPosition.y += 2f;

        MapData mapData = new MapData
        {
            Tiles = tileDatas,
            Prefabs = prefabDatas,
            PlayerStartPosition = startPosition,
            PlayerEndPosition = endPosition
        };

        mapDatas.Add(mapIndex, mapData);

        JsonManager.ObjectToJsonWithCreate(Application.dataPath + JsonFilePath, fileName, new Serialization<string, MapData>(mapDatas));
    }

    //Json로드 함수
    //테스트용
    //본 프로젝트로 옮겨서 사용
    public IEnumerator JsonToTilemap()
    {
        mapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(Application.dataPath + JsonFilePath, fileName).ToDictionary();
      
        //현재 있는 맵들 다 삭제하고 진행
        foreach (var tilemap in tilemaps)
        {
            if(tilemap != null)
            {
                Destroy(tilemap.gameObject);
            }
        }
        tilemaps = null;

        //Destroy가 느리기때문에 다 삭제될때까지 기다림
        yield return new WaitUntil(() => transform.childCount == 0);

        //데이터 로드
        //데이터테이블 변경시 같이 변경해야함
        foreach (var data in mapDatas)
        {
            foreach(var tile in data.Value.Tiles)
            {
                var tilemap = UpdateTilemapDataWithCreate(tile.BaseTilemap);

                tilemap.SetTile(tile.LocalPlace, Resources.Load<Tile>(TileAssetFilePath + tile.Name));
                tilemap.SetTransformMatrix(tile.LocalPlace, tile.Matrix);
            }
            foreach(var prefab in data.Value.Prefabs)
            {
                var tilemap = UpdateTilemapDataWithCreate(prefab.BaseTileMap);

                GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                go.transform.localScale = prefab.Scale;
            }

            //플레이어 생성코드
            //임시
            //필요시 삭제
            var playerReosurce = Resources.Load<GameObject>(PrefabFilePath + "Player");
            var player = Instantiate(playerReosurce, data.Value.PlayerStartPosition, Quaternion.identity);
        }

        yield return null;
    }

    //Tilemap 정보갱신
    //JsonToTilemap함수에 필요
    //테스트용
    //본 프로젝트로 옮겨서 사용
    public Tilemap UpdateTilemapDataWithCreate(TilemapData tilemapData)
    {
        var maps = transform.GetComponentsInChildren<Tilemap>();
        
        foreach(var m in maps)
        {
            //자식중에 같은게 있으면 리턴
            if (m.name == tilemapData.Name)
            {
                return m;
            }
        }

        Tilemap map;

        //없으면 새로만들어서 정보업데이트후 리턴
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        //TilemapRenderer를 추가하면 Tilemap도 추가됨
        go.AddComponent<TilemapRenderer>();
        //if (tilemapData.IsHaveCollider)
        //{
        //    go.AddComponent<TilemapCollider2D>();
        //}
        map = go.GetComponent<Tilemap>();

        //정보 업데이트
        //데이터테이블 변경시 같이 변경해야함
        map.transform.name = tilemapData.Name;
        map.transform.position = tilemapData.Position;
        map.transform.rotation = tilemapData.Rotation;
        map.transform.localScale = tilemapData.Scale;
        map.animationFrameRate = tilemapData.AnimationFrameRate;
        map.color = tilemapData.Color;
        map.tileAnchor = tilemapData.TileAnchor;
       //map.GetComponent<TilemapRenderer>().sortingOrder = tilemapData.OrderInLayer;

        return map;
    }
}

/*
                                데이터 class
                                추후에 데이터 테이블 참고해서 수정
                                만일, 수정시 TilemapToJson, JsonToTilemap, UpdateTilemapDataWithCreate 함수 수정
 */

[System.Serializable]
public class TileData
{
    public string Name;
    public Vector3 WorldPlace;
    public Vector3Int LocalPlace;
    public Matrix4x4 Matrix;
    public TilemapData BaseTilemap;
}

[System.Serializable]
public class PrefabData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public TilemapData BaseTileMap;
}

[System.Serializable]
public class TilemapCollider2DData
{
    public bool IsTrigger;
    public bool UsedByEffector;
    public bool UsedByComposite;
    public Vector2 Offset;
}

[System.Serializable]
public class Rigidbody2DData
{
    public RigidbodyType2D BodyType;
    public bool Simulated;
    public bool UseAutoMass;
    public float Mass;
    public float LinearDrag;
    public float AngularDrag;
    public float GravityScale;
    public CollisionDetectionMode2D CollisionDetection;
    public RigidbodySleepMode2D SleepingMode;
    public RigidbodyInterpolation2D Interpolate;
    public RigidbodyConstraints2D Constraints;
}

[System.Serializable]
public class CompositeCollider2DData
{
    public bool IsTrigger;
    public bool UsedByEffector;
    public Vector2 Offset;
    public CompositeCollider2D.GeometryType GeometryType;
    public CompositeCollider2D.GenerationType GenerationType;
    public float VertexDistance;
    public float EdgeRadius;
}

[System.Serializable]
public class TilemapRendererData
{
    public TilemapRenderer.SortOrder SortOrder;
    public TilemapRenderer.Mode Mode;
    public TilemapRenderer.DetectChunkCullingBounds DetectChunkCullingBounds;
    public int OrderinLayer;
    public SpriteMaskInteraction SpriteMaskInteraction;
}

[System.Serializable]
public class TilemapData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public float AnimationFrameRate;
    public Color Color;
    public Vector3 TileAnchor;
    public Tilemap.Orientation Orientation;

    public TilemapCollider2DData TilemapCollider;
    public TilemapRendererData TilemapRenderer;
    public Rigidbody2DData RigidBody2D;
    public CompositeCollider2DData CompositeCollider;
}


[System.Serializable]
public class MapData
{
    public List<TileData> Tiles;
    public List<PrefabData> Prefabs;
    public Vector3 PlayerStartPosition;
    public Vector3 PlayerEndPosition;
}