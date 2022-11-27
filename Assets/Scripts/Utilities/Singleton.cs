using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton
{
    public static int getSceneChange { get; set; } = 0;
    public static bool isFromReplay { get; set; } = false;
}
