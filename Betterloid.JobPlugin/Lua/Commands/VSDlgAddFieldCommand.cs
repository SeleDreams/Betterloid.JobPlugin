using System;
using Eluant;
using JobPlugin.Lua.Types;

namespace JobPlugin.Lua.Commands
{
    public class VSDlgAddFieldCommand
    {
        private static void AddIntegerField(VSFlexDialogField field)
        {
            int initialValue = int.Parse(field.InitialVal);
            JobPlugin.Instance.VSDialog.AddIntField(field.Caption, field.Name, initialValue);
        }

        private static void AddFloatField(VSFlexDialogField field)
        {
            float initialValue = float.Parse(field.InitialVal);
            JobPlugin.Instance.VSDialog.AddFloatField(field.Caption, field.Name, initialValue);
        }

        private static void AddStringField(VSFlexDialogField field)
        {
            JobPlugin.Instance.VSDialog.AddTextField(field.Caption, field.Name, field.InitialVal);
        }

        private static void AddBoolField(VSFlexDialogField field)
        {
            int parsedVal = int.Parse(field.InitialVal);
            bool initialValue = parsedVal == 1;
            JobPlugin.Instance.VSDialog.AddBooleanField(field.Caption, field.Name, initialValue);
        }

        private static void AddEnumField(VSFlexDialogField field)
        {
            string[] elements = field.InitialVal.Split(',');
            JobPlugin.Instance.VSDialog.AddEnumField(field.Caption, field.Name, elements);
        }

        public static int VSDlgAddField(LuaTable luaField)
        {
            VSFlexDialogField field = new VSFlexDialogField(luaField);
            switch (field.Type)
            {
                case VSFlexDialogFieldType.FT_INTEGER:
                    AddIntegerField(field);
                    return 1;
                case VSFlexDialogFieldType.FT_STRING:
                    AddStringField(field);
                    return 1;
                case VSFlexDialogFieldType.FT_STRING_LIST:
                    AddEnumField(field);
                    return 1;
                case VSFlexDialogFieldType.FT_FLOAT:
                    AddFloatField(field);
                    return 1;
                case VSFlexDialogFieldType.FT_BOOL:
                    AddBoolField(field);
                    return 1;
                default:
                    return 0;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSDlgAddField)))
            {
                lua.Globals["VSDlgAddField"] = fn;
            }
        }
    }
}
