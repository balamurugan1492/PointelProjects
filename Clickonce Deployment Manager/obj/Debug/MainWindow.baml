   M S B A M L   `   `   `  ����   Clickonce Deployment Manager0    *Clickonce_Deployment_Manager_32.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  
MainWindow��5   	    � WindowStartupLocation$ CenterScreen=�6   $	��535��6C   $	��535��6R   $	��535��6^    � Icon$A ;/Clickonce%20Deployment%20Manager;component/Images/logo.ico��6�    � WindowStyle$
 None=�5   	      WindowBG#���   6    � AllowsTransparency . .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6    � RowDefinitions 5
   
   �� 5      $��30 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   � -   5        $��0��5   >     TitleBar#���  6L   $��2��6{   .+�5      �� $��3,0,0,0q�6   $��Images/PointelLogo.ico��6%   $��20��6E   $��20��6P   $
��Left=�6\   6y   �� 5      -   5        $��Setup_Title��5   *   $��28,0,0,0q�6   $2��,Agent Interaction Desktop Deployment Manager��6>   $��13(�6r   
�������$
��Bold"�6�   $��Center=�6�   6�   �� 5      -   5        $��LblExitButton��5      $��Right=�6)   $��0,0,0,0q�6E   $��20��6V   $��21��6b   .��5      �� -   5        $��	imagClose��5      $��Images/Custom Close.png��6)   ��5      �� 5       �� 
TargetType" ��6    �� Triggers 5      P� 5      	 P� Property	 �@J�IsMouseOver
 P� Value
 . . �5   "   ��  �� Property � �� �� Value
 �����6?   5      5      5      55      � 56   
   $��1��6   .+�57       � -   5        $��	mainFrame��57        � NavigationUIVisibility$ Hidden=�6%   6E   58      � 59   
   $��2��6    
 Footer#��[ 6   .+�5:      �� ���#ACA899$��0,1,0,0q�6+   ._�5;      ]� .+�5<      �� -   5        $��BtnNext��5<   �   $��Right=�6   $��60��64   $��Next >��6?   $��0,0,20,0q�6S   $��Right=�6f     ButtonStyle#���  6�   6�   �� 5=      -   5        $��BtnBack��5=   �   $��Right=�6   $��< Back��64   $��0,0,20,0q�6H   $��60��6Z   $��25��6e   $��Right=�6q   $y�	Collapsed=�6�   #���  6�   6�   5>      5?      5@      5A      5B      