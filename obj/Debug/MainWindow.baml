   M S B A M L   `   `   `  ����B  >Glonass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:GlonassGlonass  5   	       Glonass.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Glonass     5   	   -   5          �Name$  MainWindow1��5   	    � Title$ Glonass��5   	   $	��900��6   $
��1600��6&   ���	#FF252525.��5	      � -   5         �Name$ MainGrid��5	      $��	-5,0,2,-2q�6   .+�5
   
   �� -   5         ��Name$ 	CanvasMap��5
      $��10,10,330,3q�6$   69   �� 5   
   -   5         ��Name$ ButtonGenerate��5      $��Generate��6)   $��	0,10,10,0q�6<   $	��Top=�6O   $��Right=�6�   $	��315��6�   �� 5   
   -   5         ��Name$ SliderCitiesCounter��5      $��	0,35,10,0q�6.   $	��Top=�6A    J� RenderTransformOrigin$ 1.477,1.101&�6Y   $��22��6}    � Value$ 2Q�6�    � Minimum$ 4Q�6�   	 � Maximum$	 50Q�6�   
 � SmallChange$
 1Q�6�    �� IsSnapToTickEnabled . $��Right=�6  $	��315��6#  �� 5   
   -   5         ��Name$ tbLog��5      $��Right=�6!   $��	0,0,10,10q�6=    �� TextWrapping$
 Wrap=�6P   $	��315��6d   $	��492��6p   $��Bottom=�6}   
�������&   System.Windows.Media.TextOptions   TextHintingMode$ Animated=�6�   ��6�   Z� ��6�   Z� 6�   �� 5   
   -   5        $ ButtonSlove��5      $��Solve��6&   $��	0,62,10,0q�66   $	��Top=�6I   $��Right=�6{   $	��315��6�   �� 5   
   -   5        $ ButtonStopSolving��5      $
��Stop��6,   $��Right=�6;   $��	0,87,10,0q�6W   $	��Top=�6j   $	��315��6�   6�   �� 5   
   -	   5        $ ButtonShowBest��5      $��	Show best��6)   $��
0,112,10,0q�6=   $	��Top=�6Q   $��22��6i   $��Right=�6�   $	��315��6�   �� 5   
   -
   5        $ ButtonDrawGen��5      $��Generate genetics object��6(   $��
0,192,10,0q�6K   $	��Top=�6_   $��Right=�6�   $	��315��6�   �� 5   
   -   5        $ ButtonStepp��5      $!��Solve(leap forward X-steps)��6&   $��
0,217,10,0q�6L   $	��Top=�6`   $��Right=�6�   $	��315��6�   �� 5   
   -   5        $ buttonStopThread��5      $
��Stop��6+   $��
0,242,10,0q�6:   $	��Top=�6N   $��Right=�6�   $	��315��6�   �� 5   
   -   5         ��Name$ labelPopCount��5      $�� ��6'   $��
0,279,10,0q�62   $	��Top=�6F   ���	#FFDCD6D6$��Center=�6u   $��Center=�6�   $��18(�6�   $��Right=�6�   $	��315��6�   5      5      