   M S B A M L   `   `   `  ����F  BICMS_Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ICMS_ServerICMS_Server  5      )    #ICMS_Server.AddEditOptionCouponView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      %localclr-namespace:ICMS_Server  5      V RMaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=nullX TMaterialDesignThemes.MahApps, Version=0.0.10.0, Culture=neutral, PublicKeyToken=nullHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5	      O	 KWPFLocalizeExtension, Version=3.3.1.0, Culture=neutral, PublicKeyToken=null2lex(http://wpflocalizeextension.codeplex.com 	 5
         MaterialDesignBody#���   5      $��Regular"�5      $��14(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5       � 	Resources 5       �� 
TargetType �� Property �� Value  IconDeviceTemp'  !MaterialDesignThemes.Wpf.PackIcon�  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null]]   Kind��%�   '��       &	 L     �� 5   
   " ��6   .�5      ��  � ��$ Center=�6;   5      �� 5   
   .R�5        $ CursorDefault=�6&   $��Center=�5   &   5      5       J� InputBindings 5      �� 5   
   
 �� Key$ Delete��5      	 �� Command	 5      �� *btn_delSelected+5      �� 5   
   $ S��5      
 �� 	Modifiers$

 Ctrlf�5       	 5      �� *btn_saveFileAs+5       �� 5!   
   $ O��5"      $

 Ctrlf�5#      	 5!      �� *btn_openFile+5#      .��5&      � $	��500��6   $
��auto��6   {�5'      �� *add_edit_coupon_check+ � RowDefinitions 5(   
   �� 5)      $��* �6   6)   �� 5*      $
��auto �6   6,   .+�5,   
   � $��25q�6   .+�5-      �� .+�5.      ]� .+�5/      �  � ColumnDefinitions 50      �� 51      61   �� 52      61   .+�55      ��  % !MaterialDesignFloatingHintTextBox#��[ 6"   $��Stretch=�57      $
��auto��58       �� TextWrapping$
 Wrap=�59      $
��auto��5;      )  #MaterialDesignThemes.Wpf.HintAssist   Hint 56      2 	 ,WPFLocalizeExtension.Extensions.LocExtension  
   Key$ username����5:      �� *txt_username+ �� UpdateSourceTrigger$ PropertyChanged=�.��5;   #   5<      �� 5>      -   5         ��Name$ 
item_group��5>   #   $��1��5?   #    & "MaterialDesignFloatingHintComboBox#��[ 5@   #   $��Stretch=�5A   #   $
��auto��5C   #   $
��auto��5D   #   ��5B   #   �� *
group_item+ 5E   #     $ group��{�5F   #   �� *group+.  (System.Windows.Interactivity.Interaction   Triggers 5G      /  )System.Windows.Interactivity.EventTrigger  5H   "      	EventName$ Loaded��61   .  (System.Windows.Interactivity.TriggerBase   Actions. 5I   &   6	  0System.Windows.Interactivity.InvokeCommandAction	   	  Command 6<   �� *
item_group+ 	  CommandParameter 5J   0   ��  �� ElementName$ 
item_group��5K   #     5L   "   $ SelectionChanged��62   . 5M   &   	   6<   �� *item_group_change+ 5N   0   �� $ 
item_group��5O   #   5Q      5S      5T      ]� 5V      .+�5W      �  5X      �� 5Y      61   �� 5Z      61   .+�5]      �� #��[ 6"   $��Stretch=�5_      $
��auto��5`      {�.  $
 Wrap=�5b      $
��auto��5d       5^        $ hr_rate����5c      �� *txt_hr_rate+$ PropertyChanged=�.��5d   #   5e      5g      5h      ]� 5j      .+�5k      �  5l      �� 5m      61   �� 5n      61   .+�5q      �� -   5        $��price��5q   "   #��[ 6/   $��Stretch=�5s      $
��auto��5t      $
 Wrap=�5u      $
��auto��5w       5r        $ hr_price����5v      �� *txt_hr_price+$ PropertyChanged=�.��5w   #    5x        5y   "   $ TextChanged��61   . 5z   &   	   6<   �� *price+ 5{   0   �� $ price��5|   #   .��5}   6   5~      5�      5�      ]� 5�      .+�5�      �  5�      �� 5�      $	��74* �6/   6=   �� 5�      $
��151* �6/   �� 5�      $
��225* �6/   6>   .+�5�      �� -   5         ��Name$ 
free_money��5�   "   #��[ 66   $��Stretch=�5�      $
��auto��5�      $
 Wrap=�5�      $
��auto��5�      $��2��6#    5�        $ 
free_money����5�      �� *txt_free_money+$ PropertyChanged=�.��5�   7    5�        5�   "   $ TextChanged��61   . 5�   &   	   6<   �� *
free_money+ 5�   0   �� $ 
free_money��5�   #   .��5�   6   5�      5�      5�      ]� 5�      .+�5�      �  5�      �� 5�      61   �� 5�      61   .+�5�      �� -   5        $ total_amount��5�   "   #��[ 68   $��Stretch=�5�      $
��auto��5�      {�.  $
 Wrap=�5�      $
��auto��5�       5�        $ total_amount����5�      �� *txt_total_amount+$ PropertyChanged=�.��5�   #    5�        5�   "   $ TextChanged��61   . 5�   &   	   6<   �� *total_amount+ 5�   0   �� $ total_amount��5�   #   .��5�   6   5�      � 5�      $��1��6    5�      �� 5�   "   65   �� 5�   "   65   .+�5�      �� -   5        $ remaining_hh��5�   &   #��[ 6<   $��Stretch=�5�      $
��auto��5�      {�.  $��0,0,5,0q�5�      $
 Wrap=�5�      $
��auto��5�       5�        $ add_hh����5�      �� *
txt_add_hh+$ PropertyChanged=�.��5�   #   5�      �� 5�      -   5        $ add_mm��5�   6   $��1��6&   #��[ 6F   $��Stretch=�5�      $
��auto��5�      {�.  $��5,0,0,0q�5�   &   $
 Wrap=�5�      $
��auto��5�       5�        $ add_mm����5�      �� *
txt_add_mm+$ PropertyChanged=�.��5�   #   5�      5�      5�      5�      ]� 5�      .+�5�      �  5�      �� 5�      61   �� 5�      61   .+�5�      �� #��[ 6"   $��Stretch=�5�      $
��auto��5�      $
 Wrap=�5�      $
��auto��5�       5�        $ exp_date����5�      �� *txt_exp_date+$ PropertyChanged=�.��5�   #   5�      5�      5�      �� 5�      $��0,10,0,0q�6   .+�5�      	�   MaterialDesignRadioButton#��[ 6"   $��Center=�5�   "    d� 	IsChecked 5�   "   �� *start_create_date+��5�        $ start_create_date��6J   	� 5�      #��[ 6"   $��Center=�5�   "    5�   "   �� *start_first_use+��5�        $ start_first_use��6H   5�      5�      5�      ]� 5�   
   $��1��6   $��Right=�6!   $	��Top=�5�      $��	0,0,25,25q�5�      .+�5�      ��  # MaterialDesignRaisedLightButton#��[ 6   $��8q�5�      $	��100��5�      $��35��5�      ��5�      �� *btn_ok+ � ToolTip 5�        $ ok����5�        $ ok��5�      �� 5�      $��
5, 0, 0, 0q�6   �� 5�      #��[ 6   $��8q�5     $	��100��5     $��35��5     ��5      �� *
btn_cancel+ 5       $ cancel����5       $ cancel��5     5     5     5     