   M S B A M L   `   `   `  ����I  EAbaloneGameWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AbaloneGameWPFAbaloneGameWPF  5   	       AbaloneGameWPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:AbaloneGameWPF     5   	     � Title$  Abalone��5   	   $	��700��6   $
��1000��6&    � 	Resources 5       �� 
TargetType �� Property �� Value��%R   '��       �� 5   
   " ��6   .�5      ��  � ��$ 35(�6)   5      5      .��5      � -   5        .+�5   
   �� -   5         ��Name$ New_Game_Button��5      $��New Game��6*   $
��Left=�6=   $��715,100,0,0q�6X   $	��Top=�6m   $	��200��6�   $	��100��6�   6�   �� 5   
   -   5         ��Name$ 	CanvasXML��5      $
��Left=�6%   $	��Top=�6@   $	��600��6X   $	��600��6e   $��	30,30,0,0q�6q   .+�5      �� -   5         ��Name$ Borad_Image_XML��5      $
��Left=�6.   $��Images/abaloneboard.png��6J   $	��600��6k   $	��600��6x   5       �� 5!   
   -   5         ��Name$ Black_Ejected_Label��5!      $��	black 0/6��6-   	 K� ZIndex$	 10��6A   $	��100��6S   $	��100��6`   $��30,69,862,501q�6l   �� 5"   
   -   5        $ White_Ejected_Label��5"      $��	white 0/6��6-   $	 10��6A   $	��100��6S   $	��100��6`   $��30,490,862,80q�6l   �� 5#   
   -   5        $ Currnt_Turn_Label��5#      $��white��6+   $	 20��6;   $	��200��6M   $	��100��6Y   $��715,222,77,348q�6f   	� 5$   
   -   5        
 	�Name$$
 Play_Against_Human_RadioButton��5$      $��Play Against Human��6>   $
��Left=�6[   $	��200��6v   $��25��6�   $��715,350,0,0q�6�   $	��Top=�6�   $��20(�6�   6�   	� 5%   
   -	   5        $"
 Play_Against_Bot_RadioButton��5%      $��Play Against Bot��6<   $
��Left=�6W   $	��200��6r   $��25��6~   $��715,380,0,0q�6�   $	��Top=�6�   $��20(�6�   6�   5(      5)      