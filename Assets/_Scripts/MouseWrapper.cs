﻿using UnityEngine;



public static class MouseWrapper
{
    // Direction vector scaled to the screen
    public static Vector3 GetMouseDirection()
    {
        return new Vector3(
            2f * (((Input.mousePosition.x) / Screen.width) - 0.5f),
            2f * (((Input.mousePosition.y) / Screen.height) - 0.5f),
            -100f);
    }
}
