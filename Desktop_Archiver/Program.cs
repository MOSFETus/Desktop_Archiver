using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Desktop_Archiver
{
    class Program
    {
        //Import all files (full path name) from a directory (excluding folders) into an array
        private static string[] ProcessFiles(string path)
        {
            //Directory.GetFiles = Directory.EnumerateFiles but EnumFiles avoids memory issues with .NET 4 and above
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)    
            .Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".mp4") || s.ToLower().EndsWith(".zip") || s.ToLower().EndsWith(".gif") || s.ToLower().EndsWith(".pdf") || s.ToLower().EndsWith(".txt") || s.ToLower().EndsWith(".mp3") || s.ToLower().EndsWith(".rar") || s.ToLower().EndsWith(".webp") || s.ToLower().EndsWith(".webm"));
            //[DYNAMIC] if needed just add new file format in the same fashion here
            return files.ToArray();
        }

        private static string[] ProcessMusicFiles(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.ToLower().EndsWith(".mp3") || s.ToLower().EndsWith(".xm"));
            return files.ToArray();
        }
        static void Main(string[] args)
        {
            //variables
            string originalName, extension, finalDirDestination, finalDirMusic;
            int musicFolder = 15;

            //sets destination folder in following format: "Desktop YYYY.MM.DD"
            string onlyDate = DateTime.Now.ToString();
            string myFolderFormat = "Desktop " + onlyDate.Substring(6, 4) + "." + onlyDate.Substring(3, 2) + "." + onlyDate.Substring(0, 2);
            
            //paths
            string destinationDir = @"E:\Old Files\" + myFolderFormat + @"\";
            string sourceDir = Environment.ExpandEnvironmentVariables(@"%homepath%\Desktop\"); //append Environment.ExpandEnvironmentVariables(string) for %username%
            string musicDir = @"E:\Musik\Playlist Youtube Part " + musicFolder.ToString() + @"\";

            //[DYNAMIC] if needed, just append new file format here
            string[] extArray = new string[] { "jpg", "jpeg", "png", "mp4", "zip", "gif", "pdf", "txt", "mp3", "rar", "webp", "webm" };

            //indexes: 0:jpg  1:jpeg  2:png   3:mp4   4:zip   5:gif   6:pdf   7:txt   8:mp3   9:rar   10:webp   11:webm
            int[] cArray = new int[extArray.Length];
            string[] originFilePath = ProcessFiles(sourceDir);

            try
            {
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                    Console.WriteLine("Directory was successfully created at {0}.\n", Directory.GetCreationTime(destinationDir));
                }
                else
                {
                    Console.WriteLine("Directory already exists, files will be copied anyway\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed:\t{0}", e.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }

            //main processing loop
            for (int i = 0; i < originFilePath.Length; i++)
            {
                originalName = originFilePath[i].Substring(21);
                extension = originFilePath[i].Substring(originFilePath[i].LastIndexOf('.') + 1);
                finalDirDestination = destinationDir + originalName;
                
                //[DYNAMIC] if needed, add new cases for new file formats here
                switch (extension)
                {
                    case "jpg":
                        cArray[0]++;
                        break;

                    case "jpeg":
                        cArray[1]++;
                        break;

                    case "png":
                        cArray[2]++;
                        break;

                    case "mp4":
                        cArray[3]++;
                        break;

                    case "zip":
                        cArray[4]++;
                        break;

                    case "gif":
                        cArray[5]++;
                        break;

                    case "pdf":
                        cArray[6]++;
                        break;

                    case "txt":
                        cArray[7]++;
                        break;

                    case "mp3":
                        cArray[8]++;
                        break;

                    case "rar":
                        cArray[9]++;
                        break;

                    case "webp":
                        cArray[10]++;
                        break;

                    case "webm":
                        cArray[11]++;
                        break;

                    default:
                        break;
                }
                //mp3 files are being moved to different path and checks if currently selected mp3 subfolder has less than 25 files. If it exceeds 25 then copy new mp3 files to new mp3 folder.
                if(extension == "mp3")
                {
                    if (ProcessMusicFiles(musicDir).Length <= 25)
                    {
                        finalDirMusic = musicDir + originalName;
                        File.Copy(originFilePath[i], finalDirMusic, true);
                        File.Delete(originFilePath[i]);
                    }
                    else
                    {
                        musicFolder++;
                        musicDir = @"E:\Musik\Playlist Youtube Part " + musicFolder.ToString() + @"\";

                        if (!Directory.Exists(musicDir))
                        {
                            Directory.CreateDirectory(musicDir);
                            Console.WriteLine("Music Directory was successfully created at {0}.\n", Directory.GetCreationTime(musicDir));
                        }
                        else
                        {
                            Console.WriteLine("Music Directory already exists, files will be copied anyway\n");
                        }

                        finalDirMusic = musicDir + originalName;
                        File.Copy(originFilePath[i], finalDirMusic, true);
                        File.Delete(originFilePath[i]);
                    }
                }
                else
                {
                    File.Copy(originFilePath[i], finalDirDestination, true);
                    File.Delete(originFilePath[i]);
                }
            }

            //console output
            Console.WriteLine("Success!\n");
            Console.Write("Success! All files moved to designated directory.\n\n");
            for(int i = 0; i < cArray.Length; i++)
            {
                Console.Write("{0}:\t{1}\n", extArray[i], cArray[i]);
            }
            Console.WriteLine("\nTotal:\t{0}\n", cArray.Sum());
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
    }
}
