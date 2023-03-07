using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelWavesSO : ScriptableObject
{
    public string nameString;
    public List<wave> waves;


    [Serializable]
    public struct wave
    {
        public int cooldown;
        public float dificultyMod;
        public EnemyWaveSO waveSO;
    }
}
