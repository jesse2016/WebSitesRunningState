using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace GT.SiteCheck.Quartz
{
    [RunInstaller(true)]
    public partial class QuartzInstaller : System.Configuration.Install.Installer
    {
        public QuartzInstaller()
        {
            InitializeComponent();
        }
    }
}
