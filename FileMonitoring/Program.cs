using System;
using System.IO;
using System.Threading;

namespace FileMonitoring
{
	class MainClass
	{
		static bool flag_OW = false; // Flag for owerwriting

		const string dir = "C:\\Test\\"; // Directory which is checked
		const string bak_dir = "\\copy\\"; // Directory of backup files
		const string prefix = ".bak"; // Prefix for backup files

		static void CheckChange(FileInfo[] files)
		{
			bool isChanged = false;

			if (!Directory.Exists(dir + bak_dir)) Directory.CreateDirectory(dir + bak_dir);  // Checking backup directory
			while (true)
			{
				DirectoryInfo info2 = new DirectoryInfo(dir);
				FileInfo[] files2 = info2.GetFiles();

				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].LastWriteTime != files2[i].LastWriteTime) // If new file != old file ->>
					{
						if (File.Exists(dir + bak_dir + files2[i].Name + prefix) && !flag_OW) // If file exist, and has no flag OW, starting for cycle where adding num of file in prefix
						{
							for (int j = 1; j <= int.MaxValue; j++)
							{
								if (!File.Exists(dir + bak_dir + files2[i].Name + prefix + j)) // If file with num not exist, backuping
								{
									files2[i].CopyTo(dir + bak_dir + files2[i].Name + prefix + j, true);
									break;
								}
							}
						}
						else
						{
							files2[i].CopyTo(dir + bak_dir + files2[i].Name + prefix, true); // Backuping, if its new file in backup dir
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