﻿

#pragma checksum "C:\Users\m0rg0_000\Documents\GitHub\MoscowMuseums\M0rg0tRss\GroupedItemsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "07A650CBC57067EFDC2278C8FB28C76B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace M0rg0tRss
{
    partial class GroupedItemsPage : global::M0rg0tRss.Common.LayoutAwarePage
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::M0rg0tRss.Common.LayoutAwarePage pageRoot; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Data.CollectionViewSource groupedItemsViewSource; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button MapButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button RefreshButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressBar Loading; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView itemListView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button backButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock pageTitle; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock OfflineMode; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::M0rg0tRss.VariableSizeGridView itemGridView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.GridView zommedOutView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualStateGroup ApplicationViewStates; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState FullScreenLandscape; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState Filled; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState FullScreenPortrait; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState Snapped; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///GroupedItemsPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            pageRoot = (global::M0rg0tRss.Common.LayoutAwarePage)this.FindName("pageRoot");
            groupedItemsViewSource = (global::Windows.UI.Xaml.Data.CollectionViewSource)this.FindName("groupedItemsViewSource");
            MapButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("MapButton");
            RefreshButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("RefreshButton");
            Loading = (global::Windows.UI.Xaml.Controls.ProgressBar)this.FindName("Loading");
            itemListView = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("itemListView");
            backButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("backButton");
            pageTitle = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("pageTitle");
            OfflineMode = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("OfflineMode");
            itemGridView = (global::M0rg0tRss.VariableSizeGridView)this.FindName("itemGridView");
            zommedOutView = (global::Windows.UI.Xaml.Controls.GridView)this.FindName("zommedOutView");
            ApplicationViewStates = (global::Windows.UI.Xaml.VisualStateGroup)this.FindName("ApplicationViewStates");
            FullScreenLandscape = (global::Windows.UI.Xaml.VisualState)this.FindName("FullScreenLandscape");
            Filled = (global::Windows.UI.Xaml.VisualState)this.FindName("Filled");
            FullScreenPortrait = (global::Windows.UI.Xaml.VisualState)this.FindName("FullScreenPortrait");
            Snapped = (global::Windows.UI.Xaml.VisualState)this.FindName("Snapped");
        }
    }
}



