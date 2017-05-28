using System;
using System.IO;
using System.Threading;

namespace FileMonitoring
{
	class MainClass
	{
		const string dir = "D:\\Documents\\C";
		const string bak_dir = "\\copy\\";
		const string prefix = ".bak";

		static void CheckChange(FileInfo[] files)
		{
			bool isChanged = false;

			if (!Directory.Exists(dir + bak_dir)) Directory.CreateDirectory(dir + bak_dir);
			while (true)
			{
				DirectoryInfo info2 = new DirectoryInfo(dir);
				FileInfo[] files2 = info2.GetFiles();

				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].LastWriteTime != files2[i].LastWriteTime)
					{
						if (File.Exists(dir + bak_dir + files2[i].Name + prefix))
						{
							for (int j = 0; j <= int.MaxValue; j++)
							{
								if (!File.Exists(dir + bak_dir + files2[i].Name + prefix + j))
								{
									files2[i].CopyTo(dir + bak_dir + files2[i].Name + prefix + j, true);
									break;
								}
							}
						}
						else
						{
							files2[i].CopyTo(dir + bak_dir + files2[i].Name + prefix, true);
						}
						isChanged = true;
						break;
					}
				}
				if (isChanged) break;

				Thread.Sleep(50);
			}
		}

		public static void Main(string[] args)
		{
			while (true)
			{
				DirectoryInfo info = new DirectoryInfo(dir);
				FileInfo[] files = info.GetFiles();
                CheckChange(files);
			}
		}
	}
}