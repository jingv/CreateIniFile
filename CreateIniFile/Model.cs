using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.RightsManagement;
using System.Text;

namespace CreateIniFile
{
    class TreeData
    {
        public TreeData() 
        {
            ChildNodes = new List<TreeData>();
        }
        
        public int NodeId { get; set; }
        public int ParentId { get; set; }
        public string NodeValue { get; set; }
        public List<TreeData> ChildNodes { get; set; }
    }
}
