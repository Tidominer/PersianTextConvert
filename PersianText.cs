//https://github.com/Tidominer/PersianTextConvert

using UnityEngine;

// ReSharper disable StringLiteralTypo

namespace Tidominer
{
	public class PersianText : MonoBehaviour
	{
		//default Values:
		static int _eNumbers = 1; //enable/disable arabic numbers: [0, 1]
		static int _fNumbers = 1; //enable/disable persian numbers: [0, 1]
		static int _eHarakat = 1; //enable/disable arabic harakat: [0, 1]

		//initialize global vars:
		static readonly int LAIndex = 168; //position of laa characters in the unicode string

		static readonly string Left = "ڤـئظشسيیبلپتنمكکگطضصثقفغعهخچحج"; //defining letters that can connect from the left

		static readonly string Right =
			"ڤـئؤرلالآىیآةوزژظشسيپبللأاأتنمكکگطضصثقفغعهخحچجدذلإإ"; //defining letters that can connect from the right

		static readonly string ArNumbs = "٠١٢٣٤٥٦٧٨٩";
		static readonly string FaNumbs = "۰۱۲۳۴۵۶۷۸۹";
		static readonly string EnNumbs = "0123456789";
		static readonly string Harakat = "ًٌٍَُِّْ"; //defining the harakat
		static readonly string Symbols = "ـ.،؟ @#$%^&*-+|\\/=~,:"; //defining other symbols

		static readonly string Unicode =
			"ﺁﺁﺂﺂ" + "ﺃﺃﺄﺄ" + "ﺇﺇﺈﺈ" + "ﺍﺍﺎﺎ" + "ﺏﺑﺒﺐ" + "ﺕﺗﺘﺖ" + "ﺙﺛﺜﺚ" + "ﺝﺟﺠﺞ" + "ﺡﺣﺤﺢ" + "ﺥﺧﺨﺦ" +
			"ﺩﺩﺪﺪ" + "ﺫﺫﺬﺬ" + "ﺭﺭﺮﺮ" + "ﺯﺯﺰﺰ" + "ﺱﺳﺴﺲ" + "ﺵﺷﺸﺶ" + "ﺹﺻﺼﺺ" + "ﺽﺿﻀﺾ" + "ﻁﻃﻄﻂ" + "ﻅﻇﻈﻆ" +
			"ﻉﻋﻌﻊ" + "ﻍﻏﻐﻎ" + "ﻑﻓﻔﻒ" + "ﻕﻗﻘﻖ" + "ﻙﻛﻜﻚ" + "ﻝﻟﻠﻞ" + "ﻡﻣﻤﻢ" + "ﻥﻧﻨﻦ" + "ﻩﻫﻬﻪ" + "ﻭﻭﻮﻮ" +
			"ﻱﻳﻴﻲ" + "ﺓﺓﺔﺔ" + "ﺅﺅﺆﺆ" + "ﺉﺋﺌﺊ" + "ﻯﻯﻰﻰ" + "گﮔﮕﮓ" + "چﭼﭽﭻ" + "پﭘﭙﭗ" + "ژﮊﮋﮋ" + "ﯼﯾﯿﯽ" +
			"کﮐﮑﮏ" + "ﭪﭬﭭﭫ" + "ﻵﻵﻶﻶ" + "ﻷﻷﻸﻸ" + "ﻹﻹﻺﻺ" +
			"ﻻﻻﻼﻼ"; //defining arabic and persian unicode chars(individual, start, middle, end)

		static readonly string Arabic =
			"آ" + "أ" + "إ" + "ا" + "ب" + "ت" + "ث" + "ج" + "ح" + "خ" +
			"د" + "ذ" + "ر" + "ز" + "س" + "ش" + "ص" + "ض" + "ط" + "ظ" +
			"ع" + "غ" + "ف" + "ق" + "ك" + "ل" + "م" + "ن" + "ه" + "و" +
			"ي" + "ة" + "ؤ" + "ئ" + "ى" + "گ" + "چ" + "پ" + "ژ" + "ی" +
			"ک" + "ڤ";

		static readonly string NotEng = Arabic + Harakat + "ء،؟"; //defining all arabic letters + harakat + arabic symbols
		static readonly string Brackets = "(){}[]";

		public static string Convert(string text) //the processing function
		{
			if (text.Length <= 1)
				return text;
			var old = "";
			var tStr = "";
			var y = text;
			var x = y.ToCharArray();
			var len = x.Length;
			int pos = 0;
			string output = "";
			var temp = 0;

			for (int g = 0; g < len; g++) //process each letter, submit it to tests and then add it to the output string
			{
				//ignoring the harakat
				var b = 1;
				var a = 1;
				while (g - b > 0 && g - b < x.Length && Harakat.IndexOf(x[g - b]) >= 0)
					b += 1;
				while (g + a > 0 && g + a < x.Length && Harakat.IndexOf(x[g + a]) >= 0)
					a += 1;
				if (g == 0) //determine the position of each letter
				{
					pos = Right.IndexOf(x[a]) >= 0 ? 1 : 0;
				}
				else if (g == (len - 1))
				{
					pos = Left.IndexOf(x[len - b - 1]) >= 0 ? 3 : 0;
				}
				else
				{
					if (Left.IndexOf(x[(g - b)]) < 0)
					{
						if (Right.IndexOf(x[(g + a)]) < 0)
							pos = 0;
						else
							pos = 1;
					}
					else if (Left.IndexOf(x[(g - b)]) >= 0)
					{
						if (Right.IndexOf(x[(g + a)]) >= 0)
							pos = 2;
						else
							pos = 3;
					}
				}

				if (x[g] == '\n') //if new line occurs, save old data in a temp, process new data, then regroup
				{
					old += output + "\n";
					output = "";
				}
				else if (x[g] == '\r') //if this char is carriage return, skip it.
				{
				}
				else if (x[g] == 'ء')
					AddChar('ﺀ');
				else if (Brackets.IndexOf(x[g]) >= 0) //if this char is a bracket, reverse it
				{
					var asd = Brackets.IndexOf(x[g]);
					if ((asd % 2) == 0)
						AddChar(Brackets[asd + 1]);
					else
						AddChar(Brackets[asd - 1]);
				}
				else if (Arabic.IndexOf(x[g]) >= 0) //if the char is an Arabic letter.. convert it to Unicode
				{
					if (x[g] == 'ل') //if this letter is (laam)
					{
						if (g + 1 < x.Length)
						{
							//check if its actually a (laa) combination
							var arPos = Arabic.IndexOf(x[g + 1]);
							//alert(ar_pos)
							if ((arPos >= 0) && (arPos < 4))
							{
								AddChar(Unicode[(arPos * 4) + pos + LAIndex]);
								g += 1;
							}
							else //if its just (laam)
								AddChar(Unicode[(Arabic.IndexOf(x[g]) * 4) + pos]);
						}else //if its just (laam)
							AddChar(Unicode[(Arabic.IndexOf(x[g]) * 4) + pos]);
					}
					else //if its any arabic letter other than (laam)
						AddChar(Unicode[(Arabic.IndexOf(x[g]) * 4) + pos]);
				}
				else if (Symbols.IndexOf(x[g]) >= 0) //if the char is a symbol, add it
					AddChar(x[g]);
				else if (Harakat.IndexOf(x[g]) >= 0) //if the char is a haraka, and harakat are enabled, add it
				{
					if (_eHarakat == 1)
						AddChar(x[g]);
					else
					{
					}
				}
				else if (Unicode.IndexOf(x[g]) >= 0) //if the char is an arabic reversed letter, reverse it back!
				{
					var uniPos = Unicode.IndexOf(x[g]);
					var laPos = Unicode.IndexOf(x[g]);
					if (laPos >= LAIndex) //if its a laa combination
						for (temp = 4; temp < 20; temp += 4) //find which laa
						{
							if (laPos < (temp + LAIndex))
							{
								AddChar(Arabic[(temp / 4) - 1]);
								AddChar('ل');
								temp = 30;
							}
						}
					else //if its any other letter
						for (temp = 4; temp < 180; temp += 4)
						{
							if (uniPos < temp)
							{
								AddChar(Arabic[(temp / 4) - 1]);
								temp = 200;
							}
						}
				}
				else //if the char is none of the above, then treat it as english text (don't reverse) (english chars + numbers + symbols (as is))
				{
					var h = g;
				
					while ((h < x.Length) && (h > 0) && (NotEng.IndexOf(x[h]) < 0) && (Unicode.IndexOf(x[h]) < 0) && (Brackets.IndexOf(x[h]) < 0)) //if this is an english sentence, or numbers, put it all in one string
					{
						if (EnNumbs.IndexOf(x[h]) >= 0)
						{
							var mynumb = EnNumbs.IndexOf(x[h]);

							if (_eNumbers == 1)
							{
								x[h] = ArNumbs[mynumb];
							}
							else if (_fNumbers == 1)
							{
								// AMIB
								x[h] = FaNumbs[mynumb];
							}

						}
						else if (ArNumbs.IndexOf(x[h]) >= 0)
						{
							var mynumb = ArNumbs.IndexOf(x[h]);

							if (_eNumbers == 0)
							{
								x[h] = EnNumbs[mynumb];
							}

						}
						else if (FaNumbs.IndexOf(x[h]) >= 0)
						{
							// AMIB
							var mynumb = ArNumbs.IndexOf(x[h]);

							if (_fNumbers == 0)
							{
								x[h] = EnNumbs[mynumb];
							}

						}

						tStr = tStr + x[h];
						h = h + 1;

						if (x[temp] == '\n')
							break;
					}

					var xstr = tStr.ToCharArray();
					var r = xstr.Length - 1;
					if ((r == 1) && (xstr[1] == ' ')) //make sure spaces between arabic and english text display properly
						tStr = " " + xstr[0];
					else
					{
						while (xstr[r] == ' ')
						{
							tStr = " " + tStr.Substring(0, (tStr.Length - 1));
							r = r - 1;
						}
					}

					output = tStr + output; //put together the arabic text + the new english text
					tStr = "";
					g = h - 1; //set the loop pointer to the first char after the english text.
				}
			}

			output = old + output; //put together the old text and the last sentence
			return output;
		
			void AddChar(char chr) //add arabic chars (change to Unicode)
			{
				output = chr + output;
			}
		}
	}
}
