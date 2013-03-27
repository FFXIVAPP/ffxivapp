// FFXIVAPP.Common
// NameToAvatarConverter.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

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

#endregion

namespace FFXIVAPP.Common.Converters
{
    public class NameToAvatarConverter : IMultiValueConverter
    {
        private const string LodestoneUrl = "http://lodestone.finalfantasyxiv.com/rc/search/search?tgt=77&q=\"{0}\"&cms=&cw={1}";
        private const string DefaultAvatar = Constants.DefaultAvatar;
        private bool _cachingEnabled = true;

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

        /// <summary>
        /// </summary>
        private string CachePath
        {
            get
            {
                try
                {
                    var location = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var applicationName = Assembly.GetEntryAssembly()
                                                  .GetName()
                                                  .Name;
                    return Path.Combine(location, applicationName, "./Avatars/");
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
            var source = new BitmapImage(new Uri(DefaultAvatar));
            return source;
            var image = values[0] as Image;
            var name = values[1] as String;
            if (image == null || name == null)
            {
                return source;
            }
            var fileName = String.Format("{0}.{1}.{2}", Constants.ServerName, name.Replace(" ", ""), "png");
            var cachePath = Path.Combine(CachePath, fileName);
            if (_cachingEnabled && File.Exists(cachePath))
            {
                return new BitmapImage(new Uri(cachePath));
            }
            var useAvatars = !String.IsNullOrWhiteSpace(Constants.ServerName);
            if (useAvatars)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var serverNumber = Constants.ServerNumber;
                    var request = (HttpWebRequest) WebRequest.Create(String.Format(LodestoneUrl, Uri.EscapeUriString(name), Uri.EscapeUriString(serverNumber)));
                    request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    var response = (HttpWebResponse) request.GetResponse();
                    var stream = response.GetResponseStream();
                    if (response.StatusCode != HttpStatusCode.OK || stream == null)
                    {
                        return;
                    }
                    var doc = new HtmlDocument();
                    doc.Load(stream);
                    var xpath = String.Format("//*[node()='{0}']//img[@class='character-icon']", name);
                    var iconNode = doc.DocumentNode.SelectSingleNode(xpath);
                    if (iconNode != null)
                    {
                        image.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                        {
                            var imageUri = iconNode.GetAttributeValue("src", DefaultAvatar);
                            if (imageUri != DefaultAvatar)
                            {
                                imageUri = _cachingEnabled ? SaveToCache(fileName, new Uri(imageUri)) : imageUri;
                            }
                            image.Source = new BitmapImage(new Uri(imageUri));
                        });
                    }
                });
                //Func<bool> d = delegate
                //{
                //    return true;
                //};
                //d.BeginInvoke(null, null);
            }
            return source;
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

        /// <summary>
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="imageUri"> </param>
        /// <returns> </returns>
        private string SaveToCache(string fileName, Uri imageUri)
        {
            var request = (HttpWebRequest) WebRequest.Create(imageUri);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            if (stream != null && response.ContentType == "image/png")
            {
                var imagePath = Path.Combine(CachePath, fileName);
                var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write);
                stream.CopyTo(fileStream);
                fileStream.Close();
                return imagePath;
            }
            return DefaultAvatar;
        }
    }
}
