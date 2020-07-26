using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class TilemapManager : MonoBehaviour
{
    public IEnumerator JsonToTilemap()
    {
        mapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(Application.dataPath + JsonFilePath, fileName).ToDictionary();

        //현재 있는 맵들 다 삭제하고 진행
        foreach (var tilemap in tilemaps)
        {
            if (tilemap != null)
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
            foreach (var tile in data.Value.Tiles)
            {
                var tilemap = UpdateTilemapDataWithCreate(tile.BaseTilemap);

                tilemap.SetTile(tile.LocalPlace, Resources.Load<Tile>(TileAssetFilePath + tile.Name));
                tilemap.SetTransformMatrix(tile.LocalPlace, tile.Matrix);
            }
            foreach (var prefab in data.Value.Prefabs)
            {
                var tilemap = UpdateTilemapDataWithCreate(prefab.BaseTileMap);

                GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                go.transform.localScale = prefab.Scale;
            }

            //플레이어 생성코드
            //임시
            //필요시 삭제
            //var playerReosurce = Resources.Load<GameObject>(PrefabFilePath + "Player");
            //var player = Instantiate(playerReosurce, data.Value.PlayerStartPosition, Quaternion.identity);
        }

        yield return null;
    }

    //Tilemap 정보갱신
    //JsonToTilemap함수에 필요
    //테스트용
    //본 프로젝트로 옮겨서 사용
    public Tilemap UpdateTilemapDataWithCreate(TilemapData mapData)
    {
        var maps = transform.GetComponentsInChildren<Tilemap>();

        foreach (var m in maps)
        {
            //자식중에 같은게 있으면 리턴
            if (m.name == mapData.Name)
            {
                return m;
            }
        }

        Tilemap map;

        //없으면 새로만들어서 정보업데이트후 리턴
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        //TilemapRenderer를 추가하면 Tilemap도 추가됨
        var renderer = go.AddComponent<TilemapRenderer>();
        TilemapRendererData rendererData = mapData.TilemapRenderer;

        renderer.sortOrder = rendererData.SortOrder;
        renderer.mode = rendererData.Mode;
        renderer.detectChunkCullingBounds = rendererData.DetectChunkCullingBounds;
        renderer.sortingOrder = rendererData.OrderinLayer;
        renderer.maskInteraction = rendererData.SpriteMaskInteraction;

        if (mapData.TilemapCollider.IsNotNull)
        {
            var collider = go.AddComponent<TilemapCollider2D>();
            var colliderData = mapData.TilemapCollider;

            collider.isTrigger = colliderData.IsTrigger;
            collider.usedByEffector = colliderData.UsedByEffector;
            collider.usedByComposite = colliderData.UsedByComposite;
            collider.offset = colliderData.Offset;
        }

        if (mapData.RigidBody2D.IsNotNull)
        {
            var rigidbody2D = go.AddComponent<Rigidbody2D>();
            var rigidbodyData = mapData.RigidBody2D;

            rigidbody2D.bodyType = rigidbodyData.BodyType;
            rigidbody2D.simulated = rigidbodyData.Simulated;
            rigidbody2D.useAutoMass = rigidbodyData.UseAutoMass;
            rigidbody2D.mass = rigidbodyData.Mass;
            rigidbody2D.drag = rigidbodyData.LinearDrag;
            rigidbody2D.angularDrag = rigidbodyData.AngularDrag;
            rigidbody2D.gravityScale = rigidbodyData.GravityScale;
            rigidbody2D.collisionDetectionMode = rigidbodyData.CollisionDetection;
            rigidbody2D.sleepMode = rigidbodyData.SleepingMode;
            rigidbody2D.interpolation = rigidbodyData.Interpolate;
            rigidbody2D.constraints = rigidbodyData.Constraints;
        }

        if (mapData.CompositeCollider.IsNotNull)
        {
            var collider = go.AddComponent<CompositeCollider2D>();
            var colliderData = mapData.CompositeCollider;

            collider.isTrigger = colliderData.IsTrigger;
            collider.usedByEffector = colliderData.UsedByEffector;
            collider.offset = colliderData.Offset;
            collider.geometryType = colliderData.GeometryType;
            collider.generationType = colliderData.GenerationType;
            collider.vertexDistance = colliderData.VertexDistance;
            collider.edgeRadius = colliderData.EdgeRadius;
        }

        map = go.GetComponent<Tilemap>();

        //정보 업데이트
        //데이터테이블 변경시 같이 변경해야함
        map.transform.name = mapData.Name;
        map.tag = mapData.Tag;
        map.transform.position = mapData.Position;
        map.transform.rotation = mapData.Rotation;
        map.transform.localScale = mapData.Scale;
        map.animationFrameRate = mapData.AnimationFrameRate;
        map.color = mapData.Color;
        map.tileAnchor = mapData.TileAnchor;
        map.orientation = mapData.Orientation;

        return map;
    }
}
