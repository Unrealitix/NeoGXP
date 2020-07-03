using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace GXPEngine { 
	class FontLoader {
		static Dictionary<string, PrivateFontCollection> fontIndex=null;

		public static Font LoadFont(string filename, float fontSize, FontStyle fontStyle = FontStyle.Regular) {
			if (fontIndex==null) {
				fontIndex=new Dictionary<string, PrivateFontCollection>();
			}
			if (!fontIndex.ContainsKey(filename)) {
				fontIndex[filename]=new PrivateFontCollection();
				fontIndex[filename].AddFontFile(filename);
				//Console.WriteLine("Loaded new font: "+fontIndex[filename].Families[0]);
			} 
			return new Font(fontIndex[filename].Families[0], fontSize, fontStyle);
		}
	}
}
