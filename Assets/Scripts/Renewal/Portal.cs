using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renewal
{
    public class Portal : MonoBehaviour
    {
        [Header("이동할 맵 이름")]
        [Tooltip("포탈에 닿았을때 이동할 맵 이름을 정확하게 적어주세요")]
        public string targetMap;

        [Header("연결할 포탈 이름")]
        [Tooltip("다음 맵으로 연결할 포탈 이름을 정확하게 적어주세요")]
        public string linkingPortalName;
    }
}