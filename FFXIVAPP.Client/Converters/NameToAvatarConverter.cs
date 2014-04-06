// FFXIVAPP.Client
// NameToAvatarConverter.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FFXIVAPP.Common.Utilities;
using HtmlAgilityPack;
using NLog;

namespace FFXIVAPP.Client.Converters
{
    public class NameToAvatarConverter : IMultiValueConverter
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        private const string DefaultAvatar = Common.Constants.DefaultAvatar;
        private readonly bool _cachingEnabled = true;

        /// <summary>
        /// </summary>
        public NameToAvatarConverter()
        {
            if (Directory.Exists(AvatarCache))
            {
                return;
            }
            try
            {
                Directory.CreateDirectory(AvatarCache);
            }
            catch
            {
                _cachingEnabled = false;
            }
        }

        private bool IsProcessing { get; set; }

        private string AvatarCache
        {
            get { return Path.Combine(Common.Constants.CachePath, "Avatars"); }
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
            var image = values[0] as Image;
            var name = values[1] as String;
            if (image == null || name == null || Regex.IsMatch(name, @"\[[(A|[\?]+]\]"))
            {
                return source;
            }
            name = Regex.Replace(name, @"\[[\w]+\]", "")
                        .Trim();
            var fileName = String.Format("{0}.{1}.{2}", Constants.ServerName, name.Replace(" ", ""), "png");
            var cachePath = Path.Combine(AvatarCache, fileName);
            if (_cachingEnabled && File.Exists(cachePath))
            {
                return ImageUtilities.LoadImageFromStream(cachePath);
            }
            var useAvatars = !String.IsNullOrWhiteSpace(Constants.ServerName);
            if (!useAvatars || IsProcessing)
            {
                return source;
            }
            IsProcessing = true;
            Func<bool> downloadFunc = delegate
            {
                try
                {
                    var serverName = Constants.ServerName;
                    var url = "http://na.finalfantasyxiv.com/lodestone/character/?q={0}&worldname={1}";
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(String.Format(url, HttpUtility.UrlEncode(name), Uri.EscapeUriString(serverName)));
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                    httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    using (var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                    {
                        using (var stream = httpWebResponse.GetResponseStream())
                        {
                            if (httpWebResponse.StatusCode == HttpStatusCode.OK && stream != null)
                            {
                                var doc = new HtmlDocument();
                                doc.Load(stream);
                                var htmlSource = doc.DocumentNode.SelectSingleNode("//html")
                                                    .OuterHtml;
                                var src = new Regex(@"<img src=""(?<image>.+)"" width=""50"" height=""50"" alt="""">", RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                var imageUrl = src.Match(htmlSource)
                                                  .Groups["image"].Value;
                                imageUrl = imageUrl.Substring(0, imageUrl.IndexOf("?", Constants.InvariantComparer))
                                                   .Replace("50x50", "96x96");
                                image.Dispatcher.Invoke(DispatcherPriority.Background, (ThreadStart) delegate
                                {
                                    var imageUri = imageUrl;
                                    if (imageUri != DefaultAvatar)
                                    {
                                        imageUri = _cachingEnabled ? SaveToCache(fileName, new Uri(imageUri)) : imageUri;
                                    }
                                    image.Source = ImageUtilities.LoadImageFromStream(imageUri);
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                IsProcessing = false;
                return true;
            };
            downloadFunc.BeginInvoke(delegate { }, downloadFunc);
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
            try
            {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(imageUri);
                using (var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse())
                {
                    using (var response = httpResponse.GetResponseStream())
                    {
                        if (response != null)
                        {
                            if (httpResponse.ContentType == "image/jpeg" || httpResponse.ContentType == "image/png")
                            {
                                var imagePath = Path.Combine(AvatarCache, fileName);
                                using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                                {
                                    response.CopyTo(fileStream);
                                    fileStream.Close();
                                }
                                return imagePath;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return DefaultAvatar;
        }
    }
}
