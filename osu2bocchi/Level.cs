using System;
using System.Collections.Generic;

namespace osu2bocchi
{
    [Serializable]
    public class Level
    {
        public List<NoteData> notes = new List<NoteData>();
    }

    public class NoteData
    {
        public int type;
        public float startTime;
        public float y;
        public int direction;
    }
}
