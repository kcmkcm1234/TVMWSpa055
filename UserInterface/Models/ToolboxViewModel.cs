using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class ToolboxViewModel
    {
        public ToolBoxStructure backbtn;
        public ToolBoxStructure addbtn;
        public ToolBoxStructure savebtn;
        public ToolBoxStructure deletebtn;
        public ToolBoxStructure resetbtn;
        public ToolBoxStructure returnBtn;
        public ToolBoxStructure calculateBtn;
        public ToolBoxStructure CloseBtn;
        public ToolBoxStructure PrintBtn;
        public ToolBoxStructure DepositBtn;
        public ToolBoxStructure WithdrawBtn;
        public ToolBoxStructure ClearBtn;
    }

    public struct ToolBoxStructure
    {
        public string Event { get; set; }
        public string Title { get; set; }//tooltip
        public string Text { get; set; }
        public string DisableReason { get; set; }
        public bool Visible { get; set; }
        public bool Disable { get; set; }
    }

    public class ToolBox
    {
        public string Dom { get; set; }
        public string Action { get; set; }
        public string ViewModel { get; set; }
    }
}