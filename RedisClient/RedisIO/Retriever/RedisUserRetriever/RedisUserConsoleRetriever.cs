﻿using RedisClient.Helpers;
using RedisClient.Models.AppModels;
using System;
using System.Text;

namespace RedisClient.RedisIO.Retriever.RedisUserRetriever
{
    public class RedisUserConsoleRetriever : IRedisUserRetriever
    {
        public RedisUser RetrieveUser()
        {
            Console.WriteLine("Username : ");
            string Username = Console.ReadLine();

            Console.WriteLine("Password : ");
            string Password = ReadLineMasked();
            string EncryptedPassword = EncryptionHelper.CreateMD5(Password);

            return new RedisUser
            {
                Username = Username,
                Password = EncryptedPassword
            };
        }

        private static string ReadLineMasked()
        {
            char mask = '*';
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    sb.Append(keyInfo.KeyChar);
                    Console.Write(mask);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);

                    if (Console.CursorLeft == 0)
                    {
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                    }
                    else Console.Write("\b \b");
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }

    }
}