using System;
using System.Linq;
using UnityEngine;

namespace ReadyPlayerMe
{
    /// <summary>
    ///     Gets Chrome, Firefox and Apple Web Kit versions out of User Agent string and uses for WebView check.
    /// </summary>
    public class UserAgent
    {
        private const int MIN_CHROME_VERSION = 70;
        private const int MIN_FIREFOX_VERSION = 64;
        private const int MIN_APPLE_WEB_KIT_VERSION = 600;

        private const string CHROME = "chrome";
        private const string FIREFOX = "firefox";
        private const string APPLE_WEB_KIT = "applewebkit";
        
        public string Value;
        public int Chrome;
        public int Firefox;
        public int AppleWebKit;

        public UserAgent(string ua = "")
        {
            Value = ua;
            
            var items = ua.ToLower().Split(' ')
                .Where(item => item.Contains(CHROME) || item.Contains(FIREFOX) || item.Contains(APPLE_WEB_KIT))
                .ToArray();
            
            foreach (var item in items)
            {
                var bits = item.Split('/');
                var browser = bits[0];
                var version = 0;
                
                try
                {
                    version = new Version(bits[1]).Major;
                }
                catch (Exception e)
                {
                    Debug.Log($"{browser} version could not be parsed: {bits[1]}. Error message: {e.Message}");
                }
                
                switch (browser)
                {
                    case CHROME: Chrome = version; break;
                    case FIREFOX: Firefox = version; break;
                    case APPLE_WEB_KIT: AppleWebKit = version; break;
                }
            }
        }

        /// <summary>
        ///     Check minimum browser versions to determine if device webview eligible to run RPM website.
        /// </summary>
        public bool IsWebViewUpToDate()
        {
            Debug.Log(this);

            if (Chrome > 0 || Firefox > 0)
            {
                return Chrome >= MIN_CHROME_VERSION || Firefox >= MIN_FIREFOX_VERSION;
            }
            
            return AppleWebKit >= MIN_APPLE_WEB_KIT_VERSION;
        }
        
        public override string ToString()
        {
            return $"Chrome: {Chrome} | Firefox: {Firefox} | Apple WebKit: {AppleWebKit}";
        }
    }
}
