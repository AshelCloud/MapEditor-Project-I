/*
                                데이터 class
                                추후에 데이터 테이블 참고해서 수정
                                만일, 수정시 TilemapToJson, JsonToTilemap, UpdateTilemapDataWithCreate 함수 수정
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
public class BoxCollider2DData
{
    public bool IsNotNull;
    public PhysicsMaterial2D Material;
    public bool IsTrigger;
    public bool UsedByEffector;
    public bool UsedByComposite;
    public bool AutoTiling;
    public Vector2 Offest;
    public Vector2 Size;
    public float EdgeRadius;
}

[System.Serializable]
public class PrefabData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public TilemapData BaseTileMap;
    public string Tag;

    public BoxCollider2DData BoxCollider2DData;
}

[System.Serializable]
public class EdgeData
{
    public PolygonCollider2DData PolygonCollider;
}

[System.Serializable]
public class PortalData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public TilemapData BaseTileMap;

    public string TargetMap;
    public string LinkingPortalName;

    public BoxCollider2DData BoxCollider;
}

[System.Serializable]
public class TilemapCollider2DData
{
    public bool IsNotNull;
    public bool IsTrigger;
    public bool UsedByEffector;
    public bool UsedByComposite;
    public Vector2 Offset;
}

[System.Serializable]
public class Rigidbody2DData
{
    public bool IsNotNull;
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
    public bool IsNotNull;
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
    public bool IsNotNull;
    public TilemapRenderer.SortOrder SortOrder;
    public TilemapRenderer.Mode Mode;
    public TilemapRenderer.DetectChunkCullingBounds DetectChunkCullingBounds;
    public int OrderinLayer;
    public SpriteMaskInteraction SpriteMaskInteraction;
}

[System.Serializable]
public class PlatformEffectorData
{
    public bool IsNotNull;
    public bool UseColliderMask;
    public int ColliderMask;
    public float RotationalOffset;
    public bool UseOneWay;
    public bool UseOneWayGroup;
    public float SurfaceArc;
    public bool UseSideFriction;
    public bool UseSideBounce;
    public float SideArc;
}

[System.Serializable]
public class PolygonCollider2DData
{
    public bool IsNotNull;
    public PhysicsMaterial2D Material;
    public bool IsTrigger;
    public bool UsedByEffector;
    public bool UsedByComposite;
    public bool AutoTiling;
    public Vector2 Offset;
    public int PathCount;
    public List<Vector2[]> Paths;
}

[System.Serializable]
public class TilemapData
{
    public string Name;
    public string Tag;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public float AnimationFrameRate;
    public Color Color;
    public Vector3 TileAnchor;
    public Tilemap.Orientation Orientation;

    public TilemapCollider2DData TilemapCollider;
    public BoxCollider2DData BoxCollider;
    public TilemapRendererData TilemapRenderer;
    public Rigidbody2DData RigidBody2D;
    public CompositeCollider2DData CompositeCollider;
    public PlatformEffectorData PlatformEffector;
}

[System.Serializable]
public class MapData
{
    public List<TileData> Tiles;
    public List<PrefabData> Prefabs;
    public List<PortalData> Portals;
    public EdgeData Edge;
}