using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Renewal
{
    public partial class TilemapManager : MonoBehaviour
    {
        private TilemapRendererData LinkingRendererData(ref Tilemap tilemap)
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

            return rendererData;
        }

        private TilemapCollider2DData LinkingTilemapCollider2DData(ref Tilemap tilemap)
        {

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

            return tilemapCollider2DData;
        }

        private Rigidbody2DData LinkingRigidbody2DData(ref Tilemap tilemap)
        {
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

            return rigidbody2DData;
        }

        private CompositeCollider2DData LinkingCompositeCollider2DData(ref Tilemap tilemap)
        {
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

            return compositeCollider2DData;
        }

        private PlatformEffectorData LinkingPlatformEffectorData(ref Tilemap tilemap)
        {
            var platformEffector = tilemap.GetComponent<PlatformEffector2D>();
            PlatformEffectorData platformEffectorData = null;
            if (platformEffector != null)
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

            return platformEffectorData;
        }

        private BoxCollider2DData LinkingBoxCollider2DData(ref Tilemap tilemap)
        {
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

            return boxColliderData;
        }

        public TilemapData CreateTilemapData(Tilemap tilemap)
        {
            var rendererData = LinkingRendererData(ref tilemap);
            var tilemapCollider2DData = LinkingTilemapCollider2DData(ref tilemap);
            var rigidbody2DData = LinkingRigidbody2DData(ref tilemap);
            var compositeCollider2DData = LinkingCompositeCollider2DData(ref tilemap);
            var platformEffectorData = LinkingPlatformEffectorData(ref tilemap);
            var boxColliderData = LinkingBoxCollider2DData(ref tilemap);

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
    }
}