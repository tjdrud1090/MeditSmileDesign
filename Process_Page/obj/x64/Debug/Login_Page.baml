   M S B A M L   `   `   `  ����G  CProcess_Page, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Process_Page.TranslationDemoProcess_Page.TranslationDemo  5          Process_Page.Login_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      6local*clr-namespace:Process_Page.TranslationDemo  5        N� Title$  
Login_Page��5
       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$
 Copy��5      6<   .��5      �  � RowDefinitions 5   
   �� 5      $
��187* �6   �� 5      $
��213* �6   ��5   
   �� 5       �� 
StartPoint$ 0.5,0&�6"    �� EndPoint$ 0.5,1&�65   .��5      �  � Color$ 	#FFBED2FD��6    � Offset$	 0.2Q�61   � 5      $ White��6   $	 0.8Q�6-   5      .+�5   
   ]� $��
0,-10,0,10q�6   $��2��6(   .+�5      �� -   5         ��Name$ image��5      $	��439��6$   $	��Top=�61   $
��1188��6I   $-��'pack://application:,,,/Image/logo_1.png��6V   $��0,64,0,0q�6�   5      '  !MaterialDesignThemes.Wpf.Snackbar  5   
   -   5        	  Name$	 MainSnackbar��5   O   $��1��5   -   $��456,0,442,0q�6:   $	��302��6O   
   MessageQueue
 5   "   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  5   O   ]� 5#   
   .+�5$      �� -   5         ��Name$ IDtext��5$      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ ID��6&   $
��Bold"�6J   $
��Auto��6^   $	��448��6l   $��0,545,0,-164q�6x   5%      ]� 5&   
   .+�5'      G� -   5         G�Name$ PasswordBox��5'      $ Password��6/   $
��Bold"�6Y   $��0,640,0,-236q�6k   $
��Auto��6�   $	��448��6�   5(      ]� 5)   
   .+�5*      ��  #  MaterialDesignRaisedLightButton#��[  6    � ToolTip$' !ID와 Password를 입력하세요��6N   $��35��5+      $��Bottom=�6   ���	#FFD2E9FB���	#FF99CEF9$
��Bold"�5,      $��Microsoft YaHei UI)�6#    J� RenderTransformOrigin$ 0.5,0.5&�6D   $��
0,0,0,-286q�6d   ���	#FF113466$��LOGIN��5-   I   $	��446��6Y   ��6   �� *Loginclicked+6Y   5/      ]� 50   
   .+�52      �� -   5           MaterialDesignOutlinedButton#��[ 52      $��회원가입��6K   $ 0.5,0.5&�53   N   $��0,774,0,-378q�6�   $	��432��6�   $
��Auto��6�   ��6   �� *RunDialogCommand+��67   Z� 6�   54      )  #MaterialDesignThemes.Wpf.DialogHost  56   
      
Identifier$ 
RootDialog��6$   $��2��6�      SnackbarMessageQueue 6<   ��  �� ElementName$ MainSnackbar�� �� Path$ MessageQueue�57      59      5:      