using System;
using UnityEngine;

namespace Pipette
{
    public class PipetteScreen : KScreen
    {
        [Header("Current State")]
        public SimHashes element;
        [NonSerialized]
        public float mass = 1000f;
        [NonSerialized]
        public float temperature = -1f;
        [NonSerialized]
        public int diseaseCount;
        public byte diseaseIdx;
    }
}