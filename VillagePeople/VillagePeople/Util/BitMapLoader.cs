using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace VillagePeople.Util {
    public class BitmapLoader {
        private static BitmapLoader _instance;

        private List<KeyValuePair<string, Image>> _bitmaps = new List<KeyValuePair<string, Image>>();

        public static BitmapLoader GetInstance() {
            return _instance ?? (_instance = new BitmapLoader());
        }

        public static Image LoadBitmap(string filename, string key) {
            var instance = GetInstance();

            var temp = instance._bitmaps.FirstOrDefault(i => i.Key == key);
            if (temp.Value != null)
                return temp.Value;

            var bitmapFile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Image loaded = new Bitmap(bitmapFile);
            instance._bitmaps.Add(new KeyValuePair<string, Image>(key, loaded));
            return loaded;
        }
    }
}