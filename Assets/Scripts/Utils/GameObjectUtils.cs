using UnityEngine;

namespace CasinoGames.Utils
{
    public static class GameObjectUtils
    {
        public static T CreateComponent<T>(string name = "") where T : Component
        {
            var componentObject = new GameObject(name);
            T component = componentObject.AddComponent<T>();

            return component;
        }
    }
}
