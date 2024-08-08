using Betterloid;
using System.IO;
using JobPlugin.Lua;
using Eluant;
using VSDialog;
using System.Diagnostics;
using System.Collections.Generic;
using JobPlugin.Lua.Types;


#if VOCALOID6
using Yamaha.VOCALOID;
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
    public class JobPlugin : IPlugin
    {
        public static JobPlugin Instance;
        public LuaLoader Lua { get; private set; }
        public JobManifest Manifest { get; set; }
        public VSDialog.VSDialog VSDialog { get; set; }
        public MusicalEditorViewModel MusicalEditor { get; private set; }
        public ulong CurrentNoteId { get; set; }
        public Dictionary<VSControlType, ulong> CurrentControlId { get; set; } = new Dictionary<VSControlType, ulong>();
        public bool Modified { get; set; }


        private void Error()
        {
            if (Modified)
            {
#if VOCALOID5
                MusicalEditor.Sequence.Undo();
#elif VOCALOID6
                MusicalEditor.Sequence.VSMSequence.Undo();
#endif
            }
            MessageBoxDeliverer.GeneralError("The job plugin terminated with an error!");
        }

        public void Startup()
        {
            Instance = this;
            VSDialog = new VSDialog.VSDialog();
            MainWindow window = App.Current.MainWindow as MainWindow;
            VSDialog.Owner = window;
            var xMusicalEditorDiv = window.FindName("xMusicalEditorDiv") as MusicalEditorDivision;
            MusicalEditor = xMusicalEditorDiv.DataContext as MusicalEditorViewModel;
            try
            {
                using (Lua = new LuaLoader())
                {
                    Lua.RunScript();
                    Manifest = Lua.GetManifest();
                    Lua.RegisterCommands();
                    ProcessParam processParam = new ProcessParam();
                    EnvParam envParam = new EnvParam { ApiVersion = "3.0.1.0", ScriptDir = Lua.LUAFolder, ScriptName = Lua.LUAFilename, TempDir = Path.GetTempPath().Replace("\\","/")};
                    int result = 0;
                    var part = MusicalEditor.ActivePart ?? throw new NoActivePartException();
                    using (Transaction transaction = new Transaction(part.Sequence)) // This is important, without using a transaction your modifications won't be applied immediately
                    {
                        result = (int)Lua.Main(processParam, envParam);
                    }
                    if (result > 0)
                    {
                        Error();
                    }

                }
            }
            catch (LuaException ex)
            {
                Error();
                Debug.WriteLine($"{ex}");
            }
            catch (NoActivePartException)
            {
                MessageBoxDeliverer.GeneralError("There is no active part ! The job plugin cannot run.");
            }
        }
    }
}

/** TODO : Implement the following functions :
 * 
 * 
 * 
 * 
 * 
 * 
 **/