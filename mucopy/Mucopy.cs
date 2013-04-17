using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Permissions;
using System.Diagnostics;

namespace mucopy
{
    public class MusicWorker
    {
        public void ProcessFolder(string path)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string[] filters = { ".mp3", ".flac", ".m4a", ".wav", ".aac", ".ogg", ".wma" };
            string[] files = Directory.GetFiles(path);
            string target = Path.Combine(args[2], Path.GetFileName(path));
            Console.WriteLine("Copying to " + target);
            foreach (string file in files)
            {
                FileInfo fInfo = new FileInfo(file);
                foreach (string format in filters)
                {   
                    if (fInfo.Extension == format) 
                    {
                        Console.WriteLine("Found music file at " + file);
                        DirectoryInfo dir = new DirectoryInfo(path);
                        string[] mData = Metadata.getMetadata(fInfo);
                        foreach (string m in mData)
                        {
                            Console.WriteLine(m);
                        }
                        if (!Directory.Exists(target))
                        {
                            Directory.CreateDirectory(target);
                        }
                        FileInfo[] sourceFiles = dir.GetFiles();
                        foreach (FileInfo f in sourceFiles)
                        {
                            f.CopyTo(Path.Combine(target, f.Name), false);
                        }
                        return;
                    }
                }
            }
            Console.WriteLine("No music found in " + path);
        }
    }
    public class MusicWatcher
    {
        private static MusicWorker mWorker = new MusicWorker();
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Watch()
        {
            Console.WriteLine("Starting music watcher");
            string[] args = System.Environment.GetCommandLineArgs();

            if (args.Length != 3)
            {
                string sSource;
                string sLog;
                sSource = "mucopy";
                sLog = "Application";

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sLog);

                EventLog.WriteEntry(sSource, "Please run mucopy with the directory path and target path", System.Diagnostics.EventLogEntryType.Error);
            }

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = args[1];
            watcher.NotifyFilter = NotifyFilters.DirectoryName;
            watcher.IncludeSubdirectories = true;
            watcher.Created += new FileSystemEventHandler(onCreated);
            watcher.EnableRaisingEvents = true;
        }

        private static void onCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("New music found: " + e.FullPath);
            mWorker.ProcessFolder(e.FullPath);
        }
    }
    class Mucopy
    {
        static void Main(string[] args)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            MusicWatcher music = new MusicWatcher();
            Thread tMusic = new Thread(music.Watch);
            tMusic.Start();
            tMusic.Join();
            autoEvent.WaitOne();
        }
    }
}
