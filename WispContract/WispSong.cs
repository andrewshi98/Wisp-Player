using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WispContract
{
    public class WispSong
    {
        public string songName { get; private set; }
        public List<String> tags { get; private set; }
        public string directory { get; private set; }

        public WispSong(string songName, List<String> tags, string directory)
        {
            this.songName = songName;
            this.tags = tags;
            this.directory = directory;
        }
    }
}
