using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Wave")]
public class EnemyWaveSO : ScriptableObject
{
    public int waveNumber;
    public List<WavePart> wavePart;

    [Serializable]
    public struct WavePart
    {
        public int cooldown;
        public List<WaveEnemy> typeEnemies;
        

    }


    [Serializable]
    public struct WaveEnemy
    {
        public EnemySheepTypeSO enemySheep;
        public int Quantity;
        public float dificultyMod;
    }
}
