using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexMusicControl
{
	class Spectrum
	{
		public WriteableBitmap targetBitmap;
		private double radius;
		public List<SpectrumLayer> SpectrumLayerList = new List<SpectrumLayer>();
		public Spectrum(WriteableBitmap source, double radius)
		{
			targetBitmap = source;
			this.radius = radius;
		}

		public void addLayer(float[] amplitudeList, Color color, int lifecount, int fadeSpeed)
		{
			SpectrumLayerList.Add(new SpectrumLayer(targetBitmap, amplitudeList, lifecount, radius, color,
				targetBitmap.Width / 2, targetBitmap.Width / 2, fadeSpeed));
		}

		public void Update()
		{
			int count = 0;
			for (int i = 0;  i < SpectrumLayerList.Count; i++)
			{
				SpectrumLayer temp = SpectrumLayerList.ElementAt(i);
				if (temp.LayerLife == 0)
				{
					SpectrumLayerList.RemoveAt(count);
					i--;
				}
				temp.Update();
			}
		}

		public void Draw()
		{
			foreach (SpectrumLayer i in SpectrumLayerList)
			{
				i.Draw();
			}
		}
	}

	class SpectrumLayer
	{
		public WriteableBitmap targetBitmap;
		public Rectangle[] Rectangles;
		public double radius;
		public int LayerLife;
		public Color LayerColor;
		public double fadeStep;
		public double currentfade;

		public SpectrumLayer(WriteableBitmap targetBitmap, float[] amplitudeList, int lifecount, double radius,
			Color color, double basex, double basey, int fadeSpeed)
		{
			double barwidth = radius * 2 * Math.PI / amplitudeList.Length * 1.2;
			this.radius = radius;
			this.targetBitmap = targetBitmap;
			LayerLife = lifecount;
			LayerColor = color;
			this.currentfade = color.A;
			this.fadeStep = (double)color.A/(double)fadeSpeed;
			double stepAmount = 360 / amplitudeList.Length;
			Rectangles = new Rectangle[amplitudeList.Length];
			for (int i = 0; i < amplitudeList.Length; i++)
			{
				Rectangles[i] = new Rectangle(basex, basey, targetBitmap, barwidth, amplitudeList[i], (i + 1) * stepAmount, color, radius, LayerLife);
			}
		}
		
		// Perform a tick to update spectrums, it does not check the time.
		public void Update()
		{
			LayerLife -= 1;
			currentfade -= fadeStep;
			if (currentfade >= 0)
			{
				for (int i = 0; i < Rectangles.Length; i++)
				{
					Rectangles[i].Update();
					Rectangles[i].color.A = (byte)currentfade;
				}
			}
		}
		
		public void Draw()
		{
			for (int i = 0; i < Rectangles.Length; i++)
			{
				Rectangles[i].Draw();
			}
		}
	}

	class Rectangle
	{
		private WriteableBitmap targetBitmap;
		private double leftcornerx, rightcornerx;
		private double basex, basey;
		public double Height;
		private double cosang, sinang;
		public Color color;
		private int p1x, p1y, p2x, p2y;
		private double baseHeight;
		private double rectangleLife;
		public Rectangle(double basex, double basey, WriteableBitmap writeableBitmap, double width, double Height, double angle, Color color, double baseHeight, double rectangleLife)
		{
			this.rectangleLife = rectangleLife;
			this.baseHeight = baseHeight;
			this.color = color;
			double HalfWidth = width/2;
			this.basex = basex;
			this.basey = basey;
			this.Height = Height;
			this.cosang = Math.Cos(angle);
			this.sinang = Math.Sin(angle);
			leftcornerx = -HalfWidth;
			rightcornerx = HalfWidth;
			targetBitmap = writeableBitmap;

			p1x = (int)(cosang * leftcornerx + basex);
			p1y = (int)(sinang * leftcornerx + basey);
			p2x = (int)(cosang * rightcornerx + basex);
			p2y = (int)(sinang * rightcornerx + basey);
		}

		public void Draw()
		{
			double RectangleHeight = baseHeight + Height * 1000;
			double TempHeight = RectangleHeight;
			int p3x = (int)(cosang * leftcornerx - sinang * TempHeight + basex);
			int p3y = (int)(sinang * leftcornerx + cosang * TempHeight + basey);
			int p4x = (int)(cosang * rightcornerx - sinang * TempHeight + basex);
			int p4y = (int)(sinang * rightcornerx + cosang * TempHeight + basey);
			using(targetBitmap.GetBitmapContext())
			{
				targetBitmap.FillQuad(p1x, p1y, p2x, p2y, p4x, p4y, p3x, p3y, color);
				//targetBitmap.FillPolygon(new int[] { p1x, p1y, p2x, p2y, p4x, p4y, p3x, p3y }, color);
			}
		}

		public void Update()
		{
			Height -= (Height / rectangleLife);
		}
	}
}
