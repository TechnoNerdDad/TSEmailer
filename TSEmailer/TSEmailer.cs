﻿/*
 * TSEmailer - An Email System for TShock Terraria Dedicated Server
 * Commands:
 *      - /email - Displays usage example and "Try '/email help' For list of TSEmail commands...".
 *      - /email help - Displays list of subcommands.
 *      - /email <command> help - Displays command usage example.
 *      - /email <user> <message> - Sends email to <user> only if their email address is registered.
 *      - /email settings - Displays email address registered and display option statuses. Display unread message count.
 *      - /email address <email address> - Set self email address.
 *      - /email players [true|false] - Allow/Disallow other players to email directly.
 *      - /email admins [true|false] - Allow/Disallow admins to email directly.
 *      - /email eblast [true|false] - Allow/Disallow receiving email directed to all registered users.
 *      - /email reply [true|false] - Allow/Disallow use sending player's email address as the 'reply-to' address.
 *      - /email notify [true|false] - Allow/Disallow sending of email when player joins server.
 *      - /email onjoin <user> - Send email to self when <user> has joined the server.
 *
 * JSON Options:
 *      - smtpserver  - SMTP relay server IP address or hostname
 *      - smtpport    - SMTP server port
 *      - mapiserver  - MAPI server for receiving messages
 *      - mapiport    - MAPI server port
 *      - smtpuser    - SMTP username for servers requiring authentication. If "" authentication is not required.
 *      - smtppass    - SMTP password for servers requiring authentication. If smtpuser is "" this is ignored.
 *      - mapiuser    - MAPI username
 *      - mapipass    - MAPI password
 *      - mailtimer   - Delay in minutes for mail checking.
 *      - jointimer   - Delay in seconds to allow joining player to disable sending "onjoin" notifications.
 *      - newplayer   - TRUE|FALSE sending of notification emails onjoin of unrecongnized players.
 * 
 * DEVTASKS:
 * Check tsemail.json for email relay settings.
 * Check for tables in DB at runtime and create them if nonexistant.
 * Player OnJoin, add their ID to TSEplayertable.
 * Prompt unrecognized players to register their email.
 * Check mail relay for messages and display them to user if they are on. Leave unread until recipient joins then display.
 */

using System;
using Terraria;
using System.IO;
using jsonConfig;
using TShockAPI;
using TShockAPI.DB;
using System.ComponentModel;

namespace TSEmailer
{
    [APIVersion(1, 11)]
    public class TSEmailer : TerrariaPlugin
    {
        public static jsConfig TSEConfig { get; set; }
        internal static string TSEConfigPath { get { return Path.Combine(TShock.SavePath, "TSEconfig.json"); } }

        public override Version Version
        {
            get { return new Version("0.0.0.1"); }
        }

        public override string Name
        {
            get { return "TerrariaServer Emailer"; }
        }

        public override string Author
        {
            get { return "Travis Dieckmann"; }
        }

        public override string Description
        {
            get { return "TSEmailer is a plugin designed to allow sending of email directly to fellow players."; }
        }

        public TSEmailer(Main game)
            : base(game)
        {
            Order = 4;
            TSEConfig = new jsConfig();
        }

        public override void Initialize()
        {
            SetupConfig();
            Commands.ChatCommands.Add(new Command("", OnEmail, "email"));
        }

        private void OnEmail(CommandArgs args)
        {
            Color ErrColor = Color.Plum;

            if (args.Player != null)
            {
                args.Player.SendMessage("'email' command has not yet been implemented.", ErrColor);
                
                switch (args.Parameters[0].ToString())
                {
                    case "help":
                        args.Player.SendMessage("example: /email <player> <\"Email message body\">", ErrColor);
                        args.Player.SendMessage("Email Commands:", ErrColor);
                        args.Player.SendMessage("     /email settings - Displays your current settings.", ErrColor);
                        args.Player.SendMessage("     /email address <youraddress@yourdomain.com> - Sets your email address.", ErrColor);
                        args.Player.SendMessage("     /email players [true|false] - Allow/Disallow other players to email directly.", ErrColor);
                        args.Player.SendMessage("     /email admins [true|false] - Allow/Disallow admins to email directly.", ErrColor);
                        args.Player.SendMessage("     /email eblast [true|false] - Allow/Disallow receiving email directed to all registered users.", ErrColor);
                        args.Player.SendMessage("     /email reply [true|false] - Allow/Disallow use sending player's email address as the 'reply-to' address.", ErrColor);
                        args.Player.SendMessage("     /email notify [true|false] - Allow/Disallow sending of email when player joins server.", ErrColor);
                        args.Player.SendMessage("     /email onjoin <user> - Send email to self when <user> has joined the server.", ErrColor);
                        break;
                    case "settings":
                        args.Player.SendMessage("/email settings - A work in progress...>", ErrColor);
                        break;
                    case "address":
                        args.Player.SendMessage("/email address - A work in progress...>", ErrColor);
                        break;
                    case "players":
                        args.Player.SendMessage("/email players - A work in progress...>", ErrColor);
                        break;
                    case "admins":
                        args.Player.SendMessage("/email admins - A work in progress...>", ErrColor);
                        break;
                    case "eblast":
                        args.Player.SendMessage("/email eblast - A work in progress...>", ErrColor);
                        break;
                    case "reply":
                        args.Player.SendMessage("/email reply - A work in progress...>", ErrColor);
                        break;
                    case "notify":
                        args.Player.SendMessage("/email address - A work in progress...>", ErrColor);
                        break;
                    case "onjoin":
                        args.Player.SendMessage("/email onjoin - A work in progress...>", ErrColor);
                        break;
                    default:
                        args.Player.SendMessage("example: /email <player> <\"Email message body\">", ErrColor);
                        args.Player.SendMessage("For more email commands: /email help", ErrColor);
                        break;
                }


                foreach (string emlParam in args.Parameters.ToArray())
                {
                    args.Player.SendMessage("'email' parameter:" + emlParam);
                
                }
            }
        }

        private void OnEmailStatus(CommandArgs args)
        {
            if (args.Player != null)
            {
                args.Player.SendMessage("'email status' command has not yet been implemented.", Color.Plum);
            }
        }

        public static void SetupConfig()
        {
            try
            {
                if (File.Exists(TSEConfigPath))
                {
                    TSEConfig = jsConfig.Read(TSEConfigPath);
                    // Add all the missing config properties in the json file
                }
                TSEConfig.Write(TSEConfigPath);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TSEmailer - Error in config file");
                Console.ForegroundColor = ConsoleColor.Gray;
                Log.Error("TSEmailer - Config Exception");
                Log.Error(ex.ToString());
            }
        }
    }
}