using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ListMenuOption
    {
        public string MenuTabName { get; set; }
        public string OptionURL { get; set; }
        public string OptionName { get; set; }
        public int OptionOrder { get; set; }
        public bool add { get; set; }
    }
}
