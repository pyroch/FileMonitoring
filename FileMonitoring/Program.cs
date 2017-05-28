using System;
using System.IO;
using System.Threading;

namespace FileMonitoring
{
	class MainClass
	{
		const string dir = "D:\\Documents\\C";

		static void CheckChange(FileInfo[] files)
		{
			bool isChanged = false;

			if (!Directory.Exists(dir + "\\copy")) Directory.CreateDirectory(dir + "\\copy");
			while (true)
			{
				DirectoryInfo info2 = new DirectoryInfo(dir);
				FileInfo[] files2 = info2.GetFiles();

				for (int i = 0; i < files.Length; i++)
					if (files[i].LastWriteTime != files2[i].LastWriteTime)
					{
						files2[i].CopyTo(dir + "\\copy\\" + files2[i].Name, true);
						isChanged = true;
						break;
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
