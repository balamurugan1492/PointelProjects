   M S B A M L   `   `   `  ���� W  SClickonce Deployment Manager, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nulla5clr-namespace:ClickOnce_Deployment_Manager_64.Classes'ClickOnce_Deployment_Manager_64.Classes  7    1ClickOnce_Deployment_Manager_64.Pages.CustomFiles   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Glocal5clr-namespace:ClickOnce_Deployment_Manager_64.Classes     -     N� Title$  CustomFiles�� � 	Resources C  =ClickOnce_Deployment_Manager_64.Classes.DisplayValueConverter   DisplayValueConverter�� %   &	          .���  � ColumnDefinitions �� $��* � � RowDefinitions �� $��70 ��� $��* �.+��� $��0�����#ACA899$��0,0,0,1q�._��  �� $
��414* ��� $	��89* �.+��� $��2��  Banner1#���  .+��� $��10q�$��WrapWithOverflow=�$
��Bold"�$	��400��$��12(�$
��Left=�$	��Top=�Custom Files�� $��WrapWithOverflow=�$��10,0,0,0q�$	��400��$
��Left=�$	��Top=�&$Add/Remove the optional custom files�� $��1��$��Center=�$��Center=�$A��;/Clickonce%20Deployment%20Manager;component/Images/logo.ico��$��48��$��48��$
��0,10q�� $��1��  �� 
TargetType �� Property �� Value��%G   '��       �� " ��.���  � ��$
 10,2q���  � ��$
 10,2q�.+�� $��0��$
��15,5q�.+�]� .+��� -   $��BtnAdd��$	��Add��$
��Left=�$��0,0,20,5q�$��60��  ButtonStyle#���  �� -   $��	BtnRemove��$��Remove��$��0,0,20,5q�$��60��#���  &   System.Windows.Controls.DataGrid  -   $��DGCustomFiles��   RowHeaderWidth$ 0Q�   VerticalGridLinesBrush
 �   �	   
IsReadOnly	 . 
   SelectionUnit$
 FullRow=�$	��490��$	��490��$	��230��$	��230��   AutoGenerateColumns .     MinColumnWidth$ 30Q�   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ 30    ColumnHeaderStyle �� =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  � ��$ Center=���  � ��$ Stretch=���  � ��$ Center=���  � ��
 �   ���  � ��$ 25����  � �� ��  �� 
TargetType"  .R�� .+��� $��
MainBorder��$��1q�$��0,0,0,0q�
���   ����#858C93._�� .+��� $��3q�$��Center=�$��Center=�#��z�� �� ContentStringFormat �� ContentStringFormat# z #��z��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width$ *    Header �� $��	File Name��$��Center=�   CellTemplate �� .R��� ����  �� Path$ 
TargetPath� �� 	Converter# [   � ToolTip �� $ 
TargetPath�# [    $ *  �� $��Relative Path��$��Center=� �� .R��� ���� $ ResolvedPath� �� $ ResolvedPath�  $ 50  �� $��Datafile��$��Center=� �� .R��� $��chbDataFile��$��Center=� d� 	IsChecked �� *
IsDataFile+