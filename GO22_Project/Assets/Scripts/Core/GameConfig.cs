using System;
using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    [CreateAssetMenu]
    public class GameConfig : ScriptableObject
    {
        public Sprite BackgroundImage;
        public string ClicheHead;
        public string ClicheTail;
        public string Instruction;
        // List of game object that each game needs (ex: player, enemy, target)
        public List<GameObjectWithPosition> characters;
    }

    [System.Serializable]
    public class GameObjectWithPosition {
        public GameObject gameObject;
        public float x;
        public float y;
    }
}