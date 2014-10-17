using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Mp3Renamer
{
	class Program
	{
		public const string Exe = ".exe";
		public const string Wpl = ".wpl";
		public const string Mp3 = ".mp3";
		public const string Wma = ".wma";
		public const string Wav = ".wav";

		static void Main()
		{
			RenameFiles(AppDomain.CurrentDomain.BaseDirectory);
		}

		private static IEnumerable<string> GetFileNames(string basePath)
		{
			var playlistFiles = Directory.GetFiles(basePath, "*" + Wpl, SearchOption.TopDirectoryOnly);

			if (playlistFiles.Length <= 0)
			{
				return Directory.GetFiles(basePath); // No playlist specified
			}

			var fileNames = new List<String>();
			var doc = XDocument.Load(playlistFiles[0]);
			var mediaElements = doc.Descendants("media");
			foreach (var mediaElement in mediaElements)
			{
				var srcAttr = mediaElement.Attribute("src");
				if (srcAttr == null)
				{
					continue;
				}

				var sepIndex = srcAttr.Value.LastIndexOf('\\');
				var file = sepIndex >= 0 ? srcAttr.Value.Substring(sepIndex + 1) : srcAttr.Value;
				fileNames.Add(Path.Combine(basePath, file));
			}

			return fileNames.ToArray();
		}

		private static void RenameFiles(string basePath)
		{
			var index = 0;

			foreach (var fileName in GetFileNames(basePath))
			{
				if (!File.Exists(fileName))
				{
					continue;
				}

				if (fileName.EndsWith(Exe) || fileName.EndsWith(Wpl))
				{
					continue;
				}

				if (fileName.EndsWith(Mp3))
				{
					MoveToFile(basePath, fileName, index, Mp3);
				}
				else if (fileName.EndsWith(Wma))
				{
					MoveToFile(basePath, fileName, index, Wma);
				}
				else if (fileName.EndsWith(Wav))
				{
					MoveToFile(basePath, fileName, index, Wav);
				}
				else
				{
					MoveToFile(basePath, fileName, index, Mp3);
				}

				index++;
			}

			Console.WriteLine("Renamed {0} files.", index);
		}

		private static void MoveToFile(string basePath, string fileName, int index, string extenstion)
		{
			var destPath = GetPath(basePath, index, extenstion);
			File.Move(fileName, destPath);
			Console.WriteLine("{0} -> {1}", Path.GetFileName(fileName), Path.GetFileName(destPath));
		}

		private static string GetPath(string basePath, int index, string extenstion)
		{
			var destPath = Path.Combine(basePath, string.Format("{0:0000}{1}", index, extenstion));
			var extraIndex = 0;
			while (File.Exists(destPath))
			{
				destPath = Path.Combine(basePath, string.Format("{0:0000}_{2}{1}", index, extenstion, extraIndex++));
			}
			return destPath;
		}
	}
}
