using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IoTDevice
{
    class Program
    {
        private static readonly Random random = new Random();       

        static void Main(string[] args)
        {         

            InitializeDevices();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void InitializeDevices()
        {

            var deviceConfigs = BaseConfigurations.LoadDeviceConfigs();

            foreach (var config in deviceConfigs)
            {
                Thread deviceThread = new Thread(() => StartDevice(config));
                deviceThread.Start();
            }
        }      

        private static void StartDevice(Device config)
        {
            var stringWrap = config.Url.Split(':');
            string ip = stringWrap[0];

            if (ip == "localhost")
                ip = "127.0.0.1";

            IPAddress ipAddress = IPAddress.Parse(ip);
            int port = Convert.ToInt32(stringWrap[1]);

            TcpListener server = new TcpListener(ipAddress, port);
            server.Start();
            Console.WriteLine($"[*] Dispositivo {config.Identifier} escutando na porta {port}");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"[*] Dispositivo {config.Identifier} conexão estabelecida");
                Thread clientThread = new Thread(() => HandleClient(client, config));
                clientThread.Start();
            }
        }

        private static void HandleClient(TcpClient client ,Device device)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string command = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"[*] Received command: {command}");
                string response;

                switch (command.ToUpper())
                {
                    case "READ":
                        response = $"{device.Identifier}: {random.NextDouble() * 100} mm\n";
                        break;
                    case "STATUS":
                        response = $"Estado do dispositivo {device.Identifier}: OK\n";
                        break;
                    case "CONFIG":
                        response = "Dados do dispositivo....\n" +
                                  $"Nome: {device.Identifier}\n" +
                                  $"Sobre :{device.Description}\n" +
                                  $"Fabricante : {device.Manufacturer} \n" +
                                  $"Nivel de Bateria : {random.Next(0,101)} %\n";
                        break;
                    case "RESET":
                        response = $"Reiniciando o dipositivo {device.Identifier}...\n";
                        response += "Reiniciado com sucesso.\n";
                        break;
                    case "HELP":
                        response = $"Comandos disponiveis {device.Identifier}:\n"
                                    + "READ - Pega o valor do dispositivo\n"
                                    + "STATUS - Pega o estado do dispositivo\n"
                                    + "CONFIG - Dados do dispositivo\n"
                                    + "RESET - Reiniciar o dispositivo\n"
                                    + "HELP - Mostra os comandos do dispositivo\n";
                        break;                   
                    default:
                        response = "Unknown command\n";
                        break;
                }

                stream.Write(Encoding.ASCII.GetBytes(response), 0, response.Length);
            }

            client.Close();
        }
    }
}
