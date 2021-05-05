#if VOCALOID5
using Yamaha.VOCALOID.VDM;
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.VOCALOID5.MusicalEditor;
using Yamaha.VOCALOID.VOCALOID5.TrackEditor;
using Yamaha.VOCALOID.VSM;
using Yamaha.VOCALOID.VSStyle;
#elif VOCALOID6
using Yamaha.VOCALOID.VDM;
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.MusicalEditor;
using Yamaha.VOCALOID.TrackEditor;
using Yamaha.VOCALOID.VSM;
using Yamaha.VOCALOID.VSStyle;
#endif

namespace JobPlugin
{
    public class JobPluginUIViewModel
    {
        MainWindow MainWindow;
        TrackEditorDivision TrackEditorDivision;
        TrackEditorViewModel TrackEditorViewModel;
        MusicalEditorDivision MusicalEditorDivision;
        MusicalEditorViewModel MusicalEditor;
        VoiceBank DefaultVoicebank;

        public JobPluginUIViewModel()
        {
            // If you need to find the name of a specific property you can rely on DNSpy or the visual studio WPF debugger to find it
            MainWindow = App.Current.MainWindow as MainWindow;
            MusicalEditorDivision = MainWindow.FindName("xMusicalEditorDiv") as MusicalEditorDivision;
            MusicalEditor = MusicalEditorDivision.DataContext as MusicalEditorViewModel;
            TrackEditorDivision = MainWindow.FindName("xTrackEditorDiv") as TrackEditorDivision;
            TrackEditorViewModel = TrackEditorDivision.DataContext as TrackEditorViewModel;
            DefaultVoicebank = App.DatabaseManager.DefaultVoiceBank;
        }
    }
}
