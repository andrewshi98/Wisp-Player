using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using WispContract;
using System.Timers;
using System.ComponentModel;

namespace FlexMusicControl
{
    /// <summary>
    /// Interaction logic for FlexMusicControl.xaml
    /// </summary>
    [Export(typeof(WispPlug))]
    public partial class FlexMusicControl : UserControl, WispPlug, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private WriteableBitmap spectrumWritableBitmap;

		Spectrum spectrum;
		Timer dispatcherTimer = new Timer(20);
		Storyboard LogoShrinkAnimation;
        WispManagerInterface wispManager;
        MenuItem FlexMusicMenuItem;
        [ImportingConstructor]
        public FlexMusicControl([Import("wispManager")] WispManagerInterface wispManager)
        {
			this.wispManager = wispManager;
            InitializeComponent();
			LogoShrinkAnimation = Logo.FindResource("Logo_ShrinkBPM") as Storyboard;
			Storyboard.SetTarget(Logo, LogoShrinkAnimation);
		}

        public void Init()
        {
			spectrumWritableBitmap = BitmapFactory.New((int)SpectrumImageBitmap.Width, (int)(SpectrumImageBitmap.Height));
			SpectrumImageBitmap.Source = spectrumWritableBitmap;
			spectrum = new Spectrum(spectrumWritableBitmap, Logo.Width/2*0.95);
			FlexMusicMenuItem = new MenuItem();
			FlexMusicMenuItem.Header = "FlexMusicControl";
            FlexMusicMenuItem.Click += FlexMusicMenuClick;
            wispManager.PluginMenus.Add(FlexMusicMenuItem);
			dispatcherTimer.Elapsed += Shrink_Tick;
			dispatcherTimer.Start();
			LogoShrinkAnimation.Begin();
		}

		private int SpectrumActivationCount = 0;
		private void Shrink_Tick(object sender, EventArgs e)
		{
			if (Bass.BASS_ChannelIsActive(wispManager.StreamHandle) == BASSActive.BASS_ACTIVE_PLAYING)
			{
				SpectrumActivationCount++;
				if (wispManager.BPM_Avaliable && wispManager.CurrentBPM.BPM > 0)
					Application.Current.Dispatcher.Invoke(() =>
					{
						LogoShrinkAnimation.Begin();
					}, DispatcherPriority.DataBind);
				if (SpectrumActivationCount >= 4)
				{
					SpectrumActivationCount = 0;
					float[] buffer = new float[2048];
					Bass.BASS_ChannelGetData(wispManager.StreamHandle, buffer, (int)BASSData.BASS_DATA_FFT2048);
					ShowSpectrum(100, 2000, 250, buffer);
				}
			}
			Application.Current.Dispatcher.Invoke(() =>
			{
				using (spectrumWritableBitmap.GetBitmapContext())
				{
					spectrumWritableBitmap.Clear(Color.FromArgb(0, 0, 0, 0));
				}
				spectrum.Update();
				spectrum.Draw();
			}, DispatcherPriority.Render);
		}

		public string Version()
        {
            return "FlexMusicControl";
        }

        private void FlexMusicMenuClick(object sender, RoutedEventArgs e)
		{
        }

        private void Logo_Click(object sender, RoutedEventArgs e)
		{
			wispManager.ChangeCurrentMusicPlayState();
        }

		private void ShowSpectrum(int startFreq, int endFreq, int barCount, float[] FFTData)
		{
			float stepAmount = (float)(endFreq - startFreq) / (float)(barCount);
			float[] spectrumList = new float[barCount + 1];
			for (float currentFreq = startFreq, i = 0; currentFreq <= endFreq; currentFreq += stepAmount, i++)
			{
				spectrumList[(int)i] = FFTData[Utils.FFTFrequency2Index((int)currentFreq, 2048, 44100)];
			}
			Application.Current.Dispatcher.Invoke(() =>
			{
				spectrum.addLayer(spectrumList, Color.FromArgb(200, 200, 200, 200), 40, 30);
			});
		}
	}
}