﻿#pragma checksum "..\..\..\Pages\PackageInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0119B02BFF85E76BC15A06AEBC0D8E8F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Clickonce_Deployment_Manager_32.Pages {
    
    
    /// <summary>
    /// PackageInfo
    /// </summary>
    public partial class PackageInfo : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 54 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtApplicationName;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPublisher;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtVersion;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSubVersion;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBaseURL;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ChbAddCustomFiles;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\Pages\PackageInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ChbDesktopShort;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Clickonce Deployment Manager;component/pages/packageinfo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\PackageInfo.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\Pages\PackageInfo.xaml"
            ((Clickonce_Deployment_Manager_32.Pages.PackageInfo)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtApplicationName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtPublisher = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtVersion = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtSubVersion = ((System.Windows.Controls.TextBox)(target));
            
            #line 60 "..\..\..\Pages\PackageInfo.xaml"
            this.txtSubVersion.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtSubVersion_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtBaseURL = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.ChbAddCustomFiles = ((System.Windows.Controls.CheckBox)(target));
            
            #line 73 "..\..\..\Pages\PackageInfo.xaml"
            this.ChbAddCustomFiles.Checked += new System.Windows.RoutedEventHandler(this.ChbAddCustomFiles_Checked);
            
            #line default
            #line hidden
            
            #line 73 "..\..\..\Pages\PackageInfo.xaml"
            this.ChbAddCustomFiles.Unchecked += new System.Windows.RoutedEventHandler(this.ChbAddCustomFiles_Unchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ChbDesktopShort = ((System.Windows.Controls.CheckBox)(target));
            
            #line 74 "..\..\..\Pages\PackageInfo.xaml"
            this.ChbDesktopShort.Checked += new System.Windows.RoutedEventHandler(this.ChbDesktopShort_Checked);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\Pages\PackageInfo.xaml"
            this.ChbDesktopShort.Unchecked += new System.Windows.RoutedEventHandler(this.ChbDesktopShort_Unchecked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

