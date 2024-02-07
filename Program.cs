using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace iTunesConverter
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public SerializableDictionary() { }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public SerializableDictionary(int capacity) : base(capacity) { }
        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }

    public class Track
    {
        public Track()
        {
            mId = 0;
            mName = "";
            mKind = "";
            mFileType = "";
            mLocation = "";

        }

        public System.UInt64 mId;
        public string mName;
        public string mKind;
        public string mFileType;
        public string mLocation;
        public string mAlbum;

        //public SerializableDictionary<string, string> mValues;

        public bool DupeCounted = false;

        /*
        public string PlaylistEntry(StringBuilder builder)
        {
            if (mValues.ContainsKey("Name")) { builder.Append(mValues["Name"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Artist")) { builder.Append(mValues["Artist"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Composer")) { builder.Append(mValues["Composer"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Album")) { builder.Append(mValues["Album"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Grouping")) { builder.Append(mValues["Grouping"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Work")) { builder.Append(mValues["Work"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Movement Number")) { builder.Append(mValues["Movement Number"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Movement Count")) { builder.Append(mValues["Movement Count"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Movement Name")) { builder.Append(mValues["Movement Name"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Genre")) { builder.Append(mValues["Genre"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Size")) { builder.Append(mValues["Size"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Time")) { builder.Append(mValues["Time"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Disc Number")) { builder.Append(mValues["Disc Number"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Disc Count")) { builder.Append(mValues["Disc Count"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Track Number")) { builder.Append(mValues["Track Number"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Track Count")) { builder.Append(mValues["Track Count"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Year")) { builder.Append(mValues["Year"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Date Modified")) { builder.Append(mValues["Date Modified"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Date Added")) { builder.Append(mValues["Date Added"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Bit Rate")) { builder.Append(mValues["Bit Rate"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Sample Rate")) { builder.Append(mValues["Sample Rate"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Volume Adjustment")) { builder.Append(mValues["Volume Adjustment"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Kind")) { builder.Append(mValues["Kind"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Equalizer")) { builder.Append(mValues["Equalizer"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Comments")) { builder.Append(mValues["Comments"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Plays")) { builder.Append(mValues["Plays"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Last Played")) { builder.Append(mValues["Last Played"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Skips")) { builder.Append(mValues["Skips"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Last Skipped")) { builder.Append(mValues["Last Skipped"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("My Rating")) { builder.Append(mValues["My Rating"]); }
            builder.Append('\t');
            if (mValues.ContainsKey("Location")) { builder.Append(mValues["Location"]); }

            return "";
        }
        */
    }

    public class Playlist
    {
        public Playlist()
        {
            mName = "";
            mTracks = new List<System.UInt64>();
        }

        public string mName;
        public List<System.UInt64> mTracks;
    }



    public class iTunesHandler
    {
        public SerializableDictionary<System.UInt64, Track> mTracks;
        public SerializableDictionary<String, List<System.UInt64>> mTracksByName;
        public SerializableDictionary<string, Playlist> mPlaylists;
        public string mMusicLibrary;

        public iTunesHandler()
        {
            mTracks = new SerializableDictionary<System.UInt64, Track>();
            mTracksByName = new SerializableDictionary<String, List<System.UInt64>>();
            mPlaylists = new SerializableDictionary<string, Playlist>();
        }

        public iTunesHandler(string aFile)
        {
            mTracks = new SerializableDictionary<System.UInt64, Track>();
            mTracksByName = new SerializableDictionary<String, List<System.UInt64>>();
            mPlaylists = new SerializableDictionary<string, Playlist>();

            XmlDocument doc = new XmlDocument();
            doc.Load(aFile);

            XmlElement plist = doc.DocumentElement;
            XmlNodeList nodes = plist.GetElementsByTagName("dict"); // You can also use XPath here

            HandleDictionary(nodes.Item(0));
        }


        private void HandleTrack(XmlNode aKey, XmlNode aValue)
        {
            if (aValue.HasChildNodes)
            {
                var track = new Track();

                var nodes = aValue.ChildNodes;
                var size = nodes.Count;
                var halfSize = size / 2;

                track.mKind = "";
                track.mLocation = "";
                track.mFileType = "";
                track.mName = "";
                track.mAlbum = "";
                //track.mValues = new SerializableDictionary<string, string>();

                for (int i = 0; i < halfSize; ++i)
                {
                    var keyIndex = i * 2;
                    var valueIndex = keyIndex + 1;

                    var key = nodes[i * 2];
                    var value = nodes[i * 2 + 1];

                    switch (key.InnerText)
                    {
                        case "Track ID":
                            {
                                System.UInt64.TryParse(value.InnerText, out track.mId);
                                break;
                            }
                        case "Kind":
                            {
                                track.mKind = value.InnerText;
                                break;
                            }
                        case "Location":
                            {
                                track.mLocation = Uri.UnescapeDataString(value.InnerText);
                                track.mFileType = Path.GetExtension(track.mLocation);
                                break;
                            }
                        case "Name":
                            {
                                track.mName = value.InnerText;
                                break;
                            }
                        case "Album":
                            {
                                track.mAlbum = value.InnerText;
                                break;
                            }
                    }

                    //track.mValues.Add(key.InnerText, value.InnerText);
                }

                mTracks.Add(track.mId, track);

                if (!mTracksByName.ContainsKey(track.mName.ToLower()))
                {
                    mTracksByName.Add(track.mName.ToLower(), new List<System.UInt64>());
                }

                mTracksByName[track.mName.ToLower()].Add(track.mId);
            }

        }


        private void HandleTracks(XmlNode aNode)
        {
            if (aNode.HasChildNodes)
            {
                var nodes = aNode.ChildNodes;
                var size = nodes.Count;
                var halfSize = size / 2;


                for (int i = 0; i < halfSize; ++i)
                {
                    var keyIndex = i * 2;
                    var valueIndex = keyIndex + 1;

                    var key = nodes[i * 2];
                    var value = nodes[i * 2 + 1];

                    HandleTrack(key, value);
                }
            }
        }


        private void HandlePlaylist(XmlNode aNode)
        {
            if (aNode.HasChildNodes)
            {
                var playlist = new Playlist();

                playlist.mName = "";

                var nodes = aNode.ChildNodes;
                var size = nodes.Count;
                var halfSize = size / 2;


                for (int i = 0; i < halfSize; ++i)
                {
                    var keyIndex = i * 2;
                    var valueIndex = keyIndex + 1;

                    var key = nodes[i * 2];
                    var value = nodes[i * 2 + 1];

                    switch (key.InnerText)
                    {
                        case "Name":
                            {
                                playlist.mName = value.InnerText;
                                break;
                            }
                        case "Visible":
                            {
                                if (value.Name == "false")
                                {
                                    return;
                                }
                                break;
                            }
                        case "Playlist Items":
                            {
                                var playlistItems = value.ChildNodes;
                                var playlistSize = value.ChildNodes.Count;
                                for (int j = 0; j < playlistSize; ++j)
                                {
                                    var itemId = playlistItems[j].ChildNodes[1];
                                    System.UInt64 trackId;

                                    if (true == System.UInt64.TryParse(itemId.InnerText, out trackId))
                                    {
                                        playlist.mTracks.Add(trackId);
                                    }
                                    else
                                    {

                                    }
                                }

                                break;
                            }
                    }
                }

                while (mPlaylists.ContainsKey(playlist.mName))
                {
                    playlist.mName += "_DuplicatePlaylistName";
                }

                mPlaylists.Add(playlist.mName, playlist);
            }
        }

        private void HandlePlaylists(XmlNode aNode)
        {
            if (aNode.HasChildNodes)
            {
                var nodes = aNode.ChildNodes;
                var size = nodes.Count;
                //var halfSize = size / 2;


                for (int i = 0; i < size; ++i)
                {
                    var value = nodes[i];
                    HandlePlaylist(value);
                }
            }
        }

        void HandleTopLevel(XmlNode aKey, XmlNode aValue)
        {
            switch (aKey.InnerText)
            {
                case "Tracks":
                    {
                        HandleTracks(aValue);
                        break;
                    }
                case "Playlists":
                    {
                        HandlePlaylists(aValue);
                        break;
                    }
                case "Music Folder":
                    {
                        mMusicLibrary = aValue.InnerText;
                        break;
                    }
            }
        }



        private void HandleDictionary(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                var nodes = node.ChildNodes;
                var size = nodes.Count;
                var halfSize = size / 2;


                for (int i = 0; i < halfSize; ++i)
                {
                    var keyIndex = i * 2;
                    var valueIndex = keyIndex + 1;

                    var key = nodes[i * 2];
                    var value = nodes[i * 2 + 1];

                    HandleTopLevel(key, value);
                }
            }
        }
    }



    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"{Directory.GetCurrentDirectory()}");

            iTunesHandler itunes = null;

            if (!File.Exists("iTunesParsed.xml"))
            {
                itunes = new iTunesHandler("D:/Music/iTunes Library.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(iTunesHandler));
                TextWriter writer = new StreamWriter("iTunesParsed.xml");
                serializer.Serialize(writer, itunes);
                writer.Close();
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iTunesHandler));
                itunes = (iTunesHandler)serializer.Deserialize(new StreamReader("iTunesParsed.xml"));
            }


            //StringBuilder playlistBuilder = new StringBuilder();
            //playlistBuilder.AppendLine("Name\tArtist\tComposer\tAlbum\tGrouping\tWork\tMovement Number\tMovement Count\tMovement Name\tGenre\tSize\tTime\tDisc Number\tDisc Count\tTrack Number\tTrack Count\tYear\tDate Modified\tDate Added\tBit Rate\tSample Rate\tVolume Adjustment\tKind\tEqualizer\tComments\tPlays\tLast Played\tSkips\tLast Skipped\tMy Rating\tLocation");
            //string precedingTabsInPlaylistEntry = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t \t\t\t\t\t\t\t\t\t\t\t\t\t";
            //playlistBuilder.AppendLine($"{precedingTabsInPlaylistEntry}{SomeFilePath}");

            StringBuilder builder = new StringBuilder();

            foreach (var track in itunes.mTracks)
            {
                if (track.Value.DupeCounted)
                {
                    continue;
                }

                var dupes = itunes.mTracksByName[track.Value.mName.ToLower()];
                var actualDupes = new List<Track>();

                foreach (var dupe in dupes)
                {
                    var dupeTrack = itunes.mTracks[dupe];
                    if ((track.Value.mId == dupeTrack.mId))// || (track.Value.mName == "Engadget Headlines"))
                    {
                        continue;
                    }

                    bool isPurchasedAACAudioFile = track.Value.mKind == "Purchased AAC audio file";

                    if (track.Value.mAlbum.ToLower() == dupeTrack.mAlbum.ToLower()
                        )//&& (!isPurchasedAACAudioFile || (isPurchasedAACAudioFile && (dupeTrack.mLocation.Length > 0))))
                    {
                        actualDupes.Add(dupeTrack);
                    }
                }

                if (actualDupes.Count > 0)
                {
                    actualDupes.Add(track.Value);
                    builder.AppendLine($"Duplicate Tracks Found: {track.Value.mName}");
                    bool differentKind = false;

                    foreach (var dupe in actualDupes)
                    {
                        dupe.DupeCounted = true;
                        builder.AppendLine($"\tId {dupe.mId};{dupe.mKind}: {dupe.mLocation}");

                        if (dupe.mKind != track.Value.mKind)
                        {
                            differentKind = true;
                        }
                    }

                    if (differentKind)
                    {
                        builder.AppendLine($"");
                        builder.AppendLine($"\tDifferent Kind Detected!");
                    }
                }
            }

            File.WriteAllText("iTunesDupes.txt", builder.ToString());
        }
    }
}