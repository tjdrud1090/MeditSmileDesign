   M S B A M L   `   `   `  ����G  CProcess_Page, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Process_Page.CardsProcess_Page.Cards  5      5clr-namespace:Process_Page.UtilProcess_Page.Util  5      ]3clr-namespace:Process_Page.Cards.TranslationControl%Process_Page.Cards.TranslationControl  5	      G(clr-namespace:Process_Page.ToothTemplateProcess_Page.ToothTemplate  5
      E'clr-namespace:Process_Page_Change.CardsProcess_Page_Change.Cards  5      #    Process_Page.SmileDesign_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:Process_Page.Cards     5      ;MouseBehavioursclr-namespace:Process_Page.Util     5      U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?trans3clr-namespace:Process_Page.Cards.TranslationControl  5	      3View(clr-namespace:Process_Page.ToothTemplate  5
      4local1'clr-namespace:Process_Page_Change.Cards  5      -   5          N�Name$  FunctionPage��5       N� Title$ FaceAlign_Page��5      .��5      � ��5   
   �� 5       �� 
StartPoint$ 0.5,0&�6"    �� EndPoint$ 0.5,1&�65   .��5      �  � Color$ #D9E5FF��6    � Offset$	 0.8Q�6/   � 5      $ White��6   $	 0.2Q�6-   5       � ColumnDefinitions 5   
   �� 5      $	��15* �6   �� 5      $	��70* �6   �� 5      $	��15* �6    � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   �� 5       $��3* �6   6*   �� 5!      $��1* �6   6*   .+�5%   
   � -   5        $��
image_view��5%      $��1��6!   $��0��6.   $��3��6>   $��3��6R   $��20q�6c   &    Process_Page.Util.MouseBehaviour   MouseWheelCommand 5&      �� 	 �� ElementName$	 FunctionPage��
 �� Path$"
 DataContext.SizeChangedWheel�.+�5'      �� -   5         ��Name$ Image_Canvas��5'      ��6+   �� $	 
image_view��$
 ActualWidth���5(      �� $	 
image_view��$
 ActualHeight�!   MouseLeftButtonDownCommand 5)      �� $	 FunctionPage��$
 DataContext.LeftDown�   MouseMoveCommand 5*      �� $	 FunctionPage��$"
 DataContext.MouseMoveCommand�   MouseLeftButtonUpCommand 5+      �� $	 FunctionPage��$
 DataContext.LeftUp�"   MouseRightButtonDownCommand 5,      �� $	 FunctionPage��$
 DataContext.RightDown�    MouseRightButtonUpCommand 5-      �� $	 FunctionPage��$
 DataContext.RightUp�)   #Process_Page.Util.KeyBoardBehaviors   KeyboardDownCommand 5.      �� $	 FunctionPage��$
 DataContext.keyboardDown�.+�50      $   Process_Page.Cards.ImageCanvas  -   5          Name$ image_layer2��50   $      
FacePoints 51   $   �� $	 FunctionPage��$
 DataContext.GagPoints�   ImageSource 52   $   �� $	 FunctionPage��$
 DataContext.Source1�   RotateAngle 53   $   �� $	 FunctionPage��$
 DataContext.GagAngle�   ScaleXY 54   $   �� $	 FunctionPage��$
 DataContext.GagScale� �� Left 55   $   �� $	 FunctionPage��$
 DataContext.GagCenter.X�
 �� Top 56   $   �� $	 FunctionPage��$
 DataContext.GagCenter.Y�57        59      -   5        $ image_layer1��59   $   $ 1Q�5=   $    5:   $   �� $	 FunctionPage��$
 DataContext.FrontalPoints� 5;   $   �� $	 FunctionPage��$
 DataContext.Source� 5<   $   �� $	 FunctionPage��$
 DataContext.Angle� 5>   $   �� $	 FunctionPage��$
 DataContext.Center.X� 5?   $   �� $	 FunctionPage��$
 DataContext.Center.Y�5@      +   %Process_Page.ToothTemplate.LowerTooth  5B      -   5          Name$ 	UserLower��5B   "   $y�Hidden=�5C   !      Tooth_Points 5B   5   �� $	 ToothControl��$
 DataContext.LowerPoints� 5D   "   �� $	 FunctionPage��$$
 DataContext.ToothLowerCenter.X� 5E   "   �� $	 FunctionPage��$$
 DataContext.ToothLowerCenter.Y�   ShowLengths 5F   !   �� $	 ToothControl��$
 DataContext.ShowLength�   Fill 5G   !   �� $	 ToothControl��$
 DataContext.ColorTeeth�6j   +   %Process_Page.ToothTemplate.UpperTooth  5I      -   5          Name$ 	UserUpper��5I   "   $y�Hidden=�5J          Tooth_Points 5I   5   �� $	 ToothControl��$
 DataContext.UpperPoints� 5K   "   �� $	 FunctionPage��$$
 DataContext.ToothUpperCenter.X� 5L   "   �� $	 FunctionPage��$$
 DataContext.ToothUpperCenter.Y�   ShowLengths 5M   !   �� $	 ToothControl��$
 DataContext.ShowLength�    Fill  5N   !   �� $	 ToothControl��$
 DataContext.ColorTeeth�6j   z�5P      �� 5Q      ! �� CenterX! 6%   �� $	 FunctionPage��$$
 DataContext.FrontalPoints[0].X�" �� CenterY" 5R   %   �� $	 FunctionPage��$$
 DataContext.FrontalPoints[0].Y�# �� ScaleX# 5S      �� $	 FunctionPage��$
 DataContext.rectsc�$ �� ScaleY$ 5T      �� $	 FunctionPage��$
 DataContext.rectsc�6`   5V      �� 5X      -   5        $ Draw_Canvas��5X      $
��Left=�6*   $	��Top=�6E   ��5Y      �� $	 
image_view��$
 ActualWidth���5Z      �� $	 
image_view��$
 ActualHeight� 5[      �� $	 FunctionPage��$
 DataContext.LeftDown� 5\      �� $	 FunctionPage��$"
 DataContext.MouseMoveCommand� 5]      �� $	 FunctionPage��$
 DataContext.LeftUp�.+�5_      �� -	   5        $ Faceline_layer0��5_      ��5`      �� $	 
image_view��$
 ActualWidth���5a      �� $	 
image_view��$
 ActualHeight�.+�5b      F� -
   5        $��midline��5b      
���   �$��1��6R   
���   ���6*   �� *	midline+y�5c      �� $	 FunctionPage��$#
 DataContext.FaceLineVisiblity�5d   "   F� 5e      -   5        $��
noseline_L��5e      
���  ��$��1��6V   
���   ���6-   �� *
noseline_L+y�5f      �� $	 FunctionPage��$#
 DataContext.FaceLineVisiblity�5g   "   F� 5h      -   5        $��
noseline_R��5h      
��� � �$��1��6X   
���   ���6-   �� *
noseline_R+y�5i      �� $	 FunctionPage��$#
 DataContext.FaceLineVisiblity�5j   "   F� 5k      -   5        $��EyeLine��5k      
���   �$��1��6R   
���   ���6*   �� *	eyeline+y�5l      �� $	 FunctionPage��$
 DataContext.LineVisiblity�5m   "   F� 5n      -   5        $��LipLine��5n      
���   �$��1��6R   
���   ���6*   �� *	lipline+y�5o      �� $	 FunctionPage��$
 DataContext.LineVisiblity�5p   "   5q      z�5r      �� 5s      ! 6%   �� $	 FunctionPage��$$
 DataContext.FrontalPoints[0].X�" 5t   %   �� $	 FunctionPage��$$
 DataContext.FrontalPoints[0].Y�# 5u      �� $	 FunctionPage��$
 DataContext.rectsc�$ 5v      �� $	 FunctionPage��$
 DataContext.rectsc�6`   5x      5y      (  "MaterialDesignThemes.Wpf.ColorZone  5~   
   $��0��6#   $��3��60   $��16q�6D   +  %MaterialDesignThemes.Wpf.ShadowAssist%   ShadowDepth$% Depth2=�6Q   &   Mode$& 
PrimaryMid=�5   -   $	��Top=�6?   $��53��6T   .��5�      ]� .+�5�      '  !MaterialDesignThemes.Wpf.PopupBox  $��Right=�6+   '   PlacementMode$' BottomAndAlignRightEdges=�6B   (   	StaysOpen( .  $��42��6}   
�������$
��Left=�6�   $��22��6�   )   PopupContent.) 5�      �� .+�5�      �� -   5        $��로그아웃��5�   !   60   �� 5�      �� 5�      -   5        $��	끝내기��5�   !   6/   5�      5�      #	  MaterialDesignThemes.Wpf.Chip	  5�      $��James Willock��6&   $��	0,-1,0,-7q�6>   $	��136��6Q   $��29��6]   $��Center=�6i   $��Right=�6�   * 	  Icon* 5�      �� 5�      $C��=C:/Users/bit/source/repos/dental align exam1/image/dental.PNG��6    5�      �� 5�      $��22(�6   
�������$	��232��6=   $��Center=�6I   $��Center=�6f   $
��0,-4q�6�   Medit Design6�   5�      5�      � 5�   
   -   5        + �Name$+ 
flow_chart��5�      $��1��6#   $��Stretch=�60   $��Stretch=�6L   .+�5�      � $��
30,20,0,20q�6    5�      �� 5�      $	��15* �6'   65   �� 5�      $	��70* �6'   65   �� 5�      $	��15* �6'   65   .+�5�      (� $
��Left=�6   $��0��65   .��5�      ��  *  &MaterialDesignFloatingActionMiniButton#���   6   $��Center=�5�      $
��Left=�64   $��0q�5�      ��5�      �� *prevFlowClick+.��5�      '
  !MaterialDesignThemes.Wpf.PackIcon
  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=null]],   Kind$, StepBackward=�62   5�      5�      �� 5�      $��1��6   $��0q�6,   $��Center=�5�      $��Center=�67   $��16(�6T   ��5�   7   �� *
changeText+5�      (� 5�      $��2��6   $��Right=�6*   .��5�      ��  , (MaterialDesignFloatingActionAccentButton#���  6   $��0q�5�      - J� RenderTransformOrigin$- 9.282,0.533&�6$   $��Right=�6H   ��5�      �� *nextFlowClick+.��5�      
  $, StepForward=�62   5�      5�      5�      5�      � 5�   
   $��2��6   $��0��6   $��	10,0,0,10q�6,   .+�5�      (   "Process_Page.Cards.FaceAlign_Step2  -   5        .  Name$#. FaceAlignControl_ReUnDoSearCh��5�   $   y�6K   �� $	 FunctionPage��$
 DataContext.showControl0�)   #Process_Page.Cards.Measurement_Step  5�      -   5        /  Name$/ MeasureControl��5�   %   $��	0,0,-10,0q�6�   y�6=   �� $	 FunctionPage��$
 DataContext.showControl1�6�   ,   &Process_Page.Cards.TeethTemplate_Step2  5�      -   5        0  Name$0 ToothControl2��5�   (   $��	0,0,-10,0q�6�   y�6?   �� $	 FunctionPage��$
 DataContext.showControl2�6�   5�      � 5�   
   -   5        $+ control_Changed��5�      $��3��6(   $��0��65   $��2��6E   $��35,0,0,0q�6V   .+�5�      +   %Process_Page.Cards.TeethTemplate_Step  -   5        1  Name$1 ToothControl��5�   '   $��-24,0,-10,0q�6�   y�6=   �� $	 FunctionPage��$
 DataContext.showControl2�6�   '   !Process_Page.Cards.FaceAlign_Step  5�      -   5        2  Name$2 FaceControl��5�   #   $	��-10q�6�   y�68   �� $	 FunctionPage��$
 DataContext.showControl0�6�   5�      � 5�   
   $��1��6   $��2��6   $��2��6,    5�      �� 5�      $
��Auto �6    60   �� 5�      $
��Auto �6    60   .+�5�      � $��0��6   $��
10,10,30,0q�6    $��Stretch=�64   .+�5�      � $��10q�6    5�      �� 5�      $	��20* �6(   �� 5�      $	��10* �6(   67   �� 5�      $	��20* �6(   67   �� 5�      $	��50* �6(   67   .+�5�      �� $��Center=�6    $��Center=�6=   $��0��5�       # MaterialDesignHeadlineTextBlock#��[ 6&   $��	Dr. Smith��5�   C   5�      �� 5�      $��Center=�6    $��Center=�6=   $��1��5�       % !MaterialDesignSubheadingTextBlock#��[ 6&   $��Shara James��5�   E   5�      �� 5�      $��2��5�        MaterialDesignDarkSeparator#��[ 5�      $��8 0 8 0q�5�      �� 5�      $��3��6    * &MaterialDesignOutlinedTextFieldTextBox#��[ 5�      $��5q�5�      $��Stretch=�5�      3 �� AcceptsReturn3 . 4 �� TextWrapping$
4 Wrap=�5�      "5 �� VerticalScrollBarVisibility$
5 Auto=�5�      )  #MaterialDesignThemes.Wpf.HintAssist6   Hint$
6 Memo��5�      {�5�      �� $
 	IsChecked�$;	 5MaterialDesignOutlinedTextFieldTextBoxEnabledComboBox��5�      5�      5�      .   (Process_Page_Change.Cards.layoutTemplate  5�   
   -   5        7  Name$7 layoutcontrol��5�       $��4��67   $��1��6D   $��Bottom=�5�   #   $��Stretch=�6>   $y�Visible=�5�   #   �� 5�   
   -   5        8 ��Name$8 
SaveButton��5�      $
��save��6;   $��0��6J   $��Stretch=�6Z   $��20q�6x   $��5��6�   $	��Top=�6�   y�5�      �� $	 FunctionPage��$
 DataContext.showControl2���6a   �� *Savecommand+)  #MaterialDesignThemes.Wpf.DialogHost  5�   
   9   
Identifier$9 
RootDialog��6$   $��5��6�   $��3��6�   :   SnackbarMessageQueue: 6<   �� $	 MainSnackbar��$
 MessageQueue�5�      5�      5�      