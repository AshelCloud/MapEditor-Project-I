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
            mapIndex = GUI.TextField(new Rect(210, 10, 200, 20), mapIndex, 25);
            nextMapCount = GUI.TextField(new Rect(210, 30, 150, 20), nextMapCount, 25);
        }
    }
}