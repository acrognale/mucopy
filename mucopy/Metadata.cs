using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace mucopy
{
    public class Song
    {
        public string Album { get; set; }
        public string[] AlbumArtists { get; set; }
        public uint Year { get; set; }
    }
    public class Metadata
    {

        public static Song getMetadata(FileInfo song)
        {
            Song tmpSong = new Song();
            TagLib.File tFile = TagLib.File.Create(song.FullName);
            tmpSong.Album = tFile.Tag.Album;
            tmpSong.AlbumArtists = tFile.Tag.AlbumArtists;
            tmpSong.Year = tFile.Tag.Year;
            return tmpSong;
        }
    }
}
