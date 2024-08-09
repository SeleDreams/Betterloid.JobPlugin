using Betterloid;
using System.IO;
using JobPlugin.Lua;
using Eluant;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using JobPlugin.Lua.Types;
using System.Globalization;
using Microsoft.Win32;
using System.Windows.Forms;
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
        // The current note, will be returned in the next VSGetNextNote call
        public WIVSMNote CurrentNote { get; set; }
        // The current control, will be returned in the next VSGetNextControl call
        public Dictionary<VSControlType, WIVSMMidiController> CurrentControl { get; set; } = new Dictionary<VSControlType, WIVSMMidiController>();
        // For some reason, the vocaloid api does not give direct access to the controllers list compared to notes. As such it is harder to keep track of their IDs, that's why we cache references to them in this list when VSSeekToBeginControlCommand is called
        public Dictionary<VSControlType, List<WIVSMMidiController>> Controllers { get; set; } = new Dictionary<VSControlType, List<WIVSMMidiController>>();

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
            string previous = Directory.GetCurrentDirectory();
            try
            {
                string luaPath = "";
                    using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
                    {
                        openFileDialog.Filter = "lua files (*.lua)|*.lua";
                        openFileDialog.FilterIndex = 2;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //Get the path of specified file
                            luaPath = openFileDialog.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                using (Lua = new LuaLoader(luaPath.Replace("\\","/")))
                {
                    Lua.RunScript();
                    Manifest = Lua.GetManifest();
                    Lua.RegisterCommands();
                    ProcessParam processParam = new ProcessParam();
                    EnvParam envParam = new EnvParam { ApiVersion = "3.0.1.0", ScriptDir = Lua.LUAFolder, ScriptName = Lua.LUAFilename, TempDir = Path.GetTempPath().Replace("\\","/")};
                    int result = 0;
                    var part = MusicalEditor.ActivePart ?? throw new NoActivePartException();
                    CurrentNote = part.GetNote(0);
                    Directory.SetCurrentDirectory(Lua.LUAFolder);
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
            catch (NoActivePartException)
            {
                MessageBoxDeliverer.GeneralError("There is no active part ! The job plugin cannot run.");
            }
            catch (Exception ex)
            {
                Error();
                Debug.WriteLine($"{ex}");
            }
            Directory.SetCurrentDirectory(previous);
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