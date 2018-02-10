using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using WispContract;

namespace FlexMusicCollection
{
	/// <summary>
	/// Interaction logic for Collections.xaml
	/// </summary>
	[Export(typeof(WispPlug))]
	public partial class MusicCollections : UserControl, WispPlug
	{
		[ImportingConstructor]
		public MusicCollections([Import("wispManager")] WispManagerInterface wispManagerInterface)
		{
			InitializeComponent();
		}

		public void Init()
		{
			//No Init
		}

		public string Version()
		{
			return "FlexMusicCollection System";
		}
	}
}
