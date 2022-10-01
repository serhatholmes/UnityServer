using EdgeMultiplay;

namespace ChatRooms
{
    public class PlayerManager : NetworkedPlayer
    {

        // Use this for initialization
        void OnEnable()
        {
            ListenToMessages();
        }

        // Once the GameObject is destroyed
        void OnDestroy()
        {
            StopListening();
        }

        // Called once a GamePlay Event is received from the server
        public override void OnMessageReceived(GamePlayEvent gamePlayEvent)
        {
            print("GamePlayEvent received from server, event name: " + gamePlayEvent.eventName);
        }

    }
}
