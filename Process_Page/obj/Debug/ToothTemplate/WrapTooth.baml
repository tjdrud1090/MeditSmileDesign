   M S B A M L   `   `   `  ����G  CProcess_Page, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Process_Page.ToothTemplateProcess_Page.ToothTemplate  5      5clr-namespace:Process_Page.UtilProcess_Page.Util  5	      [2clr-namespace:Process_Page.ToothTemplate.ArrowLine$Process_Page.ToothTemplate.ArrowLine  5
      *    $Process_Page.ToothTemplate.WrapTooth   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Process_Page.ToothTemplate     5      :MouseBehaviorsclr-namespace:Process_Page.Util     5	      DArrow2clr-namespace:Process_Page.ToothTemplate.ArrowLine     5
      .��5      �� -   5          ��Name$  Border_WrapTooth��5      $��1q�6'   
������  J� Opacity$ 1Q�6U   ._�5   
   � -   5         �Name$ Grid_WrapTooth��5      .+�5      �� -   5        $  
Border_Top��5       � Cursor$ SizeNS��6)   $	 0.5Q�69   $y�Hidden=�6G   $��5��5      $��Stretch=�6    $	��Top=�6>      BorderStyle#��[  6V   &    Process_Page.Util.MouseBehaviour!   MouseLeftButtonDownCommand 5      �� *MouseLeftDownForResizeTooth+   MouseMoveCommand 5      �� *MouseMoveForResizeTooth+   MouseLeftButtonUpCommand 5      �� *MouseLeftUpForResizeTooth+��5      �� 5       �� 
StartPoint$ 0.5, 0&�6*    �� EndPoint$ 0.5 1&�6>   .��5      � 	 � Color$	 White��6'   
 � Offset$
 0Q�65   � 5      $	 SkyBlue��6'   $	
 0.5Q�67   � 5      $	 White��6'   $
 1Q�65   5      5      4   .Process_Page.ToothTemplate.ArrowLine.ArrowLine  5      -   5          Name$ MoveTop��5      $y�Hidden=�5      
����·�$��2��6:   
���p�$
 Hand��6b   8   2Process_Page.ToothTemplate.ArrowLine.ArrowLineBase   	ArrowEnds$ Start=�5         ArrowLength$ 5Q�6'      IsArrowClosed .  5       �� *" MouseLeftDownForDragAndDropTooth+ 5!      �� *MouseMoveForDragAndDropTooth+ 5"      �� * MouseLeftUpForDragAndDropTooth+�� 5$      -   5        $  Border_Bottom��5$      $ SizeNS��6,   
����� �$ 0Q�6R   $��5��5(      $��Stretch=�6    $��Bottom=�6>   #��[  6Y    5%      �� *MouseLeftDownForResizeTooth+ 5&      �� *MouseMoveForResizeTooth+ 5'      �� *MouseLeftUpForResizeTooth+5(   Y   �� 5*      -   5        $  Border_Left��5*      $ SizeWE��6*   
�������$ 0Q�6R   $��5��5.      $
��Left=�6   $��Stretch=�6:   #��[  6V    5+      �� *MouseLeftDownForResizeTooth+ 5,      �� *MouseMoveForResizeTooth+ 5-      �� *MouseLeftUpForResizeTooth+5.   V   �� 50      -   5        $  Border_Right��50      $ SizeWE��6+   
��� ���$ 0Q�6N   $��5��54      $��Right=�6   $��Stretch=�6;   #��[  6W    51      �� *MouseLeftDownForResizeTooth+ 52      �� *MouseMoveForResizeTooth+ 53      �� *MouseLeftUpForResizeTooth+54   W   �� 56      -   5        $  Border_TopLeft��56      $ SizeNWSE��6-   
���z���$ 0Q�6W   $��5��5:      $��5��6    $
��Left=�6*   $	��Top=�6E   #��[  6]    57      �� *MouseLeftDownForResizeTooth+ 58      �� *MouseMoveForResizeTooth+ 59      �� *MouseLeftUpForResizeTooth+5:   ]   �� 5<      -	   5        $  Border_TopRight��5<      $ SizeNESW��6.   
�������$ 0Q�6W   $��5��5@      $��5��6    $��Right=�6*   $	��Top=�6F   #��[  6^    5=      �� *MouseLeftDownForResizeTooth+ 5>      �� *MouseMoveForResizeTooth+ 5?      �� *MouseLeftUpForResizeTooth+5@   ^   �� 5B      -
   5        $  Border_BottomLeft��5B      $ SizeNESW��60   
���� ��$ 0Q�6Z   $��5��5F      $��5��6    $
��Left=�6*   $��Bottom=�6E   #��[  6`    5C      �� *MouseLeftDownForResizeTooth+ 5D      �� *MouseMoveForResizeTooth+ 5E      �� *MouseLeftUpForResizeTooth+5F   `   �� 5H      -   5        $  Border_BottomRight��5H      $ SizeNWSE��61   
��� ���$ 0Q�6W   $��5��5L      $��5��6    $��Right=�6*   $��Bottom=�6F   #��[  6a    5I      �� *MouseLeftDownForResizeTooth+ 5J      �� *MouseMoveForResizeTooth+ 5K      �� *MouseLeftUpForResizeTooth+5L   a   �� 5N      -   5         ��Name$ 
Teethline5��5N      $��1��6'   
����·�$y�Visible=�6L   �� 5O      -   5        $ 
Teethline6��5O      $��1��6'   
����·�$y�Visible=�6L   �� 5P      -   5        $ 
Teethline7��5P      $��1��6'   
����·�$y�Visible=�6L   �� 5Q      -   5        $ 
Teethline4��5Q      $��1��6'   
����·�$y�Visible=�6L   �� 5R      -   5        $ 
Teethline3��5R      $��1��6'   
����·�$y�Visible=�6L   �� 5S      -   5        $ 
Teethline2��5S      $��1��6'   
����·�$y�Visible=�6L   �� 5T      -   5        $ 
Teethline1��5T      $��1��6'   
����·�$y�Visible=�6L   �� 5X      -   5         ��Name$ Canvas_Smile��5X      .+�5Y      F� -   5         F�Name$ 	SmileLine��5Y      
����·�$��3��6;   A� 5Z      -   5         A�Name$ LeftSmileControl��5Z      $ 0Q�64     SmileControlStyle#��[ 6@    5[      �� *MouseLeftDownForSmileLine+A� 5\      -   5        $ MidSmileControl��5\      $ 0Q�63   #��[ 6?    5]      �� *MouseMoveForSmileLine+A� 5^      -   5        $ RightSmileControl��5^      $ 0Q�65   #��[ 6A    5_      �� *MouseLeftUpForSmileLine+5`      5a      5b      5c      