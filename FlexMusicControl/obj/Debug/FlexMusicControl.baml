   M S B A M L   `   `   `  ����K  GFlexMusicControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:FlexMusicControlFlexMusicControl  5      '    !FlexMusicControl.FlexMusicControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:FlexMusicControl     5      $	��250��5   #     � 	Resources  5	       ��Name   Logo_ShrinkBPM �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value   EasingFunction.  (System.Windows.Media.Animation.PowerEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode��%x  &	        �� 5
   
   $ Logo_ShrinkBPM��6,   .��5      U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6,   $
 Logo��6�   .\�5        $ 0:0:0.05��6'   $
 1.05Q�6:     5      $ 0:0:0.2��6'   $ 1Q�69    5        5      $ EaseOut=�6$   5      5      U� 5      $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6,   $
 Logo��6�   .\�5        $ 0:0:0.05��6'   $
 1.05Q�6:     5      $ 0:0:0.2��6'   $ 1Q�69    5        5      $ EaseOut=�6$   5      5      5      5      .��5      � -   5        $��MusicControlRootContainer��5      $
��Left=�6,   $	��250��6G   .+�5   
   � -   5        $��SpectrumRootContainer��5      $
��Left=�6,   $	��250��6G   5      �� 5    
   -   5        $��SpectrumImageBitmap��5       $	��500��6+   $	��500��67   $��-125, -125, -125, -125q�6D   �� 5!   
   -   5         ��Name$
 Logo��5!      $	��250��6"   $��Center=�6/   $��Center=�6J   $	��250��6g   	 J� RenderTransformOrigin$	 0.5,0.5&�6s   ��5"      �� 5#      
 �� ImageSource$
 logo.png��6   z�5%      V� 5&      .~�5'      �� �� 5(      T� 5)      5*      5,      5-      5.      