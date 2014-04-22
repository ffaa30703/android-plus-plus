﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnvDTE;
using Microsoft.Build.Framework.XamlTypes;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VCProjectEngine;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Debuggers;
using Microsoft.VisualStudio.Project.Utilities;
using Microsoft.VisualStudio.Project.Utilities.DebuggerProviders;
using Microsoft.VisualStudio.Project.VS.Debuggers;

using AndroidPlusPlus.Common;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.VsDebugLauncher
{

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  [ExportDebugger("AndroidPlusPlusDebugger")]

  [PartMetadata(ProjectCapabilities.Requires, ProjectCapabilities.VisualC)]

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class DebugLauncherXI : DebugLaunchProviderBase 
  {
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Import]
    private Rules.RuleProperties DebuggerProperties { get; set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override bool CanLaunch (DebugLaunchOptions launchOptions)
    {
      LoggingUtils.PrintFunction ();

      return DebugLauncher.CanLaunch ((int) launchOptions);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override IDebugLaunchSettings [] QueryDebugTargets (DebugLaunchOptions launchOptions)
    {
      LoggingUtils.PrintFunction ();

      DebugLaunchSettings debugLaunchSettings = null;

      try
      {
        Dictionary<string, string> projectProperties = DebuggerProperties.ProjectPropertiesToDictionary ();

        if (launchOptions.HasFlag (DebugLaunchOptions.NoDebug))
        {
          debugLaunchSettings = (DebugLaunchSettings) DebugLauncher.StartWithoutDebugging ((int) launchOptions, projectProperties);
        }
        else
        {
          debugLaunchSettings = (DebugLaunchSettings) DebugLauncher.StartWithDebugging ((int) launchOptions, projectProperties);
        }
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        DebugLauncher.ShowErrorDialog ((IServiceProvider) ServiceProvider, string.Format ("Debugging failed to launch, encountered exception: {0}", e.Message));
      }

      return new IDebugLaunchSettings [] { debugLaunchSettings };
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////