   M S B A M L   `   `   `  ����G  CProcess_Page, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Process_Page.TranslationDemoProcess_Page.TranslationDemo  5      *    $Process_Page.TranslationDemo.Slider1   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:Process_Page.TranslationDemo     5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      $	��314��5      $	��278��6+   .��5	      �   � RowDefinitions  5
   
   �� 5      $	��98* �6   �� 5      $	��41* �6   .+�5   
   �� -   5            MaterialDesignOutlinedButton#��[  5      $��회원가입��6G   $
��Auto��5   N   $��82,191,82,36q�6\   $��2��5      ��5      �� *RunDialogCommand+��67   Z� 5   "   �� 5   
   -   5         ��Name$ IDtext��5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ ID��6"   $
��Bold"�6F   $��
0,51,0,108q�6X   $
��Auto��6n   G� 5   
   -   5         G�Name$ PasswordBox��5      $ Password��6+   $
��Bold"�6U   $��	0,93,0,67q�6g   $
��Auto��6{   �� 5   
    # MaterialDesignRaisedLightButton#��[ 6    � ToolTip$' !ID와 Password를 입력하세요��6J   $��27��5      $��Bottom=�6   ���	#FFD2E9FB���	#FF99CEF9$
��Bold"�5      $��Microsoft YaHei UI)�6#    J� RenderTransformOrigin$ 0.5,0.5&�6D   $��0,0,0,17q�6d   ���	#FF113466$��LOGIN��5   I   ��6   �� *Loginclicked+6I   5      5      