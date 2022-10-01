using UnityEngine;
using EdgeMultiplay;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ChatRooms
{
    [RequireComponent(typeof(EdgeManager))]
    public class GameManager : EdgeMultiplayCallbacks
    {

        public Button startGameButton;
        public InputField nameInputField;
        public GameObject StartPanel;
        public static string playerName;
        public GameObject RoomSelectionPanel;
        public GameObject ScrollViewContent;
        public GameObject RoomButtonPrefab;
        public GameObject CreateRoomPanel;

        // Use this for initialization
        void Start()
        {
            startGameButton.onClick.AddListener(StartButtonPressed);
            
        }

        // Called once connected to your server deployed on Edge
        public override void OnConnectionToEdge()
        {
            StartPanel.SetActive(false);

            EdgeManager.GetRooms();
            RoomSelectionPanel.SetActive(true);
        }

        public override void OnFaliureToConnect(string reason)
        {
            Debug.Log("Connection faliure reason: " + reason);
        }

        public override void OnRoomsListReceived(List<Room> rooms)
        {
            CreateRoomButtons(rooms);
        }

        public override void OnNewRoomCreatedInLobby()
        {
            EdgeManager.GetRooms();
        }

        public override void OnRoomsUpdated()
        {
          EdgeManager.GetRooms();
        }

        public override void OnRoomRemovedFromLobby()
        {
            EdgeManager.GetRooms();
        }

        public override void OnRoomJoin(Room room)
        {
            EdgeManager.gameSession.roomId = room.roomId;
            RoomSelectionPanel.SetActive(false);
            CreateRoomPanel.SetActive(false);
            if(room.gameStarted)
            {
              ChatManager.chatStarted = true;
            }
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        public override void OnRoomCreated(Room room)
        {
            EdgeManager.gameSession.roomId = room.roomId;
            RoomSelectionPanel.SetActive(false);
            CreateRoomPanel.SetActive(false);
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        public override void PlayerJoinedRoom(Room room)
        {
            if (EdgeManager.gameSession.roomId != "" && EdgeManager.gameSession.roomId == room.roomId)
            {
                string serverMessage = room.roomMembers[room.roomMembers.Count - 1].playerName + " joined the room";
                FindObjectOfType<ChatManager>().ShowReceivedMessage("<color=red>Server</color>", serverMessage);
            }
        }

        public override void OnPlayerLeft(RoomMemberLeft playerLeft)
        {
            string serverMessage = EdgeManager.GetPlayer(playerLeft.idOfPlayerLeft).playerName + " left the room.";
            FindObjectOfType<ChatManager>().ShowReceivedMessage("<color=red>Server</color>", serverMessage);
        }

        public override void OnGameStart()
        {
            ChatManager.chatStarted = true;
        }


        public override void OnLeftRoom()
        {
            SceneManager.UnloadSceneAsync(1);
            RoomSelectionPanel.SetActive(true);
            EdgeManager.GetRooms();
        }

        void CreateRoomButtons(List<Room> rooms)
        {
            // clean
            foreach (Transform child in ScrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
            // create room buttons
            foreach (Room room in rooms)
            {
                GameObject roomButton = Instantiate(RoomButtonPrefab, ScrollViewContent.transform);
                roomButton.GetComponent<RoomButtonManager>().SetupRoomButton(room);
            }
        }


        public void StartButtonPressed()
        {
            ConnectToEdge();
            playerName = nameInputField.text;
            startGameButton.interactable = false;
        }

        public override void OnWebSocketEventReceived(GamePlayEvent gamePlayEvent)
        {
            NetworkedPlayer sender = EdgeManager.GetPlayer(gamePlayEvent.senderId);
            string senderName = sender.playerName;
            switch (gamePlayEvent.eventName)
            {
                case "chat":
                    FindObjectOfType<ChatManager>().ShowReceivedMessage(senderName, gamePlayEvent.stringData[0]);
                    break;
            }
        }

    }
}