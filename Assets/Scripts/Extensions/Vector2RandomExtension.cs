using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2RandomExtension {

	public static float V2Random (Vector2 v2)
    {
        return Random.Range(v2.x, v2.y);
    }
}
