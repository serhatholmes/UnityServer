using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EdgeMultiplay;

namespace ChatRooms
{
    public class CreateRoomManager : MonoBehaviour
    {
        public Button createRoomButton;
        public InputField RoomNameInput;
        public InputField MaxPlayersPerRoomInput;
        public InputField MinPlayersPerRoomInput;
        private void Start()
        {
            createRoomButton.onClick.AddListener(CreateRoom);
        }

        void CreateRoom()
        {
            createRoomButton.interactable = false;
            EdgeManager.CreateRoom(playerName: GameManager.playerName,playerAvatar: 0,maxPlayersPerRoom: int.Parse(MaxPlayersPerRoomInput.text), minPlayersToStartGame: int.Parse(MinPlayersPerRoomInput.text)
                , playerTags: new Dictionary<string, string>()
                {
                {"roomName",RoomNameInput.text}
                });
        }
    }
}