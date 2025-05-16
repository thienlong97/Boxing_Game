using Match3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Match3Manager : GenericSingleton<Match3Manager>
    {
        public static Action<CellConfig.CellType, int> onMatch;

        [Header("References")]
        public GridGroup gridGroup;
        public TouchHandler touchHandler;

        public void Start()
        {
            // Initialized
            gridGroup.Initialized();
            touchHandler.Initialized();
            GenerateMatch3();
        }

        public void GenerateMatch3()
        {
            // Generate
            gridGroup.GenerateGrid();
        }
    }
}