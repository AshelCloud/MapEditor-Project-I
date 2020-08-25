using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Renewal
{
    public partial class TilemapManager : MonoBehaviour
    {
        #region GUIVariables
        public string fileName;
        public string mapName;
        #endregion

        #region TilemapVariables
        Tilemap[] curTilemaps;
        #endregion

        #region Data
        private Dictionary<string, MapData> mapData;
        #endregion

        #region Path
        private const string JsonFilePath = "/Resources/MapJsons";
        #endregion
    }
}
