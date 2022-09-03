using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Generation;
using System;
using UnityEngine.EventSystems;

namespace Game.UI
{
    using Game.Run;
    public class MinimapRoom : MonoBehaviour, IPointerDownHandler
    {
        public Room room;
        Image img;

        private void Awake()
        {
           img = GetComponent<Image>(); 
        }
        public void Start()
        {
            if (room is null) {
                Debug.LogWarning("No se pudo iniciar el icono de minimapa porque no tenia asignada una habitacion");
                return;
            }
            Color newColor = GetColorByType(room);
            newColor.a = GetAlphaByExploreState();
            img.color = newColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(room.exploredState == ExploredState.Nearby || room.exploredState == ExploredState.Explored)
                Run.instance.navigator.MoveTo(room.position);
        }

        public void UpdateIcon()
        {
            Color newColor = GetColorByType(room);
            newColor.a = GetAlphaByExploreState();
            img.color = newColor;
        }
        Color GetColorByType(Room room)
        {
            switch (room.Type)
            {
                case RoomType.Treasure:
                    return Color.yellow;
                case RoomType.Shop:
                    return Color.magenta;
                case RoomType.NextLevel:
                    return Color.red;
                case RoomType.Start:
                    return Color.gray;
                default:
                    return Color.white;
            }
        }

        float GetAlphaByExploreState()
        {
            switch (room.exploredState)
            {
                case ExploredState.NotDiscovered:
                    return 0;
                case ExploredState.Nearby:
                    return 0.3f;
                case ExploredState.Explored:
                    return 1;
                default:
                    throw new ArgumentException($"{room.exploredState} no es un estado de exploracion valido");
            }
        }
    }
}
