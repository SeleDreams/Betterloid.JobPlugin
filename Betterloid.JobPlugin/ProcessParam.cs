using Eluant.ObjectBinding;
using System;
#if VOCALOID6
using Yamaha.VOCALOID.VSM;
#elif VOCALOID5
using Yamaha.VOCALOID.VSM;
using System.Linq;
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
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
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
#if VOCALOID5
                BeginPosTick = note.RelPosition.Tick;
#elif VOCALOID6
                BeginPosTick = note.RelPosTick.Value;
#endif
            }
#if VOCALOID6
            note = activePart.LastSelectedNote;
#elif VOCALOID5
            note = activePart.Notes.LastOrDefault(n => n.IsSelected);
            
#endif
            if (note != null)
            {
#if VOCALOID5
                EndPosTick = note.RelEnd.Tick;
#elif VOCALOID6
                BeginPosTick = note.RelEndTick.Value;
#endif
            }
            else
            {
#if VOCALOID5
                EndPosTick = activePart.Duration.Tick;
#elif VOCALOID6
                EndPosTick = activePart.DurationTick.Value;
#endif
            }


#if VOCALOID5
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
        
#elif VOCALOID6
            var pos = musicalEditor.SongPosition.Value;
            if (pos < activePart.AbsPosTick.Value)
            {
                pos = 0;
            }
            else
            {
                pos = musicalEditor.SongPosition.Value - activePart.AbsPosTick.Value;
            }
            SongPosTick = pos;
#endif
        }
        [LuaMember("beginPosTick")]
        public long BeginPosTick { get; set; }

        [LuaMember("endPosTick")]
        public long EndPosTick { get; set; }

        [LuaMember("songPosTick")]
        public long SongPosTick { get; set; }
    }
}
