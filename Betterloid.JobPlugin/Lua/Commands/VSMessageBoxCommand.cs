using System;
using Betterloid.Wrappers;
using Eluant;
using MsgBoxEx;

namespace JobPlugin.Lua.Commands
{
    public class VSMessageBoxCommand
    {
        public enum VSMessageBoxType {
            MB_OK = 0,
            MB_OKCANCEL = 1,
            MB_ABORTRETRYIGNORE = 2,
            MB_YESNOCANCEL = 3,
            MB_YESNO = 4,
            MB_RETRYCANCEL = 5
        }
        public enum VSMessageBoxResult
        {
            ID_OK = 1,
            ID_CANCEL = 2,
            ID_ABORT = 3,
            ID_RETRY = 4,
            ID_IGNORE = 5,
            ID_YES = 6,
            ID_NO = 7
        }

        static MessageBoxResultEx ShowEx(string message,VSMessageBoxType type)
        {
            var functionality = new MsgBoxExtendedFunctionality();
            string title = "Job Plugin";
            switch (type)
            {
                case VSMessageBoxType.MB_OK:
                    return MessageBoxEx.ShowEx(message, new object[] { title, App.MainWindow.Object, MessageBoxButtonEx.OK, null });
                case VSMessageBoxType.MB_OKCANCEL:
                    return MessageBoxEx.ShowEx(message, new object[] {title, App.MainWindow.Object, MessageBoxButtonEx.OKCancel, null });
                case VSMessageBoxType.MB_ABORTRETRYIGNORE:
                    return MessageBoxEx.ShowEx(message, new object[] { title, App.MainWindow.Object, MessageBoxButtonEx.AbortRetryIgnore, null });
                case VSMessageBoxType.MB_YESNOCANCEL:
                    return MessageBoxEx.ShowEx(message,new object[] { title, App.MainWindow.Object, MessageBoxButtonEx.YesNoCancel, null });
                case VSMessageBoxType.MB_YESNO:
                    return MessageBoxEx.ShowEx(message,new object[] { title, App.MainWindow.Object, MessageBoxButtonEx.YesNo, null });
                case VSMessageBoxType.MB_RETRYCANCEL:
                    return MessageBoxEx.ShowEx(message,new object[] { title, App.MainWindow.Object, MessageBoxButtonEx.RetryCancel, null });
            }
            return MessageBoxResultEx.OK;
        }

        static int GetResult(MessageBoxResultEx result)
        {
            switch (result)
            {
                case MessageBoxResultEx.OK:
                    return (int)VSMessageBoxResult.ID_OK;
                case MessageBoxResultEx.Cancel:
                    return (int)VSMessageBoxResult.ID_CANCEL;
                case MessageBoxResultEx.Abort:
                    return (int)VSMessageBoxResult.ID_ABORT;
                case MessageBoxResultEx.Retry:
                    return (int)VSMessageBoxResult.ID_RETRY;
                case MessageBoxResultEx.Ignore:
                    return (int)VSMessageBoxResult.ID_IGNORE;
                case MessageBoxResultEx.Yes:
                    return (int)VSMessageBoxResult.ID_YES;
                case MessageBoxResultEx.No:
                    return (int)VSMessageBoxResult.ID_NO;
            }
            return (int)VSMessageBoxResult.ID_OK;
        }

        public static int VSMessageBox(string message, int type)
        {
            if (!Enum.IsDefined(typeof(VSMessageBoxType),type))
            {
                MessageBoxDeliverer.GeneralError("The type defined for the message box type is invalid");
                return (int)VSMessageBoxResult.ID_ABORT;
            }
            VSMessageBoxType vsType = (VSMessageBoxType)type;
            MessageBoxResultEx result = ShowEx(message, vsType);
            return GetResult(result);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string, int, int>(VSMessageBox)))
            {
                lua.Globals["VSMessageBox"] = fn;
            }
        }
    }
}
