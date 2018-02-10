using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass.Misc;

namespace WispContract
{
    public interface WispManagerInterface
    {
        ObservableCollection<MenuItem> PluginMenus { get; set; }
        int StreamHandle { get; set; }
        BPMCounter CurrentBPM { get; set; }
		bool BPM_Avaliable { get; set; }
		void ChangeCurrentMusicPlayState();
		void ReloadMusic();
		void ResetPlayer();
	}
}
