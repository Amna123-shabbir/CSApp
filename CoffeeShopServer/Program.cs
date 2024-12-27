using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;



namespace CoffeeShopServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(System.Net.IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Coffee Shop Server Started!");
            Console.WriteLine("Waiting for client connections...");

            while (true)
            {
                try
                {
                    Socket socketForClient = server.AcceptSocket();
                    Console.WriteLine("Client connected!");

                    Thread clientThread = new Thread(() =>
                    {
                        HandleClient(socketForClient);
                    });

                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        static void HandleClient(Socket clientSocket)
        {
            using (NetworkStream stream = new NetworkStream(clientSocket))
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream) { AutoFlush = true })
            {
                writer.WriteLine("Welcome to Amna's Coffee Shop! Here are the available options:");
                writer.WriteLine("1. Espresso");
                writer.WriteLine("2. Cappuccino");
                writer.WriteLine("3. Latte");
                writer.WriteLine("4. Mocha");
                writer.WriteLine("Please type the number of your choice:");

                try
                {
                    string clientMessage;
                    while ((clientMessage = reader.ReadLine()) != null)
                    {
                        Console.WriteLine("Client >> " + clientMessage);

                        string response;
                        switch (clientMessage)
                        {
                            case "1":
                                response = "You ordered Espresso. Thank you!";
                                break;
                            case "2":
                                response = "You ordered Cappuccino. Thank you!";
                                break;
                            case "3":
                                response = "You ordered Latte. Thank you!";
                                break;
                            case "4":
                                response = "You ordered Mocha. Thank you!";
                                break;
                            default:
                                response = "Invalid choice. Please select a valid option (1-4).";
                                break;
                        }

                        writer.WriteLine(response);

                        if (response.Contains("Thank you!"))
                        {
                            break; 
                        }
                    }
                    Console.WriteLine("You ordered Espresso.Thank you!");
                    Console.WriteLine("Client disconnected.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    clientSocket.Close();
                }
            }
        }
    }
}