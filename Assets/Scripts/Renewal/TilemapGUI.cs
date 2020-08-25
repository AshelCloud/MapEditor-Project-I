using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renewal
{
    public partial class TilemapManager : MonoBehaviour
    {
        private void OnGUI()
        {
            fileName = GUI.TextField(new Rect(10, 10, 200, 20), fileName, 25);
            mapName = GUI.TextField(new Rect(210, 10, 200, 20), mapName, 25);

            if(GUI.Button(new Rect(10, 30, 200, 20), "CreateJson"))
            {
                TilemapToJson();
            }
        }
    }
}