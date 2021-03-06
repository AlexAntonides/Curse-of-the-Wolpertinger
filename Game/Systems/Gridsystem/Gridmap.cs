﻿using UnityEngine;
using System.Collections;

public class Gridmap : MonoBehaviour
{
    /*
     * Gridmap instructions:
     * 
     * - 0 = Wolpertinger Area.
     * - 1 = Friendly Area.
     * - 2 = Path.
     * - 3 = Unwalkable.
     *
     * - 21 = Path Begin.
     * - 22 = Path End.
     */

    public int[,,] gridMap = new int[,,]
    { 
		/* Current: 10x1x10 cubes. */
		{{ 1, 1, 3, 1, 1, 1, 1, 1, 1, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0},}, // --
		{{ 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 2, 2, 0, 0, 0},}, // .        
		{{ 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 3, 3, 0, 2, 0, 0, 2, 0, 0, 0},}, // C        
		{{ 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0, 0, 2, 0, 0, 2, 0, 0, 3},}, // O        
		{{ 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0, 0, 2, 0, 0, 2, 21, 0, 3},}, // L        
		{{ 1, 1, 1, 2, 1, 2, 1, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2},}, // U      
		{{ 22, 2, 2, 2, 1, 2, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3},}, // M        
		{{ 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0},}, // N       
		{{ 3, 3, 1, 1, 1, 2, 2, 2, 2, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0},}, // S   
		{{ 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 3, 3, 0, 0, 0, 3, 0},}, // .    
		/*-------------ROWS---------------- */
	};
}
