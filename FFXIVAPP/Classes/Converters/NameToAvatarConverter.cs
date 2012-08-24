// FFXIVAPP
// NameToAvatarConverter.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HtmlAgilityPack;
using NLog;

namespace FFXIVAPP.Classes.Converters
{
    public sealed class NameToAvatarConverter : IMultiValueConverter
    {
        private const String LodestoneUrl = "http://lodestone.finalfantasyxiv.com/rc/search/search?tgt=77&q=\"{0}\"&cms=&cw={1}";
        private const String DefaultAvatar = "pack://application:,,,/FFXIVAPP;component/Resources/NoImage.jpg";
        private bool _cachingEnabled = true;
        
        /// <summary>
        /// </summary>
        private String CachePath
        {
            get
            {
                try
                {
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Assembly.GetEntryAssembly().GetName().Name, "./Avatars/");
                }
                catch
                {
                    _cachingEnabled = false;
                    return "./Avatars/";
                }
            }
        }

        /// <summary>
        /// </summary>
        public NameToAvatarConverter()
        {
            if (Directory.Exists(CachePath))
            {
                return;
            }
            try
            {
                Directory.CreateDirectory(CachePath);
            }
            catch
            {
                _cachingEnabled = false;
            }
        }

        #region Implementation of IMultiValueConverter

        /// <summary>
        /// </summary>
        /// <param name="values"> </param>
        /// <param name="targetType"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == null)
            {
                return null;
            }
            var image = values[0] as Image;
            var name = values[1] as String;
            if (image == null || name == null)
            {
                return new BitmapImage(new Uri(DefaultAvatar));
            }
            var cachePath = Path.Combine(CachePath, name.Replace(" ", "") + ".png");
            if (_cachingEnabled && File.Exists(cachePath))
            {
                return new BitmapImage(new Uri(cachePath));
            }
            var src = new BitmapImage(new Uri(DefaultAvatar));
            ThreadPool.QueueUserWorkItem(delegate
            {
                var request = (HttpWebRequest) WebRequest.Create(String.Format(LodestoneUrl, Uri.EscapeUriString(name), Uri.EscapeUriString(Settings.Default.ServerName)));
                request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                var response = (HttpWebResponse) request.GetResponse();
                var s = response.GetResponseStream();
                if (response.StatusCode != HttpStatusCode.OK || s == null)
                {
                }
                else
                {
                    var doc = new HtmlDocument();
                    doc.Load(s);
                    var iconNode = doc.DocumentNode.SelectSingleNode(String.Format("//*[node()='{0}']//img[@class='character-icon']", name));

                    if (iconNode != null)
                    {
                        image.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                        {
                            var imageUri = iconNode.GetAttributeValue("src", DefaultAvatar);
                            if (imageUri != DefaultAvatar)
                            {
                                imageUri = _cachingEnabled ? SaveToCache(name, new Uri(imageUri)) : imageUri;
                            }
                            image.Source = new BitmapImage(new Uri(imageUri));
                        });
                    }
                }
            });
            return src;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="imageUri"> </param>
        /// <returns> </returns>
        private String SaveToCache(String name, Uri imageUri)
        {
            var request = (HttpWebRequest) WebRequest.Create(imageUri);
            var response = request.GetResponse();
            var s = response.GetResponseStream();
            if (s != null && response.ContentType == "image/png")
            {
                var imagePath = Path.Combine(CachePath, name.Replace(" ", "") + ".png");
                var fstream = new FileStream(imagePath, FileMode.Create, FileAccess.Write);
                s.CopyTo(fstream);
                fstream.Close();
                return imagePath;
            }
            return DefaultAvatar;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="targetTypes"> </param>
        /// <param name="parameter"> </param>
        /// <param name="culture"> </param>
        /// <returns> </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}