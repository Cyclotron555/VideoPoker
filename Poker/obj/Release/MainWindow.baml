   M S B A M L   `   `   `  ���� 	  Poker    Poker.mainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     @d2http://schemas.microsoft.com/expression/blend/2008     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     �Name$  MainWin�� � Title$ Red Black Poker�� � 
ResizeMode$ CanResizeWithGrip=�$��Segoe UI Black)� � WindowStartupLocation$ CenterScreen=�$	��960��$	��520��$
��Bold"� � WindowStyle$
 None=� � AllowsTransparency . ��
� .���  � Color$ 	#FF084983�� � Offset$ 1Q�� $ 	#FF2086C1��.��(� -    (�Name$ MainViewBox��	 (� Stretch$	 Uniform=�.��� -   
 �Name$
 mainGrid��$��0,0,0,0q� � UseLayoutRounding . $
��1010��$	��520��$��Disabled=�$��Disabled=� � ShowGridLines .   � IsSharedSizeScope . ����  j� Stretch$ Uniform=� �� ImageSource$ Resources/BLogo3.png�� �� Opacity$	 0.5Q�.+��� -    ��Name$ MainStackPanel��$��0,0,1,0q�.+��� -   $ TopStackPanel�� �� Orientation$ 
Horizontal=�$	��180��$��10,0,0,0q�.+��� $��
20,-20,0,0q�$	��202��$	��160��.+��� -    ��Name$
 logo��$��Resources/LogoR.png��$��-25,-56,0,0q�$	��272���� -   $ BonusStackPanel��$ Vertical=�$��39��$��32,2,0,2q�.+��� -    ��Name$ BonusBorder9��$��30��$��15��$��0,14,0,0q�$��1q����	#FF090041 �� CornerRadius$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$	��★�� �� TextAlignment$ Center=�$��Segoe UI Semibold)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -	   $ BonusBorder8��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$	��★��$ Center=�$��Segoe UI Semibold)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -
   $ BonusBorder7��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$��B��$ Center=�$��Segoe UI Black)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -   $ BonusBorder6��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$��O��$ Center=�$��Segoe UI Black)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -   $ BonusBorder5��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$��N��$ Center=�$��Segoe UI Black)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -   $ BonusBorder4��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$��U��$ Center=�$��Segoe UI Black)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -   $ BonusBorder3��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$��S��$ Center=�$��Segoe UI Black)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -   $ BonusBorder2��$��30��$��15��$��0,3,0,0q�$��1q����	#FF090044$ 2�����	#FFDC0000._��� ���	#FFA8FFFF$	��★��$ Center=�$��Segoe UI Semibold)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� �� -   $ BonusBorder1��$��30��$��15��$��0,3,0,0q�$��1q����	#FFDC0000$ 2��._��� ���	#FFA8FFFF$	��★��$ Center=�$��Segoe UI Semibold)�$��28��$��Normal"�$��0q�$��Center=�$��14(�$��21����Z� ����  �� Color$ 	#FF090044���� -   $ WinningTableStackPanel��$��	402,0,0,0q��� -   $ MainBonusBorder��
������ $��0,10,0,6q� �� Padding$ 0q��� -   $ ScoreStackPanel��$ Vertical=�$��0q�.+��� -    ��Name$ LblScoreString��$	��250��$��50��$��
10,10,10,0q�$
��CASH��$��Bottom=�$��Center=����	#FFCED7F0$��24(�$
��Bold"��� -   $ LblScore��$	��250��$��80��$��	10,0,10,0q�$�� ��$	��Top=�$��Center=����	#FFCED7F0$��56(��� -   $ WindowCtrlStackPanel��$ Vertical=�$��20��.+��� -    ��Name$ MoveLbl��$��0q�$��20��$��22��$	��❖��$��Normal"� � Cursor$
 Hand��$��Microsoft Sans Serif)����	#FFBDF6FF$��22(�{�.   �� Padding$ 1,0,0,0q�� -    �Name$ moveInfo��$��
MousePoint=� � HorizontalOffset$	 -25�� � VerticalOffset$	 -10��  � AllowsTransparency  . $��Scroll=�.���� $��1q�
�������! J� Opacity$	! 0.5Q�$ 4������    BorderColor# �   ._��� $��GRAB AND MOVE WITH MOUSE��$ 5q�$��NoWrap=�  BorderBrush#���  $
��Bold"�$��Segoe UI Black)�$��10(��� -   $ CloseLbl��
������ $��0,2,0,0q�$��20��$��22��$	��Ⓧ�����	#FFBDF6FF$��22(�$
��Bold"�$
 Hand��$��Microsoft Sans Serif)�$ 1,0,0,0q�� -   $ 
closePopUp��$��
MousePoint=�$	 -25��$	 -10��  . $��Scroll=�.���� $��1q�
�������$	! 0.5Q�$ 4������ # �   ._��� $��
CLOSE GAME��$ 5q�$��NoWrap=�#���  $
��Bold"�$��Segoe UI Black)�$��10(��� -   $ 	ResizeLbl��
������ $��0,2,0,0q�$��20��$��22��$	��⍠�����	#FFBDF7FF$��Normal"�$��24(�$��Microsoft Sans Serif)�$
 Hand��$ 1,0,0,0q�� -   $ resizePopUp��$��
MousePoint=�$	 -25��$	 -10��  . $��Scroll=�.���� $��1q�
�������$	! 0.5Q�$ 4������ # �   ._��� $��TOGGLE FULLSCREEN��$ 5q�$��NoWrap=�#���  $
��Bold"�$��Segoe UI Black)�$��10(��� -   $ 	MinMaxLbl��
������ $��0,6,0,0q�$��20��$��22��$	��⍢�����	#FFBDF7FF$
 Hand��$��Microsoft Sans Serif)�$��22(�$��Normal"�$ 1,0,0,0q�� -   $ minMaxPopUp��$��
MousePoint=�$	 -25��$	 -10��  . $��Scroll=�.���� $��1q�
�������$	! 0.5Q�$ 4������ # �   ._��� $��MINIMIZE��$ 5q�$��NoWrap=�#���  $
��Bold"�$��Segoe UI Black)�$��10(��� -   $ SettingsLbl��$��0,3,0,0q�$��20��$��22��$	��⚙�����	#FFBDF7FF$
 Hand��$��16(�$ 0,2,0,0q�$��Microsoft Sans Serif)�$��Normal"�� -    $ settingsPopUp��$��
MousePoint=�$	 -25��$	 -10��  . $��Scroll=�.���� $��1q�
�������$	! 0.5Q�$ 4������ # �   ._��� $��SETTINGS | STATISTICS��$ 5q�$��NoWrap=�#���  $
��Bold"�$��Segoe UI Black)�$��10(��� -!   $ CardsStockPanel��$��0,5,0,0q�$ 
Horizontal=�$	��960��$	��220��$��Center=�$��Center=�.+��� $	��184��$��20,0,0,0q�.+��� -"   $ card_1��$!�� Resources/backdesign_7.png�� . $
 Hand��$��Uniform=�$	��220���� $	��184��.+��� -#   $ card_2��$!�� Resources/backdesign_7.png�� . $��Uniform=�$
 Hand��$	��220���� $	��184��.+��� -$   $ card_3��$!�� Resources/backdesign_7.png�� . $��Uniform=�$
 Hand��$	��220���� $	��184��.+��� -%   $ card_4��$!�� Resources/backdesign_7.png�� . $��Uniform=�$
 Hand��$	��220���� $	��184��.+��� -&   $ card_5��$!�� Resources/backdesign_7.png�� . {�.  $
 Hand��$	��220���� -'   $ HoldSignsStackPanel��$ 
Horizontal=�$	��960��$��18��$��Center=�$��Center=�$��0,5,0,0q�.+��� -(   $ h1Parent��$	��184��$��20,0,0,0q�.+��� -)   $ 	hold_sign��" K� ZIndex$" 4��$��18���� -*   $ h2Parent��$	��184��.+��� -+   $ 
hold_sign2��$" 4�� . $��18���� -,   $ h3Parent��$	��184��.+��� --   $ 
hold_sign3��$" 4�� . $��18���� -.   $ h4Parent��$	��184��.+��� -/   $ 
hold_sign4��$" 4�� . $��18���� -0   $ h5Parent��$	��184��.+��� -1   $ 
hold_sign5��$" 4�� . $��18���� -2   $ ShuffleTextBoxStackPanel��$ 
Horizontal=�$	��960��$��30��$��Center=�$��Center=�.+��� $	��400��$��280,0, 0, 0q�.+��� -3   $ shufflingTextBox��$��20(����	#FFCED7F0$
��Bold"�$ Center=��� -4   $ BottomStackPanel��$ 
Horizontal=�$
��1010��$��60��$��0,0,0,0q�$��Center=�$��Center=�.+��� -5   $ BottomLeftSP��$ 
Horizontal=�$��0,7,0,0q�.+��� $��58,0,0,0q�.+��� -6   # ��Name$# BetMinus��$
��BET-��$��50��$��44��$��0q��� .+��� -7   $ 	BetBorder�����	#FF000444$ 4��$ 0q�$��1q�
���   �$��50��$��44��$��50��._��� -8   $ BetLabel��$��22(�$��Center=�$��Center=�$��1�����	#FF089AFF���	#FFCED7F0$��0q��� .+��� -9   $# BetPlus��$
��BET+��$��0q�$��50��$��44���� -:   $ 
BallanceSP��$ Vertical=�$	��2,0q�.+��� -;   $ 
lblAccount��$��90��$��25��$��WALLET�����	#FFCED7F0$��Center=�$��Center=�$��Center=�$��0,-4,0,0q�$��Segoe UI Black)�$
��Bold"�$��Center=��� -<   $ lblAccountAmount��$��90��$��18��$�� �����	#FFCED7F0$��Center=�$��Center=�$��0q�$��Center=�$��Center=�$��16(�$��Disabled=�$��0q���Z� �� -=   $ AccountButtonsSP��$ 
Horizontal=�.+��� .+��� ->   $# WithrawButton��$��TRANSFER
 TO CASH��$��44��$��80��$��0q�$��0q��� .+��� -?   $# CashOutButton��$��	CASH
 OUT��$��44��$��80��$��2,0,0,0q�$��0q��� .+��� -@   $# DepositButton��$��   ADD
MONEY��$��44��$��80��$��2,0,0,0q�$��0q��� -A   $ DoubleUpAndCollectSP��$ 
Horizontal=�$��42,0,0,0q�.+��� .+��� -B   $# DoubleUpButton��$��	DOUBLE UP��$��53��$	��100��$��2,0,0,0q�$��1q�$��14(��� .+��� -C   $# 	ColButton��$��COLLECT��$��53��$	��100��$��2,0,0,0q�$��1q�$��14(��� -D   $ DrawButtonSP��$��30,0,0,0q�.+��� -E   $
# Draw��$
��DRAW��$��53��$	��100��$��0q�$��1q�$��22(� � -F   $  �Name$$ 	MainFrame��$��-32,0,-26,0q�$��Disabled=�%  � NavigationUIVisibility$% Hidden=�$��3��{�.  ��Z� 