using System.IO;
using UnityEngine;

namespace HoneyPot
{
    public class Loader
    {
        public static void Load()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<Paranoia>();
            GameObject.DontDestroyOnLoad(_gameObject);
        }
        public static void Unload()
        {
            _Unload();
        }
        private static void _Unload()
        {
            GameObject.Destroy(_gameObject);
        }
        private static GameObject _gameObject;
    }
}