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
		public const string Txt = ".txt";
		public static string _customExtension;

		static void Main(string[] args)
		{
			if (args.Length > 0)
			{
				_customExtension = args[0];
			}
			
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
					Console.WriteLine("Could not find file {0}", Path.GetFileName(fileName));
					continue;
				}

				if (fileName.EndsWith(Exe) || fileName.EndsWith(Wpl) || fileName.EndsWith(Txt))
				{
					continue;
				}

				MoveToFile(basePath, fileName, index);

				index++;
			}

			Console.WriteLine("Renamed {0} files.", index);
		}

		private static void MoveToFile(string basePath, string fileName, int index)
		{
			var extension = string.IsNullOrEmpty(_customExtension) ? Path.GetExtension(fileName) : _customExtension;
			var destPath = GetPath(basePath, index, extension);
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
