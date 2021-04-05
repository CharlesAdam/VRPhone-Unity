using System.Net.Sockets;
using System.Threading.Tasks;
using GlobalDatas;
using System;
using System.Text;
using UnityEngine;
using System.Threading;

namespace MessageReader
{
    public class MessageReaderService
    {
        #region Public Variables
        // [Header("Network")]
        public string ipAddress = "192.168.0.27";
        public int port = 53558;

        public GameObject textObject;
        
        #endregion

        #region Private m_Variables
        private NetworkStream m_NetStream = null;
        private byte[] m_Buffer = new byte[49152];
        private int connectionWaitTime = 4;
        #endregion
        public async Task<NotificationData> FetchMessage()
        {
            await Task.Yield();
            var client = await Connect();
            var data = ReadMessages(client);
            if (data == null){
                //tratar erros ao tentar ler as mensagens
            }
            ClientLog(data.ToString(), Color.clear);
            return data;
            // checkForMessages(data);
        }
        private async Task<TcpClient> Connect()
        {
            bool isConnected = false;
            TcpClient client = new TcpClient();
            do
            {
                try
                {
                    client.Connect(ipAddress, port);
                    ClientLog("Client Started", Color.green);
                    isConnected = true;

                }
                catch (SocketException)
                {
                    ClientLog(
                        String.Format("Could not connect to server waiting ${0} seconds ...", connectionWaitTime)
                        , Color.blue);
                    await Task.Delay(connectionWaitTime * 1000);
                }
            } while (!isConnected);
            return client;
        }

        private NotificationData ReadMessages(TcpClient client)
        {
            m_NetStream = client.GetStream();

            ClientLog("Reading Messages ...", Color.yellow);
            var receivedMessage = m_NetStream.Read(m_Buffer, 0, m_Buffer.Length);
            return ParseMessage(Encoding.ASCII.GetString(m_Buffer, 0, receivedMessage));
        }

        private NotificationData ParseMessage(string receivedMessage)
        {
            ClientLog("Msg received on Client: " + "<b>" + receivedMessage + "</b>", Color.green);
            return JsonUtility.FromJson<NotificationData>(receivedMessage);
        }

        private void checkForMessages(NotificationData data)
        {
            data.notificationList.ForEach(
                delegate (NotificationInfo notification)
                {
                    ClientLog("Notifications for you " + "<b>" + notification + "</b>", Color.green);   
                }
            );
        }
        #region Close Client
        //Close client connection
        private void CloseClient(TcpClient client)
        {
            Debug.Log("Client Closed");
    
            if (client.Connected)
                client.Close();

            if (client != null)
                client = null;

        }
        #endregion
        private void ClientLog(string message, Color color){
            Debug.Log("<b>MessageReader:</b> " + message);
        }
    }
}