   M S B A M L   `   `   `  ���� @  <Poker, Version=1.0.0.3, Culture=neutral, PublicKeyToken=nullclr-namespace:PokerPoker      Poker.RedOrBlack   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     %localclr-namespace:Poker     -     N�Name$  RedOrBlack1�� N� ShowsNavigationUI .   N� 
FontFamily$ Segoe UI Black)� N� Title$ 
RedOrBlack��$
��1066��$	��520�� N� 
Background 
� .���  � Color$ 	#FF2084BF�� � Offset$ 0Q�� $ 	#FF084983��$ 1Q�.���� -    ��Name$ RedOrBlackMain��.+��� -   $	 Top�� �� Orientation$ 
Horizontal=�$	��200��$	��Top=�$��Right=�$
��1022��$��0q�.+��� $ 
Horizontal=�.+��� -   	 ��Name$	 GuessingCard��$ ��Resources/backdesign_4.png��
 � UseLayoutRounding
 . $��Uniform=� � Cursor$
 Hand��$	��160��$��21,20,-129,20q� K� ZIndex$ 6���� $ 
Horizontal=�.+��� -   $	 card1��$!�� Resources/backdesign_4.png��
 . $��Uniform=�$	��160��$��216,0,-57,0q�$ 1���� -   $	 card2��$!�� Resources/backdesign_4.png��
 . $��Uniform=�$	��160��$��	0,0,-57,0q�$ 2���� -   $	 card3��$!�� Resources/backdesign_4.png��
 . $��Uniform=�$	��160��$��	0,0,-57,0q�$ 3���� -   $	 card4��$!�� Resources/backdesign_4.png��
 . $��Uniform=�$	��160��$��	0,0,-57,0q�$ 4���� -	   $	 card5��$!�� Resources/backdesign_4.png��
 . $��Uniform=�$	��160��$��0,20,-57,20q�$ 5���� -
   $	 card6��$!�� Resources/backdesign_4.png��
 . $��Uniform=�$	��160��$��	0,20,4,20q�$ 6���� $	��160��$��54��.+��� -    ��Name$ B6��$��22��$��50��$��2,4,2,2q�
���  �� �� CornerRadius$ 3�����	#FFF3F4F5$��3q�._��� -    ��Name$	 TB6��$	��64X�����	#FFF3F4F5$��Segoe UI Black)�$��18(�$��0q�$��Center=�$��Center=��� -   $ B5��$��22��$��50��$��2q�
���  ��$ 3�����	#FFF3F4F5$��3q�._��� -   $	 TB5��$	��32X��$��Segoe UI Black)�$��16(�$��0q�$��Center=�$��Center=����	#FFF3F4F5�� -   $ B4��$��22��$��50��$��2q�
���  ��$ 3�����	#FFF3F4F5$��3q�._��� -   $	 TB4��$	��16X��$��Segoe UI Black)�$��16(�$��0q�$��Center=�$��Center=����	#FFF3F4F5�� -   $ B3��$��22��$��50��$��2q�
���  ��$ 3�����	#FFF3F4F5$��3q�._��� -   $	 TB3��$��8X��$��Segoe UI Black)�$��16(�$��0q�$��Center=�$��Center=����	#FFF3F4F5�� -   $ B2��$��22��$��50��$��2q�
���  ��$ 3�����	#FFF3F4F5$��3q�._��� -   $	 TB2��$��4X��$��Segoe UI Black)�$��16(�$��0q�$��Center=�$��Center=����	#FFF3F4F5�� -   $ B1��$��22��$��50��$��2,2,2,0q�
���  ��$ 3�����	#FFF3F4F5$��3q�._��� -   $	 TB1��$��2X��$��Segoe UI Black)�$��16(�$��0q�$��Center=�$��Center=����	#FFF3F4F5�� $��5,0,0,0q�.+��� -    ��Name$ LblWin��$	��WIN��$	��209��$��0q�$
��Bold"�$��72(�$��Center=�$��Center=����	#FFFFF2F2$��Disabled=�$��Center=�$��Center=�$��0q��� -   $ LblWinCount��$��000000��$��0q����	#FFFFF2F2$��Center=�$��Center=�$��Disabled=�$��72(�$��Center=�$��Center=�$��0q�
 .  �� -   $ Middle��$ 
Horizontal=�$
��1021��$	��188��$��0q�.+��� -   $	 originalCard1��$ ��Resources/backdesign_4.png��$��43,0,0,0q�$	��148���� -   $ tcard1��$	��WIN�� J� RenderTransformOrigin$ -2.333,0.42&�$��-133,65,-27,65q�$��58��$ 2��$	��106��$��42(�
���  �� �� Padding$ 0q�$��Center=�$��Center=� �� TextAlignment$ Center=�$��Disabled=�&   System.Windows.Media.TextOptions   TextHintingMode$ Fixed=����	#CD015397�� -   $	 originalCard2��$ ��Resources/backdesign_4.png��$	��148��$��20,0,0,0q��� -   $ tcard2��$	��WIN��$ -2.333,0.42&�$��-133,65,-27,65q�$��58��$ 2��$	��106��$��42(�
���  ��$ 0q�$��Center=�$��Center=�$ Center=�$��Disabled=�$ Fixed=����	#CD015397�� -   $	 originalCard3��$ ��Resources/backdesign_4.png��$	��148��$��20,0,0,0q��� -   $ tcard3��$	��WIN��$ -2.333,0.42&�$��-133,65,-27,65q�$��58��$ 2��$	��106��$��42(�
���  ��$ 0q�$��Center=�$��Center=�$ Center=�$��Disabled=�$ Fixed=����	#CD015397�� -    $	 originalCard4��$ ��Resources/backdesign_4.png��$	��148��$��20,0,0,0q��� -!   $ tcard4��$	��WIN��$ -2.333,0.42&�$��-133,65,-27,65q�$��58��$ 2��$	��106��$��42(�
���  ��$ 0q�$��Center=�$��Center=�$ Center=�$��Disabled=�$ Fixed=����	#CD015397�� -"   $	 originalCard5��$ ��Resources/backdesign_4.png��$	��148��$��20,0,0,0q��� -#   $ tcard5��$	��WIN��$ -2.333,0.42&�$��-133,65,-27,65q�$��58��$ 2��$	��106��$��42(�
���  ��$ 0q�$��Center=�$��Center=�$ Center=�$��Disabled=�$ Fixed=����	#CD015397�� -$   $ LblHand��$
��HAND��$��48(����	#FFFFF2F2$��Center=�$��Bottom=�$	��130��$��Center=�$��Center=�$��20,0,0,0q�$	��310��$��5q� � IsSharedSizeScope . �� -%   $ Bottom��$ 
Horizontal=�$
��1064��$	��132��$��0q�$��Center=�$��Center=�.+��� -&   $ logoSP��$��0q�.+��� -'   $	 	logoImage��$��Resources/LogoR.png��$	��234��$��	0,-55,0,0q��� -(   $ RedBStackPanel��$ 
Horizontal=�$��0q�.+��� -)    ��Name$ 	RedButton��$	��RED��
�������$��48(�$��Center=�$��Center=�$	��188��$	��100���� -*   $ BlackBStackPanel��$ 
Horizontal=�$��14,0,0,0q�.+��� -+   $ BlackButton��$��BLACK��
���   �$��48(�$��Center=�$��Center=�$	��188��$	��100���� -,   $ CollectBStackPanel��$ 
Horizontal=�$��	100,0,0,0q�.+��� --   $ CollectButton��$��COLLECT��
�������$��48(�$��Center=�$��Center=�$	��240��$	��100��