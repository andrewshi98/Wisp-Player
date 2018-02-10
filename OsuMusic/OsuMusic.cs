using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WispContract;

namespace OsuMusic
{
    [Export(typeof(WispPlug))]
    public class OsuMusicManager : WispPlug
    {
        public WispManagerInterface _wispManager;
        public MenuItem osuMusicMenuItem;
        [ImportingConstructor]
        public OsuMusicManager([Import("wispManager")] WispManagerInterface wispManager)
        {
            _wispManager = wispManager;
        }
		
        public void Init()
        {
            osuMusicMenuItem = new MenuItem();
            osuMusicMenuItem.Header = "OsuMusic Importer";
            osuMusicMenuItem.Click += Anything;
            _wispManager.PluginMenus.Add(osuMusicMenuItem);
        }

        public string Version()
        {
            return "OsuMusic Manager v0.1b";
        }

        private void Anything(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello There from OsuMusic");
        }
    }
}
