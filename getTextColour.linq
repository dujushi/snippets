<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	getTextColour("ffffff").Dump("Text Colour");
}
/*
 * text colour based on current background colour
 * Ref: http://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color/33684421#33684421
 */
string getTextColour(string backgroundColour)
{
	int rgb;
	if (Int32.TryParse(backgroundColour, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out rgb))
	{
		double r = (double) (rgb >> 16 & (uint) byte.MaxValue);
		double g = (double) (rgb >> 8 & (uint) byte.MaxValue);
		double b = (double) (rgb & (uint) byte.MaxValue);
		var sRGB = (new double[] {r, g , b}).Select(
			c => {
				c = c / 255.0;
				if (c <= 0.03928)
				{
					c = c / 12.92;
				}
				else
				{
					c = Math.Pow((c + 0.055) / 1.055, 2.4);
				}
				return c;
			}
		).ToArray();
		double luminance = 0.2126 * sRGB[0] + 0.7152 * sRGB[1] + 0.0722 * sRGB[2];
		if (luminance > 0.179)
		{
			return "000000";
		}
	}
	return "ffffff";
}
