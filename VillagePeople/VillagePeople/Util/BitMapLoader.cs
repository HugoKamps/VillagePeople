﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace VillagePeople.Util
{
    public class BitmapLoader
    {
        public static BitmapLoader GetInstance()
        {
            if (_instance == null)
                _instance = new BitmapLoader();
            return _instance;
        }

        private static BitmapLoader _instance;

        private List<KeyValuePair<string, Image>> _bitmaps = new List<KeyValuePair<string, Image>>();

        public static Image LoadBitmap(string filename, string key)
        {
            var Instance = GetInstance();

            var temp = Instance._bitmaps.FirstOrDefault(i => i.Key == key);
            if (temp.Value != null)
                return Instance._bitmaps.FirstOrDefault(i => i.Key == key).Value;

            FileStream bitmapFile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Image loaded = new Bitmap(bitmapFile);
            Instance._bitmaps.Add(new KeyValuePair<string, Image>(key, loaded));
            return loaded;
        }
    }
}