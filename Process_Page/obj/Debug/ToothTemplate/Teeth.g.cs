﻿#pragma checksum "..\..\..\ToothTemplate\Teeth.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EFD36E92D90AC480629520CB6F2880297D8D9D8B7ED85423D1A31F6A482E9339"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using Process_Page.ToothTemplate;
using Process_Page.Util;
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


namespace Process_Page.ToothTemplate {
    
    
    /// <summary>
    /// Teeth
    /// </summary>
    public partial class Teeth : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\ToothTemplate\Teeth.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Process_Page.ToothTemplate.Teeth TeethControl;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\ToothTemplate\Teeth.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas Canvas_Teeth;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\ToothTemplate\Teeth.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Process_Page.ToothTemplate.RotateTeeth rotateTeeth;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\ToothTemplate\Teeth.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Process_Page.ToothTemplate.DrawTeeth drawTeeth;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\ToothTemplate\Teeth.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Process_Page.ToothTemplate.WrapTeeth wrapTeeth;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\ToothTemplate\Teeth.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox list;
        
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
            System.Uri resourceLocater = new System.Uri("/Process_Page;component/toothtemplate/teeth.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ToothTemplate\Teeth.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.TeethControl = ((Process_Page.ToothTemplate.Teeth)(target));
            return;
            case 2:
            this.Canvas_Teeth = ((System.Windows.Controls.Canvas)(target));
            return;
            case 3:
            this.rotateTeeth = ((Process_Page.ToothTemplate.RotateTeeth)(target));
            return;
            case 4:
            this.drawTeeth = ((Process_Page.ToothTemplate.DrawTeeth)(target));
            return;
            case 5:
            this.wrapTeeth = ((Process_Page.ToothTemplate.WrapTeeth)(target));
            return;
            case 6:
            this.list = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

