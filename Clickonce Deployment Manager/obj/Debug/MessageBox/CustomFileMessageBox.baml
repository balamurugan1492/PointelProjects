   M S B A M L   `   `   `  ����   Clickonce Deployment ManagerE    ?Clickonce_Deployment_Manager_32.MessageBox.CustomFileMessageBox   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � AllowsTransparency  .  � Topmost .  � ShowInTaskbar .   � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�6D      WindowMessageBoxBG#���   5       � SizeToContent$ WidthAndHeight=�6G   .��5      �  � RowDefinitions 5	   
   �� 5
      $��30 �6   6*   �� 5      $��* �6   6)   �� 5      $��40 �6   6*   .+�5   
   � -   5          TitleBar#���  5      .+�5      �� -   5        $��Setup_Title��5   )   $��5,0,0,0q�6   $��13(�6<   
�������$
��Bold"�6]   $��Center=�6o   6�   5      �� 5   
   $��1��6   .+�5      �� -   5        $��TxtBlockMessage��5   )   $��5,5,5,5q�6   $
��Wrap=�6@   $	��400��6T   6e   5      �� 5   
   $��2��6   $��30��6"   $��Center=�6.   .+�5      ]� .+�5      �� -   5        $
��Btn1��5   r   $��Right=�6   $��0,0,20,0q�61   $��5,5,5,5q�6D   $��Right=�6V     ButtonStyle#���  6�   $��60��6�   $��Cancel��6�   6�   �� 5      -   5        $
��Btn2��5   t   $��Right=�6   $��0,0,20,0q�61   $��5,5,5,5q�6D   $��Right=�6X   #���  6�   $��60��6�   $	��Yes��6�   6�   �� 5      -   5        $
��Btn3��5   q   $��Right=�6   $��0,0,20,0q�61   $��5,5,5,5q�6C   $��Right=�6U   #���  6�   $	��120��6�   $�� Copy To SubFolder��6�   6�   5      5      5      5      