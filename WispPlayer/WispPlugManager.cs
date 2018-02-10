using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using WispContract;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Un4seen.Bass;
using System.Windows.Interop;
using Un4seen.Bass.Misc;
using System.Windows.Threading;
using System.ComponentModel;
using System.Timers;

namespace WispPlayer
{

    public class WispPlugProvider
    {
        [ImportMany]
        public IEnumerable<WispPlug> PlugList { get; private set; }

        public WispPlugProvider(WispManagerInterface wispManager)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog("."));
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeExportedValue("wispManager", wispManager);
            container.ComposeParts(this);
        }
    }

    public class WispPlugManager : WispManagerInterface, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

		Timer bpmTimer;
		public WispPlugProvider wispPlugProvider;
        public ObservableCollection<MenuItem> PluginMenus { get; set; }
        public int StreamHandle { get; set; }
        public BPMCounter CurrentBPM { get; set; }
		public bool BPM_Avaliable { get; set; }

        #region Test Field

        public String FileName { get; set; }

		#endregion

		public WispPlugManager(IntPtr windowHandle)
        {
			StreamHandle = -1;
            wispPlugProvider = new WispPlugProvider(this);
            PluginMenus = new ObservableCollection<MenuItem>();
            CurrentBPM = new BPMCounter(20, 44100);
			CurrentBPM.BPMHistorySize = 50;

            //bpm calculaion initialization
            // setting the BPM range (60-180 is a range most analog DJ Mixers do have)
			BPM_Avaliable = false;
			
            bpmTimer = new Timer(20);
            bpmTimer.Elapsed += async (sender, e) => await Task.Run(() => Bpm_tick(sender, e));

            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, windowHandle);

            foreach (WispPlug temp in wispPlugProvider.PlugList)
            {
                temp.Init();
            }
        }

        private void Bpm_tick(object sender, ElapsedEventArgs e)
        {
			BPM_Avaliable = CurrentBPM.ProcessAudio(StreamHandle, false);
        }

        public List<UserControl> GetUIList()
        {
            List<UserControl> userControlList = new List<UserControl>();
            foreach (WispPlug temp in wispPlugProvider.PlugList)
            {
                Debug.WriteLine(temp);
                if (temp is UserControl)
				{
                    userControlList.Add(temp as UserControl);
                }
            }
            return userControlList;
        }

		public void ResetPlayer()
		{
			if(StreamHandle != -1)
			{
				Bass.BASS_StreamFree(StreamHandle);
			}
			bpmTimer.Stop();
		}

		public void ChangeCurrentMusicPlayState()
		{
			if (Bass.BASS_ChannelIsActive(StreamHandle) != BASSActive.BASS_ACTIVE_STOPPED)
			{
				if (Bass.BASS_ChannelIsActive(StreamHandle) == BASSActive.BASS_ACTIVE_PLAYING)
				{
					Bass.BASS_ChannelPause(StreamHandle);
				} else
				{
					Bass.BASS_ChannelPlay(StreamHandle, false);
				}
			}
		}

        public void ReloadMusic()
		{
			if (Bass.BASS_ChannelIsActive(StreamHandle) != BASSActive.BASS_ACTIVE_STOPPED)
			{
				ResetPlayer();
			}
			if (FileName != null && System.IO.File.Exists(FileName))
			{
				StreamHandle = Bass.BASS_StreamCreateFile(FileName, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT);
				Bass.BASS_ChannelPlay(StreamHandle, false);
				BASS_CHANNELINFO bassInfo = new BASS_CHANNELINFO();
				Bass.BASS_ChannelGetInfo(StreamHandle, bassInfo);
				CurrentBPM.Reset(bassInfo.freq);
				bpmTimer.Start();
			}
        }

		public void OpenMusicFile(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
			OpenFileDialog dlg = new OpenFileDialog();

			dlg.Filter = "MP3 Files (*.mp3)|*.mp3|WAV Files (*.wav)|*.wav";

			// Get the selected file name and display in a TextBox 
			if (dlg.ShowDialog() == true)
			{
				// Open document 
				this.FileName = dlg.FileName;
			}

			ReloadMusic();
		}
    }
}
