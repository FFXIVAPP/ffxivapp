// FFXIVAPP.Client
// NameToAvatarConverter.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HtmlAgilityPack;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Converters
{
    [DoNotObfuscate]
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
                return new BitmapImage(new Uri(cachePath));
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
                                    image.Source = new BitmapImage(new Uri(imageUri));
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
