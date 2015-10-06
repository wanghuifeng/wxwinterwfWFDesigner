using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

//Nice free MP3 API, see UltraID3Lib.dll
using HundredMilesSoftware.UltraID3Lib;

namespace Sonic
{
    public class XMLAndSQLQueryOperations
    {
        private static String combinedXMLPath = String.Empty;

        static XMLAndSQLQueryOperations()
        {
            combinedXMLPath = Path.Combine(Globals.AppLocation, Globals.LibraryFile).ToString();
        }


        public static IQueryable<MP3> GetMatchingMP3Files(Func<MP3, Boolean> filter)
        {
            SQLMP3sDataContext datacontext = new SQLMP3sDataContext();
#if DEBUG
            datacontext.Log = Console.Out;
#endif
            return datacontext.MP3s.Where(filter).AsQueryable();
        }


        public static void CreateDB_MP3Files()
        {
            SQLMP3sDataContext datacontext = new SQLMP3sDataContext();
            datacontext.ExecuteCommand("DELETE FROM MP3");

            foreach (var xmlElement in StreamXmlElements(combinedXMLPath, "MusicLocation"))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(xmlElement.Value);
                    foreach (var fileInfo in di.GetFiles("*.*", SearchOption.AllDirectories))
                    {

                        #region Process Files
    
                        //only return an actual MP3File, basically files on
                        //disk with no ID3 metadata are useless, so don't yield
                        //those items
                        MP3 mp3File = ProcessSingleMP3File(fileInfo.FullName);
                        if (mp3File != null)
                        {
                            //Need to write this to DB
                            datacontext = new SQLMP3sDataContext();
                            datacontext.MP3s.InsertOnSubmit(mp3File);
                            datacontext.SubmitChanges();
                        }
                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    //Oooops, something went wrong reading file
                    //not much we can do about it, just skip it
                }
            }
        }

        /// <summary>
        /// Obtain the ID3 information for the given filename
        /// </summary>
        public static MP3 ProcessSingleMP3File(String fileName)
        {
            MP3 mp3File = null;

            Boolean hasTag = false;

            String album = String.Empty;
            String artist = String.Empty;
            String genreName = String.Empty;
            String title = String.Empty;

            //Use the ID3 Library (and why not)
            UltraID3 readMP3File = new UltraID3();
            readMP3File.Read(fileName);

            //check for ID3 v2 Tag 1st
            if (readMP3File.ID3v2Tag.ExistsInFile)
            {
                hasTag = true;
                album = readMP3File.ID3v2Tag.Album;
                artist = readMP3File.ID3v2Tag.Artist;
                genreName = readMP3File.ID3v2Tag.Genre;
                title = readMP3File.ID3v2Tag.Title;
            }

            //check for ID3 v1 Tag
            if (readMP3File.ID3v1Tag.ExistsInFile && !hasTag)
            {
                hasTag = true;
                album = readMP3File.ID3v1Tag.Album ?? "Uknown";
                artist = readMP3File.ID3v1Tag.Artist ?? "Uknown";
                genreName = readMP3File.ID3v1Tag.GenreName ?? "Uknown";
                title = readMP3File.ID3v1Tag.Title ?? "Uknown";
            }

            //Only create an actual MP3File if we actually found
            //a ID3 Tag
            if (hasTag)
            {
                mp3File = new MP3();
                mp3File.FileName = fileName;
                mp3File.Album = album;
                mp3File.Artist = artist;
                mp3File.GenreName = genreName;
                mp3File.Title = title;
            }

            return mp3File;

        }



        public static IEnumerable<XElement> StreamXmlElements(String uri, String name)
        {
            using (XmlReader reader = XmlReader.Create(uri))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) &&
                      (reader.Name == name))
                    {
                        XElement element = (XElement)XElement.ReadFrom(reader);
                        yield return element;
                    }
                }
                reader.Close();
            }
        }


        public static void PopulateXmlWithLibraryDirectories()
        {
            try
            {
                XNamespace ns = "http:sachabarber.net/Music";
                XElement xmlRoot = new XElement(ns + "Music");

                if (File.Exists(combinedXMLPath))
                    File.Delete(combinedXMLPath);

                foreach (var musicLocation in Globals.MusicLocations)
                {
                    DirectoryInfo di = new DirectoryInfo(musicLocation);
                    DirectoryInfo[] disFound = 
                        di.GetDirectories("*.*", SearchOption.AllDirectories);
                    
                    foreach (var info in disFound)
                    {
                        //Write entry to xml file
                        xmlRoot.Add(
                            new XElement("MusicLocation", info.FullName.ToString()));
                    }
                }
                xmlRoot.Save(combinedXMLPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ooops, its busted " + ex.Message);
            }
        }




    }
}
