using System;
using System.IO;
using System.Threading;

namespace FileMonitoring
{
	class MainClass
	{
		static void CheckChange(FileInfo[] files)
		{
			bool isChanged = false;
			string dir = "c:\\test";
			if (!Directory.Exists(dir + "\\copy"))
				Directory.CreateDirectory(dir + "\\copy");
			while (true)
			{
				DirectoryInfo info2 = new DirectoryInfo(dir);
				FileInfo[] files2 = info2.GetFiles();

				//foreach (var item2 in files2 files)
				for (int i = 0; i < files.Length; i++)
					if (files[i].LastWriteTime != files2[i].LastWriteTime)
					{
						Console.WriteLine("Izmenilsya " + files2[i].Name);
						files2[i].CopyTo(dir + "\\copy\\" + files2[i].Name, true);
						isChanged = true;
						break;
					}
				if (isChanged)
					break;
				Thread.Sleep(50);
			}
					
		}
		public static void Main(string[] args)
		{
			while (true)
			{
				DirectoryInfo info = new DirectoryInfo("c:\\test");
				FileInfo[] files = info.GetFiles();
                CheckChange(files);
			}
		}
	}
}
