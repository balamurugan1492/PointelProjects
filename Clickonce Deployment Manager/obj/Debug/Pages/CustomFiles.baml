   M S B A M L   `   `   `  ����W  SClickonce Deployment Manager, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nulla5clr-namespace:Clickonce_Deployment_Manager_32.Classes'Clickonce_Deployment_Manager_32.Classes  5      7    1Clickonce_Deployment_Manager_32.Pages.CustomFiles   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:Clickonce_Deployment_Manager_32.Classes     5      -   5          N� Title$  CustomFiles��5       � 	Resources 5	      C  =Clickonce_Deployment_Manager_32.Classes.DisplayValueConverter   DisplayValueConverter�� 5
   
   %   &	          5      6*   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��70 �6   6*   �� 5      $��* �6   .+�5   
   �� $��0��6   ���#ACA899$��0,0,0,1q�64   ._�5      �  5      �� 5      $
��414* �6'   �� 5      $	��89* �6'   .+�5      �� $��2��6     Banner1#���  61   .+�5      �� $��10q�6    $��WrapWithOverflow=�6,   $
��Bold"�6L   $	��400��6^   $��12(�6j   $
��Left=�6x   $	��Top=�6�   Custom Files6�   �� 5      $��WrapWithOverflow=�6    $��10,0,0,0q�6@   $	��400��6R   $
��Left=�6_   $	��Top=�6z   &$Add/Remove the optional custom files6�   5      �� 5       $��1��6   $��Center=�6(   $��Center=�6E   $A��;/Clickonce%20Deployment%20Manager;component/Images/logo.ico��6`   $��48��6�   $��48��6�   $
��0,10q�6�   6�   5!      5"      � 5$   
   $��1��6    5%       �� 
TargetType �� Property �� Value��%   '��       �� 5&      " ��6   .�5'      ��  � ��$
 10,2q�6/   �� 5(       � ��$
 10,2q�60   6?   5)      5*      .+�5,      � $��0��6   $
��15,5q�6&   .+�5-      ]� .+�5.      �� -   5        $��BtnAdd��5.   `   $	��Add��6   $
��Left=�6,   $��0,0,20,5q�6C   $��60��6U     ButtonStyle#���  6�   6�   �� 5/      -   5        $��	BtnRemove��5/   .   $��Remove��6   $��0,0,20,5q�6?   $��60��6Q   #���  6t   6�   50      &   System.Windows.Controls.DataGrid  51      -   5        $��DGCustomFiles��51   w      RowHeaderWidth$ 0Q�6      VerticalGridLinesBrush
 �   �	   
IsReadOnly	 . 
   SelectionUnit$
 FullRow=�6_   $	��490��6�   $	��490��6�   $	��230��6�   $	��230��6�      AutoGenerateColumns .     MinColumnWidth$ 30Q�6�      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ 30 6�      ColumnHeaderStyle 52      �� 53      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�54      ��  � ��$ Center=�6I   �� 55       � ��$ Stretch=�6D   �� 56       � ��$ Center=�6K   �� 57       � ��
 �   �6;   �� 58       � ��$ 25��67   �� 59       � �� 5:   "   �� 5;   &    �� 
TargetType"  66   .R�5<   *   � .+�5=   .   �� $��
MainBorder��65   $��1q�6G   $��0,0,0,0q�6[   
���   ����#858C93._�5>   2   � .+�5?   6   �� $��3q�6G   $��Center=�6R   $��Center=�6o   #��z��6�    �� ContentStringFormat �� ContentStringFormat# z 5@      #��z��6L   5A   3   5B   /   5C   +   5D   '   5F      5G         Columns 5I      4  .System.Windows.Controls.DataGridTemplateColumn  5K      ,  &System.Windows.Controls.DataGridColumn   Width$ * 61      Header 5L      �� 5M   "   $��	File Name��6,   $��Center=�6=   6\      CellTemplate 5P      �� 5Q   "   .R�5R   &   �� ��60   ��  �� Path$ 
TargetPath� �� 	Converter# [   � ToolTip 6�   �� $ 
TargetPath�# [  5S   '   5T   #   5V        5X      $ * 61    5Y      �� 5Z   "   $��Relative Path��6,   $��Center=�6A   6`    5\      �� 5]   "   .R�5^   &   �� ��60   �� $ ResolvedPath� 6S   �� $ ResolvedPath�5_   '   5`   #   5b        5d      $ 50 61    5e      �� 5f   "   $��Datafile��6,   $��Center=�6<   6[    5h      �� 5i   "   .R�5j   &   �� $��chbDataFile��6M   $��Center=�60    d� 	IsChecked 6`   �� *
IsDataFile+6�   5k   #   5m      5o      5p      5q      5r      5s      