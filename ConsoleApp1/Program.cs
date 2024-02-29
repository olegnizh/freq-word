using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;


class Program
{
	static void Main()
	{

		var words = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
		var FreqWords = new List<FreqWord>();

		string path = @"..\..\..\..\Text1.txt";
		words = GetWordFrequency(path);

		foreach (var word in words)
		{
			var noPunctuationText = new string(word.Key.Where(c => !char.IsPunctuation(c)).ToArray());
			FreqWord newFreqWord = new FreqWord(word.Value, noPunctuationText);
			AddUnique(newFreqWord, FreqWords);
		}

		var sortedFreqWords = from f in FreqWords
							  orderby f.Freq
							  select f;

		foreach (var p in sortedFreqWords)
			Console.WriteLine($"{p.Freq} - {p.Word}");
		Console.ReadKey();

	}

	private static void AddUnique(FreqWord newFreqWord, List<FreqWord> FreqWords)
	{
		bool alreadyExists = false;
		foreach (var fw in FreqWords)
		{
			if (fw.Freq == newFreqWord.Freq)
			{
				alreadyExists = true;
				break;
			}
		}

		if (!alreadyExists)
			FreqWords.Add(newFreqWord);

	}


	public class FreqWord
	{
		public FreqWord(int freq, string word)
		{
			Word = word;
			Freq = freq;
		}

		public string Word { get; }
		public int Freq { get; }

	}

	static private Dictionary<string, int> GetWordFrequency(string file)
	{
		return File.ReadLines(file)
				   .SelectMany(x => x.Split())
				   .Where(x => x != string.Empty)
				   .GroupBy(x => x)
		           .ToDictionary(x => x.Key, x => x.Count());
	}



}
