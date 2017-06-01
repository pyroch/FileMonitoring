using System;
using System.IO;
using System.Threading;

namespace FileMonitoring
{
	class MainClass
	{
		static bool flag_OW = false; // Flag for owerwriting

		static string dir; // Directory which is checked
		static string bak_dir; // Directory of backup files
		const string prefix = ".bak"; // Prefix for backup files


		static void Init()
		{
			Console.WriteLine("Choose a new directory? (N - no)");
			if (Console.ReadLine() == "N" && File.Exists(@"./settings"))
			{
				using (StreamReader sr = new StreamReader(@"./settings", System.Text.Encoding.Default))
				{
					dir = sr.ReadLine();
					bak_dir = sr.ReadLine();
				}
			}
			else
			{
				Console.WriteLine("Type directory what to be monitored");
				dir = Console.ReadLine();
				Console.WriteLine("Type directory for backup files");
				bak_dir = Console.ReadLine();
				bak_dir += @"\";

				Console.WriteLine("Add owerwrite flag? (Y - yes)");
				if (Console.ReadLine() == "Y")
					flag_OW = true;
				else
					flag_OW = false;
					

				using (StreamWriter sw = new StreamWriter(@"./settings", false, System.Text.Encoding.Default))
				{
					sw.WriteLine(dir);
					sw.WriteLine(bak_dir);
				}
			}
		}

		static void CheckChange(FileInfo[] files)
		{
			bool isChanged = false;

			if (!Directory.Exists(bak_dir)) Directory.CreateDirectory(bak_dir);  // Checking backup directory
			while (true)
			{
				DirectoryInfo info2 = new DirectoryInfo(dir);
				FileInfo[] files2 = info2.GetFiles();

				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].LastWriteTime != files2[i].LastWriteTime) // If new file != old file ->>
					{
						string fullName = bak_dir + files2[i].Name + prefix;
						if (File.Exists(fullName) && !flag_OW) // If file exist, and has no flag OW, starting for cycle where adding num of file in prefix
						{
							for (int j = 1; j <= int.MaxValue; j++)
							{
								if (!File.Exists(fullName + j)) // If file with num not exist, backuping
								{
									files2[i].CopyTo(fullName + j, true);
									break;
								}
							}
						}
						else
						{
							files2[i].CopyTo(fullName, true); // Backuping, if its new file in backup dir
						}

						isChanged = true; // bool for breaking while cycle
						break;
					}
				}
				if (isChanged) break;

				Thread.Sleep(1000);
			}
		}

		public static void Main(string[] args)
		{
			Init();

			foreach (var args_s in args) // If Main receivec argument -OW, bak files will be owerwritten
				if (args_s == "-OW")
					flag_OW = true;
			
			while (true)
			{
				DirectoryInfo info = new DirectoryInfo(dir);
				FileInfo[] files = info.GetFiles();
				CheckChange(files);
			}
		}
	}
}