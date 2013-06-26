﻿#pragma checksum "..\..\Window1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2E81E4FA3C6ECD371AB75FF9B2FA5982"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
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


namespace MT_Creator_WPF {
    
    
    /// <summary>
    /// Window1
    /// </summary>
    public partial class Window1 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\Window1.xaml"
        internal System.Windows.Controls.MenuItem Play_Button;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Window1.xaml"
        internal System.Windows.Controls.MenuItem Pause;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Window1.xaml"
        internal System.Windows.Controls.MenuItem Stop;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\Window1.xaml"
        internal System.Windows.Controls.MenuItem Sound;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\Window1.xaml"
        internal System.Windows.Controls.Canvas scene;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MT_Creator_WPF;component/window1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Window1.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.BG_Color);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 13 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_Load);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 14 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Click_Close);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 16 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_Text);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 18 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_Image);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 21 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_Vid);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 24 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_Sound);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 27 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.BG_Color);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 29 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Remove);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 30 "..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Export);
            
            #line default
            #line hidden
            return;
            case 11:
            this.Play_Button = ((System.Windows.Controls.MenuItem)(target));
            
            #line 33 "..\..\Window1.xaml"
            this.Play_Button.Click += new System.Windows.RoutedEventHandler(this.Click_Play);
            
            #line default
            #line hidden
            return;
            case 12:
            this.Pause = ((System.Windows.Controls.MenuItem)(target));
            
            #line 38 "..\..\Window1.xaml"
            this.Pause.Click += new System.Windows.RoutedEventHandler(this.Pause_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.Stop = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 14:
            this.Sound = ((System.Windows.Controls.MenuItem)(target));
            
            #line 48 "..\..\Window1.xaml"
            this.Sound.Click += new System.Windows.RoutedEventHandler(this.SelectSound);
            
            #line default
            #line hidden
            return;
            case 15:
            this.scene = ((System.Windows.Controls.Canvas)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
