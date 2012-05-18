using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;

namespace CSharp___DllImport
{
    public static partial class Phone
    {
        public static class IO
        {
            public static bool DirectoryExists(string fullPath)
            {
                return DllImportCaller.lib.StringCall("shcore", "DirectoryExists", fullPath) == 1;
            }
            [Obsolete("Fails for now. Always returns -Int32.MinValue", true)]
            public static object CreateDirectoryPath(string fullPath)
            {
                return DllImportCaller.lib.CreateDirectoryPath7(fullPath);//StringCall("shcore", "CreateDirectoryPath", fullPath);
            }

            /// <param name="smalFileName">Hello.txt for example</param>
            /// <returns></returns>
            public static bool PathIsValidFileName(string smalFileName)
            {
                //Phone.IO.PathIsValidFileName("asdjaskdjkasd¤%//{{{{]]{]}}][[}];;:::,.,.//*-**++¨´¨´¨'´''-,____½§§.//676210"); (false)
                //Phone.IO.PathIsValidFileName("hello.lol"); (true)
                
                return DllImportCaller.lib.StringCall("shcore", "PathIsValidFileName", smalFileName /* Hello.exe */) == 1;
            }
            public static bool PathIsValidPath(string fullPath)
            {
                //Phone.IO.PathIsValidPath("//\\85*/*/-%&/()¨¨¨++'''¨'´`´´"); (false)
                //Phone.IO.PathIsValidPath("\\ABC123\\"); (true)

                return DllImportCaller.lib.StringCall("shcore", "PathIsValidPath", fullPath) == 1;
            }
            public static class Storage
            {
                public static bool PathIsRemovableDevice(string fullPath)
                {
                    /*Phone.IO.Storage.PathIsRemovableDevice("\\"); (false) 
                    (lol, HDD cant be removable, y sure??..., just smash the device in the floor and its out)*/

                    return DllImportCaller.lib.StringCall("shcore", "PathIsRemovableDevice", fullPath) == 1;
                }
            }
            
            public class File7 : IDisposable
            {
                //http://sharpallegro.googlecode.com/svn-history/r98/allegro/trunk/examples/expackf.cs

                public const int EOF = -1;

                public int Handle { get; private set; }
                public bool Disposed { get; private set; }
                
                private File7(int handle)
                {
                    this.Handle = handle;
                }

                private void checkDisposed()
                {
                    if (Disposed) throw new ObjectDisposedException("File7");
                }

                public static byte[] ReadAllBytes(string fullpath)
                {
                    var f = Phone.IO.File7.Open(fullpath, "rb");

                    int c = f.Getc();

                    var va = new System.Collections.Generic.List<byte>();

                    while (c != Phone.IO.File7.EOF)
                    {
                        va.Add((byte)c);
                        c = f.Getc();
                    }

                    //var data = va.Select(a => (char)a).ToArray();
                    //string r = new string(data);

                    f.Close();

                    return va.ToArray();
                }

                public static string ReadAllString(string fullpath)
                {
                    var f = Phone.IO.File7.Open(fullpath, "r");

                    int c = f.Getc();

                    var va = new System.Collections.Generic.List<byte>();

                    while (c != Phone.IO.File7.EOF)
                    {
                        va.Add((byte)c);
                        c = f.Getc();
                    }

                    var data = va.Select(a => (char)a).ToArray();
                    string r = new string(data);

                    f.Close();

                    return r;
                }

                public static File7 Open(string fullPath, string mode)
                {
                    int handle = DllImportCaller.lib.fopen7(fullPath, mode);
                    if (handle == 0)
                    {
                        throw new Exception(DllImportCaller.LastError().ToString());
                    }

                    return new File7(handle);
                }

                public void Seek(long offset, int origin)
                {
                    checkDisposed();

                    DllImportCaller.lib.fseek7(Handle, offset, origin);
                }

                public int Getc()
                {
                    checkDisposed();

                    return DllImportCaller.lib.fgetc7(Handle);
                }

                public void Putc(char value)
                {
                    checkDisposed();

                    var sameBack = DllImportCaller.lib.fputc7(Handle, value);
                    
                    if (value != sameBack)
                    {
                        throw new Exception("Error writing to file");
                    }
                }

                public void Close()
                {
                    if (!Disposed)
                    {
                        Disposed = true;
                        var ret = DllImportCaller.lib.fclose7(Handle);
                        Handle = 0;
                    }
                }

                public int GetFileSize()
                {
                    checkDisposed();

                    int val = DllImportCaller.lib.fsize7(Handle);

                    if (val == -1)
                    {
                        throw new Exception(DllImportCaller.LastError().ToString());
                    }

                    return val;
                }

                ~File7()
                {
                    Close();
                }

                public void Dispose()
                {
                    checkDisposed();

                    Close();
                }
            }


            public class FilePointer
            {
                public string FullFileName;
                public File7 GetFile(string mode)
                {
                    return File7.Open(FullFileName, mode);
                }
                public string FileName
                {
                    get
                    {
                        return System.IO.Path.GetFileName(FullFileName);
                    }
                }
            }

            public class DirectoryPointer
            {
                public string FullDirectoryName;
                public Directory GetDirectory()
                {
                    return Directory.OpenDirectory(FullDirectoryName);//new  .Open(FullDirectoryName, mode);
                }
            }

            public class Directory
            {
                public string FolderFullPath { get; private set; }

                private Directory(string fullFolder)
                {
                    this.FolderFullPath = fullFolder;
                }

                public static Directory OpenDirectory(string fullFolderName)
                {
                    return new Directory(fullFolderName);
                }

                public FilePointer[] GetFiles()
                {
                    var dir = Directory7.OpenDirectory(System.IO.Path.Combine(FolderFullPath, "*.*"));

                    return dir
                        .Skip(1)
                        .Where(f => f.RAW.dwFileAttributes != System.IO.FileAttributes.Directory)
                        .Select(f => new FilePointer { FullFileName = System.IO.Path.Combine(FolderFullPath, f.RAW.cFileName) })
                        .ToArray();
                }

                public DirectoryPointer[] GetDirectories()
                {
                    var dir = Directory7.OpenDirectory(System.IO.Path.Combine(FolderFullPath, "*.*"));

                    return dir
                        .Skip(1)
                        .Where(f => f.RAW.dwFileAttributes == System.IO.FileAttributes.Directory)
                        .Select(f => new DirectoryPointer { FullDirectoryName = System.IO.Path.Combine(FolderFullPath, f.RAW.cFileName) })
                        .ToArray();
                }

                public class Directory7
                {
                    public WIN32_FIND_DATA RAW;
                    public string CurrentDirectoryPath;
                    public bool IsThisMap;

                    private Directory7(WIN32_FIND_DATA data, string currMap, bool isThis)
                    {
                        this.RAW = data;
                        this.CurrentDirectoryPath = currMap;
                        this.IsThisMap = isThis;
                    }


                    public static Directory7[] OpenDirectory(string fullFolderName)
                    {
                        WIN32_FIND_DATA data;
                        var handle = DllImportCaller.lib.FindFirstFile7(fullFolderName, out data);

                        if (handle == -1)
                        {
                            var err = DllImportCaller.LastError().ToString();

                            if (err == "-2147483643")
                            {
                                throw new System.IO.IOException("Forbidden or not exist");
                            }
                            else
                            {
                                throw new Exception(err);
                            }
                        }

                        var list = new List<Directory7>();
                        list.Add(new Directory7(data, fullFolderName, true)); //add first defined "out data"


                        int next;

                        while ((next = DllImportCaller.lib.FindNextFile7(handle, out data)) != 0)
                        {
                            list.Add(new Directory7(data, System.IO.Path.Combine(fullFolderName, data.cFileName), false));//continue to add
                        }

                        var r = DllImportCaller.lib.FindClose7(handle);
                        return list.ToArray();
                    }
                    public File7[] GetFiles()
                    {
                        return null;
                    }
                    public Directory7[] GetDirectories()
                    {
                        //var next = System.IO.Path.Combine(CurrentDirectoryPath, "*.*");

                        var q = (from a in OpenDirectory(System.IO.Path.Combine(CurrentDirectoryPath, "*.*")) where a.RAW.dwFileAttributes == System.IO.FileAttributes.Directory select a);
                        return q.ToArray();
                    }
                }
            }

            public static class DebugDir
            {
                public static void DumpWindowsToDebugConsole()
                {
                    WriteDir(new Phone.IO.DirectoryPointer { FullDirectoryName = "\\Windows" }, 0);
                }
                private static void WriteDir(Phone.IO.DirectoryPointer po, int indent)
                {
                    try
                    {
                        var d = po.GetDirectory();

                        try
                        {
                            var files = d.GetDirectories();
                            foreach (var item in files)
                            {
                                System.Diagnostics.Debug.WriteLine(new string('-', indent) + "[DIR]" + item.FullDirectoryName);
                                WriteDir(item, indent + 1);
                            }
                        }
                        catch (Exception) { }

                        try
                        {
                            var files = d.GetFiles();
                            foreach (var item in files)
                            {
                                System.Diagnostics.Debug.WriteLine(new string('-', indent) + item.FullFileName);
                            }
                        }
                        catch (Exception) { }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
