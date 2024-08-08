using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin.Lua
{
    public enum VSFlexDialogFieldType
    {
        FT_INTEGER = 0, // Integer type.
        FT_BOOL, // Boolean type.
        FT_FLOAT, // Real type.
        FT_STRING, // String type.
        FT_STRING_LIST // String type list (combo box)
    }
}
