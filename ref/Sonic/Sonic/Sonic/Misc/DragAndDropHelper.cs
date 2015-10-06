using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using HundredMilesSoftware.UltraID3Lib;

namespace Sonic
{
    /// <summary>
    /// File types for Drag operation
    /// </summary>
    public enum FileType { Audio, NotSupported }

    /// <summary>
    /// Drag and drop helper
    /// </summary>
    public class DragAndDropHelper
    {

        #region Public Methods
        /// <summary>
        /// Do Drop, which will stored the items in the database
        /// </summary>
        public void Drop(DragEventArgs e)
        {

            try
            {
                e.Effects = DragDropEffects.None;

                string[] fileNames = 
                    e.Data.GetData(DataFormats.FileDrop, true) 
                        as string[];

                //is it a directory, get the files and check them
                if (Directory.Exists(fileNames[0]))
                {
                    string[] files = Directory.GetFiles(fileNames[0]);
                    AddFilesToDatabase(files);
                }
                //not a directory so assume they are individual files
                else
                {
                    AddFilesToDatabase(fileNames);
                }
            }
            catch
            {
                e.Effects = DragDropEffects.None;
            }
            finally
            {
                // Mark the event as handled, so control's native 
                //DragOver handler is not called.
                e.Handled = true;
            }
        }

        /// <summary>
        /// Show the Copy DragDropEffect if files are supported
        /// </summary>
        public void DragOver(DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;

                string[] fileNames = 
                    e.Data.GetData(DataFormats.FileDrop, true) 
                        as string[];

                //is it a directory, get the files and check them
                if (Directory.Exists(fileNames[0]))
                {
                    string[] files = Directory.GetFiles(fileNames[0]);
                    CheckFiles(files, e);
                }
                //not a directory so assume they are individual files
                else
                {
                    CheckFiles(fileNames, e);
                }
            }
            catch
            {
                e.Effects = DragDropEffects.None;
            }
            finally
            {
                // Mark the event as handled, so control's native 
                //DragOver handler is not called.
                e.Handled = true;
            }
        }

        /// <summary>Returns the FileType </summary>
        /// <param name="fileName">Path of a file.</param>
        public FileType GetFileType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLower();

            if (extension == ".mp3")
                return FileType.Audio;

            return FileType.NotSupported;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Checks that the files being dragged are valid
        /// </summary>
        private void CheckFiles(string[] files, DragEventArgs e)
        {
            foreach (string fileName in files)
            {
                FileType type = GetFileType(fileName);

                // Only Image files are supported
                if (type == FileType.Audio)
                    e.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Adds dragged files to database
        /// </summary>
        /// <param name="files"></param>
        private void AddFilesToDatabase(String[] files)
        {
            SQLMP3sDataContext datacontext = new SQLMP3sDataContext();

            try
            {
                foreach (string fileName in files)
                {
                    FileType type = GetFileType(fileName);

                    // Handles image files
                    if (type == FileType.Audio)
                    {
                        MP3 mp3File = XMLAndSQLQueryOperations.
                            ProcessSingleMP3File(fileName);
                        if (mp3File != null)
                        {
                            datacontext = new SQLMP3sDataContext();

                            //does it already exist in the database, if it does return
                            if (datacontext.MP3s.Where(mp3 =>
                                mp3.Album == mp3File.Album).Count() > 0)
                                return;

                            //Doesn't exist so add it in to DB
                            datacontext.MP3s.InsertOnSubmit(mp3File);
                            datacontext.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Oooops, something went wrong reading file
                //not much we can do about it, just skip it
            }
        }
        #endregion
    }
}
