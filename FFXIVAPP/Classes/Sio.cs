// FFXIVAPP
// Sio.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Views;
using NLog;
using SocketIOClient;

namespace FFXIVAPP.Classes
{
    public class Sio
    {
        public Client Socket;
        private static readonly FlowDocumentHelper FD = new FlowDocumentHelper();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string _clipText = "";

        /// <summary>
        /// </summary>
        public void Create()
        {
            try
            {
                Socket = new Client("http://ffxiv-app.com:4000");
                Socket.ConnectionRetryAttempt += Socket_ConnectionRetryAttempt;
                Socket.Error += Socket_Error;
                Socket.HeartBeatTimerEvent += Socket_HeartBeatTimerEvent;
                Socket.Message += Socket_Message;
                Socket.Opened += Socket_Opened;
                Socket.SocketConnectionClosed += Socket_SocketConnectionClosed;
                Socket.Connect();
                Socket.On("welcome", data =>
                {
                    var message = data.Json.GetFirstArgAs<Message>();
                    OnNewLine(message);
                });
                Socket.On("message", data =>
                {
                    var message = data.Json.GetFirstArgAs<Message>();
                    OnNewLine(message);
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Socket_ConnectionRetryAttempt(object sender, EventArgs e)
        {
            UpdateStatus(string.Format("Notice : {0}", "Retrying Connection"));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Socket_Error(object sender, ErrorEventArgs e)
        {
            UpdateStatus(string.Format("Error : {0}", e.Message));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Socket_HeartBeatTimerEvent(object sender, EventArgs e)
        {
            UpdateStatus(string.Format("Event : {0}", "Heartbeat"));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Socket_Message(object sender, MessageEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Socket_Opened(object sender, EventArgs e)
        {
            UpdateStatus(string.Format("Notice : {0}", "Connection Opened"));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Socket_SocketConnectionClosed(object sender, EventArgs e)
        {
            UpdateStatus(string.Format("Notice : {0}", "Connection Closed"));
        }

        /// <summary>
        /// </summary>
        /// <param name="Event"> </param>
        /// <param name="data"> </param>
        public void SendMessage(string Event, object data)
        {
            if (Event == "login")
            {
                //UpdateStatus(String.Format("Transmitted : {0}", ((Login) data).ToJsonString()));
            }
            else
            {
                //UpdateStatus(String.Format("Sending : {0}", ((Message) data).ToJsonString()));
                if (!Settings.Default.EnableChat)
                {
                    //UpdateStatus(string.Format("Error : {0}", "Transmission Ignored; Enable Chat Server : Off"));
                    return;
                }
                if (Socket == null)
                {
                    //UpdateStatus(string.Format("Error : {0}", "Transmission Ignored; Not Connected"));
                    return;
                }
            }
            Socket.Emit(Event, data);
        }

        /// <summary>
        /// </summary>
        public void Destroy()
        {
            Socket.ConnectionRetryAttempt -= Socket_ConnectionRetryAttempt;
            Socket.Error -= Socket_Error;
            Socket.HeartBeatTimerEvent -= Socket_HeartBeatTimerEvent;
            Socket.Message -= Socket_Message;
            Socket.Opened -= Socket_Opened;
            Socket.SocketConnectionClosed -= Socket_SocketConnectionClosed;
            if (Socket == null)
            {
                return;
            }
            Socket.Close();
            Socket.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        /// <param name="raw"> </param>
        [STAThread]
        private void OnNewLine(Message line)
        {
            lock (this)
            {
                switch (line.source)
                {
                    case "Server":
                        switch (line.message)
                        {
                            case "Hello":
                                UpdateStatus(string.Format("Notice : {0}", line.message));
                                SendMessage("login", new Login {source = "PC", name = Settings.Default.SiteName, api = Settings.Default.APIKey});
                                break;
                            case "Login Success":
                                UpdateStatus(string.Format("Notice : {0}", line.message));
                                break;
                            case "Login Error":
                                UpdateStatus(string.Format("Notice : {0}", line.message));
                                Destroy();
                                break;
                        }
                        break;
                    case "Phone":
                    case "Web":
                        if (!Settings.Default.WebsiteControl)
                        {
                            UpdateStatus(string.Format("Command : {0}", line.command + " " + line.message));
                            UpdateStatus(string.Format("Error : {0}", "Command Ignored; Web Control : Off"));
                            break;
                        }
                        if (Constants.FFXIVOpen)
                        {
                            try
                            {
                                _clipText = line.command + " " + line.message;
                                if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
                                {
                                    var t = new Thread(SetText);
                                    t.SetApartmentState(ApartmentState.STA);
                                    t.Start();
                                }
                                KeyHelper.KeyPress(Keys.Escape);
                                KeyHelper.KeyPress(Keys.Space);
                                KeyHelper.Paste();
                                KeyHelper.KeyPress(Keys.Return);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("{0} :\n{1}", ex.Message, ex.StackTrace);
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private void SetText()
        {
            Clipboard.SetText(_clipText);
        }

        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventList"> </param>
        /// <returns> </returns>
        private static Boolean CheckEvent(string eventType, IEnumerable<string> eventList)
        {
            return eventList.Any(t => t == eventType);
        }

        /// <summary>
        /// </summary>
        /// <param name="message"> </param>
        private void UpdateStatus(string message)
        {
            ChatV.View.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate
            {
                ChatV.View.StatusMessage.Content = message;
                FD.AppendFlow(DateTime.Now.ToString("[HH:mm:ss] "), message, "FFFFFF", ChatV.View.Log._FDR);
                return null;
            }), null);
        }
    }
}