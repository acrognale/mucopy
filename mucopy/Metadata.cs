using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace mucopy
{
    class Metadata
    {
        public static string[] getMetadata(FileInfo song)
        {
            TagLib.File tFile = TagLib.File.Create(song.FullName);
            string[] metadata = { tFile.Tag.Album, tFile.Tag.AlbumArtists.ToString(), tFile.Tag.Year.ToString() };
            return metadata;
        }
    }
}
