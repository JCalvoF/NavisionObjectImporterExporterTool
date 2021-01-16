/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:NavisionObjectImporterExporterTool"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using NavisionObjectImporterExporterTool.Data;
using NavisionObjectImporterExporterTool.Services;
using NavisionObjectImporterExporterTool.Models;
using NavisionObjectImporterExporterTool.Enum;

// Toolkit namespace
using SimpleMvvmToolkit;

namespace NavisionObjectImporterExporterTool
{
    /// <summary>
    /// This class creates ViewModels on demand for Views, supplying a
    /// ServiceAgent to the ViewModel if required.
    /// <para>
    /// Place the ViewModelLocator in the App.xaml resources:
    /// </para>
    /// <code>
    /// &lt;Application.Resources&gt;
    ///     &lt;vm:ViewModelLocator xmlns:vm="clr-namespace:NavisionObjectImporterExporterTool"
    ///                                  x:Key="Locator" /&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// <para>
    /// Then use:
    /// </para>
    /// <code>
    /// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
    /// </code>
    /// <para>
    /// Use the <strong>mvvmlocator</strong> or <strong>mvvmlocatornosa</strong>
    /// code snippets to add ViewModels to this locator.
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        // Create MainPageViewModel on demand
        public MainPageViewModel MainPageViewModel
        {
            get { return GetMainPage(); }
        }


         

        private MainPageViewModel GetMainPage()
        {
            IConnectionsService connservice = new ConnectionsService();            
            ICommandService commandservice = new CommandService_NAV2013_R2();

            INavisionObjectsRepository objectsrepository = new NavisionObjectsRepository_2013_R2();
            INavObjectService objservice = new NavObjectService(objectsrepository);
            IScriptService scriptservice = new ScriptsService();

            return new MainPageViewModel(connservice, objservice, commandservice, scriptservice);
        }

    }
}