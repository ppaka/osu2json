using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu2json
{
    [Serializable]
    public class LevelBundle
    {
        public string[] levels = { };
    }

    [Serializable]
    public class Level
    {
        public Info info;
    }

    [Serializable]
    public class Info
    {
        public string title = "";
        public string artist = "";
        public string creator = "";
        public string difficultyName = "";
        public float difficultyLevel;

        public string audioPath = "";
        public int audioPreviewTime = 0;
        public string level4KFilePath = "", level9KFilePath = "";
        public string videoPath = "";
        public string backgroundImagePath = "";
        public string iconImagePath = "";

        public Dictionary<string, string> subtitlePaths = new Dictionary<string, string>();
    }
}
