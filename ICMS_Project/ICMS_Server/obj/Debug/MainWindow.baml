   M S B A M L   `   `   `  ����F  BICMS_Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ICMS_ServerICMS_Server  5   	       ICMS_Server.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   %localclr-namespace:ICMS_Server  5   	   O KWPFLocalizeExtension, Version=3.3.1.0, Culture=neutral, PublicKeyToken=null2lex(http://wpflocalizeextension.codeplex.com  5      V RMaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=nullX	 TMaterialDesignThemes.MahApps, Version=0.0.10.0, Culture=neutral, PublicKeyToken=nullHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  	 5   	   H
 DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;controls,http://metro.mahapps.com/winfx/xaml/controls 
 5   	     � WindowStartupLocation$  CenterScreen=�5	   	   $	��600��5
   	   $
��1080��6   4  .WPFLocalizeExtension.Engine.LocalizeDictionary   DesignCulture$ th��5   	   =  7WPFLocalizeExtension.Providers.ResxLocalizationProvider   DefaultAssembly$ ICMS_Server��5   	      DefaultDictionary$ resLang��5   	   ( 
 "MahApps.Metro.Controls.MetroWindow   	GlowBrush
 �   �$��0q�5   	      MaterialDesignBody#���   5   	   $��Regular"�5   	   $��14(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	      ShowIconOnTitleBar .   � Title$ InternetCafe (Alpha)��5      	   TitleCharacterCasing$	 Normal=�5   G   
   TitleAlignment$
 Center=�5       � 
ResizeMode$ 	CanResize=�5       � Icon$ Res/logo/icms.png��6.   .  (System.Windows.Interactivity.Interaction   Triggers 5       /  )System.Windows.Interactivity.EventTrigger  5!   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5"      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *WindowLoadedCommand+5#        5$   
   $ Closing��6   . 5%         6$   �� *WindowClosingCommand+5&         LeftWindowCommands 5)      +	 
 %MahApps.Metro.Controls.WindowCommands	  5*   
   y�6"   �� *	list_menu+ �� 	Converter -
   'ICMS_Server.BooleanToVisiblityConverter
  .@�5+      �� -   5         ��Name$ btnInfo��5+       � Cursor$
 Hand��5-      $��14(�6#    �� 	IsEnabled .  .��5/      ��  �� Orientation$ 
Horizontal=�6   .+�50      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.1.1345, Culture=neutral, PublicKeyToken=null]]   Kind$
 Menu=�6.   $��20��6:   $��20��6F   51       � ContextMenu 53      �� 54      $��13(�6"   @�55      m� 56      $��0 0 0 0q�57   '   ��56   '   2  ,WPFLocalizeExtension.Extensions.LocExtension  
   Key$
 main�� m� Command 58   '   �� *
btn_main+ m� Icon 59   "     5:   &   $
 Home=�6>   5<      �� 5=      $��	0 -4 0 -4q�6(   m� 5?      $��0 0 0 0q�5@   '   ��5?   '     $ option�� 5A   '   �� *
btn_option+ 5B   "     5C   &   $ SettingsOutline=�6>   5E      �� 5F      $��	0 -4 0 -4q�6(   m� 5H      $��0 0 0 0q�5I   '   ��5H   '     $ logout�� 5J   '   �� *
btn_logout+ 5K   "     5L   &   $ LogoutVariant=�6>   5N      5P      5R      5S         WindowButtonCommands 5V      1 
 +MahApps.Metro.Controls.WindowButtonCommands  5W   
    3 /MahApps.Metro.Styles.WindowButtonCommands.Win10#���  6(   .��5Z      � .+�5[   
   �� $��0q�6    J� SnapsToDevicePixels . ��5\      �� *
CurrPage+ '   !ICMS_Server.AppPageValueConverter  5_      )  #MaterialDesignThemes.Wpf.DialogHost  5a   
      
Identifier$
 Main��6$      IsOpen 5b   $   �� *DialogHostMain+5c        5f   
   $ InMain��6$    5g   $   �� *DialogHostInMain+5h        5j   
   $	 Msg��6$    5k   $   �� *DialogHostMsg+5l      5n      5p      