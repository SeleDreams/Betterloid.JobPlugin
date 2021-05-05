using Eluant.ObjectBinding;
using System;
using System.Linq;

#if VOCALOID6
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.Design.UI;
using Yamaha.VOCALOID.MusicalEditor;
using Yamaha.VOCALOID.VSM;
using VOCALOID = Yamaha.VOCALOID;
#elif VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.VOCALOID5.MusicalEditor;
using Yamaha.VOCALOID.VSM;
using VOCALOID = Yamaha.VOCALOID.VOCALOID5;
#endif


namespace JobPlugin
{
    public class NoActivePartException : Exception
    {

    }
    public class ProcessParam
    {
        public ProcessParam()
        {
            MainWindow window = App.Current.MainWindow as MainWindow;
            var xMusicalEditorDiv = window.FindName("xMusicalEditorDiv") as MusicalEditorDivision;
            var musicalEditor = xMusicalEditorDiv.DataContext as MusicalEditorViewModel;
            var activePart = musicalEditor.ActivePart;
            if (activePart == null)
            {
                throw new NoActivePartException();
            }
            WIVSMNote note;
#if VOCALOID6
           note = activePart.FirstSelectedNote;
#elif VOCALOID5
            note = activePart.Notes.FirstOrDefault(n => n.IsSelected);
#endif
            if (note != null)
            {
                BeginPosTick = note.RelPosition.Tick;
            }
#if VOCALOID6
            note = activePart.LastSelectedNote;
#elif VOCALOID5
            note = activePart.Notes.LastOrDefault(n => n.IsSelected);
            
#endif
            if (note != null)
            {
                EndPosTick = note.RelEnd.Tick;
            }
            else
            {
                EndPosTick = activePart.Duration.Tick;
            }

            var pos = musicalEditor.SongPosition.Tick;
            if (pos < activePart.AbsPosition.Tick)
            {
                pos = 0;
            }
            else
            {
                pos = musicalEditor.SongPosition.Tick - activePart.AbsPosition.Tick;
            }
            SongPosTick = pos;
        }

        [LuaMember("beginPosTick")]
        public long BeginPosTick { get; set; }

        [LuaMember("endPosTick")]
        public long EndPosTick { get; set; }

        [LuaMember("songPosTick")]
        public long SongPosTick { get; set; }
    }
}
