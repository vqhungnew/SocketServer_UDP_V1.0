using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Udp Server";

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            // giá trị Any của IPAddress tương ứng với Ip của tất cả các giao diện mạng trên máy

            //Show all IP configurations
            showIpAddress();

            //var localIp = IPAddress.Any;
            //var localIp = "192.168.111.8";

            Console.WriteLine("Enter the IP you want to use:");
            int addOrder = Int16.Parse(Console.ReadLine());
            IPAddress localIp = ipHost.AddressList[addOrder];
            //Console.WriteLine("You are using IP:" + ipAddr.ToString());



            // tiến trình server sẽ sử dụng cổng 1308
            var localPort = 1308;
            // biến này sẽ chứa "địa chỉ" của tiến trình server trên mạng
            var localEndPoint = new IPEndPoint(localIp, localPort);
            // yêu cầu hệ điều hành cho phép chiếm dụng cổng 1308
            // server sẽ nghe trên tất cả các mạng mà máy tính này kết nối tới
            // chỉ cần gói tin udp đến cổng 1308, tiến trình server sẽ nhận được
            // một overload khác của hàm tạo Socket
            // InterNetwork là họ địa chỉ dành cho IPv4
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEndPoint);
            Console.WriteLine($"Local socket bind to {localEndPoint}. Waiting for request ...");

            var size = 1024;
            var receiveBuffer = new byte[size];
            while (true)
            {
                // biến này về sau sẽ chứa địa chỉ của tiến trình client nào gửi gói tin tới
                EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                // khi nhận được gói tin nào sẽ lưu lại địa chỉ của tiến trình client
                var length = socket.ReceiveFrom(receiveBuffer, ref remoteEndpoint);
                var text = Encoding.ASCII.GetString(receiveBuffer, 0, length);
                Console.WriteLine($"Received from {remoteEndpoint}: {text}");
                // chuyển chuỗi thành dạng in hoa
                var result = text.ToUpper();
                var sendBuffer = Encoding.ASCII.GetBytes(result);
                // gửi kết quả lại cho client
                socket.SendTo(sendBuffer, remoteEndpoint);
                Array.Clear(receiveBuffer, 0, size);
            }
        }
        static void showIpAddress()  // https://stackoverflow.com/questions/6803073/get-local-ip-address
        {
            String strHostName = string.Empty;
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 0; i < addr.Length; i++)
            {
                Console.WriteLine("IP Address {0}: {1} ", i, addr[i].ToString());
            }
            //return ;
            //Console.ReadLine();
        }
    }
}