using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace redis_client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName()); 
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1"); 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 6378); 
  
            // Creation TCP/IP Socket using  
            // Socket Class Costructor 
            Socket redisSocket = new Socket(ipAddr.AddressFamily, 
                        SocketType.Stream, ProtocolType.Tcp); 

            ConnectToRedis(localEndPoint, redisSocket);

            var online = 1;
            while(online == 1)
            {
                Console.WriteLine("Enter a redis command...");
                var input = Console.ReadLine();
                if(input.ToLower() == "exit")
                {
                    online = 0;
                    break;
                }

                SendMessage(Encoding.ASCII.GetBytes($"{input}\r\n"), redisSocket); 
            }
            
  
            // Close Socket using  
            // the method Close() 
            redisSocket.Shutdown(SocketShutdown.Both); 
            redisSocket.Close(); 
        }

        private static void ConnectToRedis(IPEndPoint localEndPoint, Socket redisSocket)
        {
            redisSocket.Connect(localEndPoint); 
  
            Console.WriteLine("Socket connected to -> {0} ", redisSocket.RemoteEndPoint.ToString()); 
  
            byte[] messageSent = Encoding.ASCII.GetBytes("ping\r\n"); 
            SendMessage(messageSent, redisSocket);
        }

        private static void SendMessage(byte[] message, Socket redisSocket)
        {
            int byteSent = redisSocket.Send(message); 
    
            byte[] messageReceived = new byte[1024]; 
            int byteRecv = redisSocket.Receive(messageReceived); 
            Console.WriteLine("Message from Redis Server -> {0}",  
            Encoding.ASCII.GetString(messageReceived, 0, byteRecv)); 
        }
    }
}
