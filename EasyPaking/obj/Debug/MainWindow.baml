   M S B A M L   `   `   `  ����E  AEasyPaking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EasyPaking
EasyPaking  5   	   d `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF 5   	       EasyPaking.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:EasyPaking     5   	   `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     5   	   _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   -   5          � Title$  
MainWindow��5
   	   $	��600��6   $
��1180��6)    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.    � WindowStyle$
 None=�5   	   ���	#FF3A3A3A.��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��500 �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��10q�6"   .+�5      � .+�5      �� 
�������$��	10 0 25 0q�6-   '  !MaterialDesignThemes.Wpf.PackIcon  5         Kind$ 	MapMarker=�6*   $��Center=�6;   $��2q�6V   �� 5      $��Right=�6      MaterialDesignFlatButton#��[  65   .��5        $ Search=�6.   5      5       �� 5!       �� Orientation$ 
Horizontal=�6   .+�5"      �� $��10q�6   $��Ethernet Port��6(   $��17(�6=   
�������$
��Bold"�6^   �� 5#      $��150, 10q�6   $��	Comm Port��6-   $��17(�6>   
�������$
��Bold"�6_   5$      �� 5%      $ 
Horizontal=�6   .+�5&      �� $��
Server IP:��6   $��15(�6.   
�������$��Normal"�6O   �� 5'      -   5         ��Name$ severIPcomboBox��5'      
�������$	��180��6G   $��25, 0, 0, 0q�6S   �� 5)      $��
Port Name:��6   $��15(�6.   
�������$��Normal"�6O   $
��10,0q�6c   �� 5*      -   5        $ CommPort_comboBox��5*      
�������$	��180��6I   $
��0, 0q�6U   5+      �� 5.      $ 
Horizontal=�6   $��0, 10, 0, 0q�62   .+�5/      �� $��Server Port:��6   $��15(�60   
�������$��Normal"�6Q   �� 50      -   5        	 ��Name$	 severPorttextBox��50      $
��6000��64   $	��180��6@   
�������$��10, 0q�6_   �� 52      $��
Baud Rate:��6   $��15(�6.   
�������$��Normal"�6O   �� 53      -   5        $ CommBaud_comboBox��53      
�������$	��180��6I   $��10, 0, 0, 0q�6U   .@�6j   .@�54      \ �� $
��2400��6#   �� 55      $
��4800��6#   �� 56      $
��9600��6#   �� 57      $��19200��6#   �� 58      $��38400��6#   �� 59      $��57600��6#   �� 5:      $��115200��6#   5;      5<      �� 5?      $ 
Horizontal=�6   $��0, 10, 0, 0q�62   .+�5@      �� $��WDC IP:��6   $��15(�6+   
�������$��Normal"�6L   �� 5A      -   5        
 ��Name$
 deviceIPstatelabel��5A      $��0.0.0.0��68   
�������$��Normal"�6Z   $��15(�6n   $��32, 0q�6|   5B      �� 5C      $ 
Horizontal=�6   $��0, 10, 0, 0q�62   .+�5D      �� $��Connect State:��6   $��15(�62   
�������$��Normal"�6S   �� 5E      -   5        $
 connectstatelabel��5E      $��Waiting Connect...��67   $��15(�6Q   
�������$��Normal"�6r   $��10, 0q�6�   5F      �� 5G      $ 
Horizontal=�6   .+�5H      �� -   5         ��Name$ startListenbutton��5H      $��Start  Listen��64   $��16(�6L   +  %MaterialDesignThemes.Wpf.ShadowAssist   Darken . 
�������$	��200��5I   +   $��15, 0q�67   6F   �� 5J      -	   5        $
 listenstatelabel��5J      $	��●��66   $��60(�6?   $	��100��6M   
������� . 6p   �� 5M      -
   5        $ OpenClosePort_Button��5M      $��
Open  Comm��67   $��16(�6L    . 
�������$	��150��5N   +   $��15, 0q�67   6F   �� 5O      -   5        $
 CommStatus_label��5O      $	��●��66   $��60(�6?   $	��100��6M   
������� . 6p   5P      �� 5Q      -   5        $	 richTextBox1��5Q      
�������$	��250��6?   5R      �� 5T   
   -   5        $��1��5T      $��Right=�6!   #��[  6=   .��5U        $ Close=�6&   5V      '  !Microsoft.Maps.MapControl.WPF.Map  5X   
   +  %Microsoft.Maps.MapControl.WPF.MapCore   CredentialsProviderT  NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F @AhlbEXgAQBlU7Ms9Envm8nUN-YYGavRqWhwkIUDGO7e-5D0zYm_ktwLAZsWZjYw4 6      Mode;  5Microsoft.Maps.MapControl.WPF.Design.MapModeConverter$
 Road 6g   $��1��6s   $��1��6�      	ZoomLevel$ 16Q�6�      Center<  6Microsoft.Maps.MapControl.WPF.Design.LocationConverter$ 43.196171, 76.870633 6�      Children. 5Y      �� ,  &Microsoft.Maps.MapControl.WPF.MapLayer   Position$ 43.196171, 76.870633 5Z         PositionOriginB	  <Microsoft.Maps.MapControl.WPF.Design.PositionOriginConverter$ BottomCenter	 5[      $��30��66   $��30��6A   .+�5^        $ 	MapMarker=�6+   $��30��6<   $��30��6G   ���	#FF3C3C3C6S   5_      �� 5a      $ 43.19548, 76.870958 6   .+�5b      �� -   5        $
 lotColor��5b      $	��●��6.   
��� � �$��40(�6I    �� TextAlignment$ Center=�6X   5c      5d      5e      5f      