using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class TilemapManager : MonoBehaviour
{
    //경로
    private const string TileAssetFilePath = "TileAssets/";
    private const string PrefabFilePath = "Prefabs/";

    //Application.dataPath와 연동해서 사용
    private const string JsonFilePath = "/Resources/MapJsons";

    //저장될 Json파일이름, 타일맵들, 플레이어위치 GUI용 오브젝트
    [Header("Settings")]
    public string fileName;
    public string mapIndex;
    public string nextMapCount;
    public string previousMapName;
    public List<string> nextMapNames;
    public Tilemap[] tilemaps;
    public GameObject playerStartPositionFlag;
    public List<GameObject> playerEndPositionFlags;

    //데이터 저장 Dictionary
    private Dictionary<string, MapData> mapDatas;

    //체크용 Property
    private bool PlayerStartPositionSettingMode { get; set; }
    private bool PlayerEndPositionSettingMode { get; set; }
    private int EndFlagCount { get; set; }

}
