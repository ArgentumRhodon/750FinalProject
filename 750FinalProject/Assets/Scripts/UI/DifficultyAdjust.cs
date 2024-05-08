using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyAdjust : MonoBehaviour
{
    public WaveManager WaveManager;

    public void ChangeDifficulty(int diff)
    {
        WaveManager.SPECoef = WaveManager.enemyCoef = diff;
    }
}
