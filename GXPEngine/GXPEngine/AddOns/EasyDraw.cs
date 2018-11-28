using System.Drawing;
using System.Drawing.Text;

namespace GXPEngine 
{
	public enum CenterMode {Min, Center, Max}

	/// <summary>
	/// Creates an easy-to-use layer on top of .NET's System.Drawing methods.
	/// The API is inspired by Processing: internal states are maintained for font, fill/stroke color, etc., 
	/// and everything works with simple methods that have many overloads.
	/// </summary>
	public class EasyDraw : Canvas 
	{
		static Font defaultFont = new Font ("Noto Sans", 15);

		public Font font		{ get; protected set;}
		public Pen pen			{ get; protected set;}
		public SolidBrush brush	{ get; protected set;}
		protected bool _stroke=true;
		protected bool _fill=true;
		protected CenterMode _horizontal=CenterMode.Min;
		protected CenterMode _vertical=CenterMode.Max;
		protected CenterMode _horizontalShape=CenterMode.Center;
		protected CenterMode _verticalShape=CenterMode.Center;

		public EasyDraw (int width, int height) : base (new Bitmap (width, height))
		{
			Initialize ();
		}

		public EasyDraw (System.Drawing.Bitmap bitmap) : base (bitmap)
		{
			Initialize ();
		}

		public EasyDraw (string filename) : base(filename)
		{
			Initialize ();
		}

		void Initialize() 
		{
			pen = new Pen (Color.White, 1);
			brush = new SolidBrush (Color.White);
			font = defaultFont;
			graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; //AntiAlias;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		}

		//////////// Setting Font

		public void TextFont(Font newFont) 
		{
			font = newFont;
		}

		public void TextFont(string fontName, float pointSize) 
		{
			font = new Font (fontName, pointSize);
		}

		public void TextFont(string fontName, float pointSize, FontStyle style) 
		{
			font = new Font (fontName, pointSize, style);
		}

		public void TextSize(float pointSize) 
		{
			font = new Font (font.OriginalFontName, pointSize, font.Style);
		}

		//////////// Setting Alignment for text, ellipses and rects
		 
		public void TextAlign(CenterMode horizontal, CenterMode vertical) 
		{
			_horizontal = horizontal;
			_vertical = vertical;
		}

		public void ShapeAlign(CenterMode horizontal, CenterMode vertical) 
		{
			_horizontalShape = horizontal;
			_verticalShape = vertical;
		}

		//////////// Setting Stroke

		public void NoStroke() 
		{
			_stroke=false;
		}

		public void Stroke(Color newColor) 
		{
			pen.Color = newColor;
			_stroke = true;
		}

		public void Stroke(Color newColor, int alpha) 
		{
			pen.Color = Color.FromArgb (alpha, newColor);
			_stroke = true;
		}

		public void Stroke(int grayScale) 
		{
			pen.Color = Color.FromArgb (255, grayScale, grayScale, grayScale);
			_stroke = true;
		}

		public void Stroke(int grayScale, int alpha) 
		{
			pen.Color = Color.FromArgb (alpha, grayScale, grayScale, grayScale);
			_stroke = true;
		}

		public void Stroke(int red, int green, int blue) 
		{
			pen.Color = Color.FromArgb (255, red, green, blue);
			_stroke = true;
		}

		public void Stroke(int red, int green, int blue, int alpha) 
		{
			pen.Color = Color.FromArgb (alpha, red, green, blue);
			_stroke = true;
		}

		public void StrokeWeight(float width) 
		{
			pen.Width = width;
			_stroke = true;
		}

		//////////// Setting Fill

		public void NoFill() 
		{
			_fill = false;
		}

		public void Fill(Color newColor) 
		{
			brush.Color = newColor;
			_fill = true;
		}

		public void Fill(Color newColor, int alpha) 
		{
			brush.Color = Color.FromArgb (alpha, newColor);
			_fill = true;
		}

		public void Fill(int grayScale) 
		{
			brush.Color = Color.FromArgb (255, grayScale, grayScale, grayScale);
			_fill = true;
		}

		public void Fill(int grayScale, int alpha) 
		{
			brush.Color = Color.FromArgb (alpha, grayScale, grayScale, grayScale);
			_fill = true;
		}

		public void Fill(int red, int green, int blue) 
		{
			brush.Color = Color.FromArgb (255, red, green, blue);
			_fill = true;
		}

		public void Fill(int red, int green, int blue, int alpha) 
		{
			brush.Color = Color.FromArgb (alpha, red, green, blue);
			_fill = true;
		}

		//////////// Draw & measure Text
		 
		public void Text(string text, float x, float y) 
		{
			float twidth,theight;
			TextDimensions (text, out twidth, out theight);
			if (_horizontal == CenterMode.Max) 
			{
				x -= twidth;
			} else if (_horizontal == CenterMode.Center) 
			{ 
				x -= twidth / 2;
			}
			if (_vertical == CenterMode.Max) 
			{
				y -= theight;
			} else if (_vertical == CenterMode.Center) 
			{
				y -= theight / 2;
			}
			graphics.DrawString (text, font, brush, x, y); //left+BoundaryPadding/2,top+BoundaryPadding/2);
		}

		public float TextWidth(string text) 
		{
			SizeF size = graphics.MeasureString (text, font);
			return size.Width;
		}

		public float TextHeight(string text) 
		{
			SizeF size = graphics.MeasureString (text, font);
			return size.Height;
		}

		public void TextDimensions(string text, out float width, out float height) 
		{
			SizeF size = graphics.MeasureString (text, font);
			width = size.Width;
			height = size.Height;
		}

		//////////// Draw Shapes
		 
		public void Rect(float x, float y, float width, float height) {
			ShapeAlign (ref x, ref y, width, height);
			if (_fill) {
				graphics.FillRectangle (brush, x, y, width, height);
			}
			if (_stroke) {
				graphics.DrawRectangle (pen, x, y, width, height);
			}
		}

		public void Ellipse(float x, float y, float width, float height) {
			ShapeAlign (ref x, ref y, width, height);
			if (_fill) {
				graphics.FillEllipse (brush, x, y, width, height);
			}
			if (_stroke) {
				graphics.DrawEllipse (pen, x, y, width, height);
			}
		}

		public void Arc(float x, float y, float width, float height, float startAngleDegrees, float sweepAngleDegrees) {
			ShapeAlign (ref x, ref y, width, height);
			if (_fill) {
				graphics.FillPie (brush, x, y, width, height, startAngleDegrees, sweepAngleDegrees);
			}
			if (_stroke) {
				graphics.DrawArc (pen, x, y, width, height, startAngleDegrees, sweepAngleDegrees);
			}
		}

		public void Line(float x1, float y1, float x2, float y2) {
			if (_stroke) {
				graphics.DrawLine (pen, x1, y1, x2, y2);
			}
		}

		public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3) {
			Polygon(x1,y1,x2,y2,x3,y3);
		}

		public void Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
			Polygon(x1,y1,x2,y2,x3,y3,x4,y4);
		}

		public void Polygon(params float[] pt) {
			PointF[] pts = new PointF[pt.Length / 2];
			for (int i = 0; i < pts.Length; i++) {
				pts [i] = new PointF (pt [2 * i], pt [2 * i + 1]);
			}
			Polygon (pts);
		}

		public void Polygon(PointF[] pts) {
			if (_fill) {
				graphics.FillPolygon (brush, pts);
			}
			if (_stroke) {
				graphics.DrawPolygon (pen, pts);
			}
		}

		protected void ShapeAlign(ref float x, ref float y, float width, float height) {
			if (_horizontalShape == CenterMode.Max) 
			{
				x -= width;
			} else if (_horizontalShape == CenterMode.Center) 
			{ 
				x -= width / 2;
			}
			if (_verticalShape == CenterMode.Max) 
			{
				y -= height;
			} else if (_verticalShape == CenterMode.Center) 
			{
				y -= height / 2;
			}
		}
	}
}