   M S B A M L   `   `   `  ����d  `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5       HeavyClient%   HeavyClient.Data.ViewModels.Map   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     5      K
liveCharts4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5        N� Title$	  Map��5
      .��5      � .+�5   
   �� .@�5      �� $	��Map��6   .��5      '   !Microsoft.Maps.MapControl.WPF.Map  -   5          Name$ MyMap��5      +   %Microsoft.Maps.MapControl.WPF.MapCore   CredentialsProviderT   NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$r lCtZIdPSZBTCy9fZTEl71~wC65EwYLEkzMYSpTYg32nw~AoMVtPXE6rJFSrC0ybreB_BqDc1Im6VL36CaJIJUdtkh969Ca_apOmYTZtrO7jj- 5      5      5      �� 5      -   5         ��Name$ detailsItem��5   '   $��Details��6   .��5      �  � ShowGridLines$
 True��6   $
��Auto��6,   $
��Auto��69    � ColumnDefinitions 5      �� 5      $��* �6+   �� 5      $��* �6+    � RowDefinitions 5      �� 5      $��* �6(   �� 5      $��* �6(   .+�5      �� 
�������$��0��64   $��0��6A    �� Orientation$ Vertical=�6Q   .+�5       �� $ 
Horizontal=�6%   .+�5!      �� $��
Departure:��6$   $��20(�69   �� 5"      -   5         ��Name$ DepartAdress��5"   C   $��Adress��6$   $��20(�65   6C   5#      �� 5$      -   5        	 ��Name$	 depStackPane��5$   %   $ 
Horizontal=�6;   .+�5%      �� $��Departure Station:��6$   $��20(�6A   �� 5&      -   5        $ DepartureStation��5&   A   $
��Name��6$   $��20(�63   6A   5'      �� 5(      $ 
Horizontal=�6%   .+�5)      �� $��Arrival:��6$   $��20(�67   �� 5*      -   5        $ ArriveAdress��5*   C   $��Adress��6$   $��20(�65   6C   5+      �� 5,      -   5        $	 arrStackPane��5,   %   $ 
Horizontal=�6;   .+�5-      �� $��Arrival Station:��6$   $��20(�6?   �� 5.      -   5        $ ArrivalStation��5.   A   $
��Name��6$   $��20(�63   6A   5/      �� 50      $ 
Horizontal=�6%   .+�51      �� $��Total Distance:��6$   $��20(�6>   �� 52      -	   5        $ Distance��52   C   $��1000.2��6$   $��20(�65   6C   53      �� 54      $ 
Horizontal=�6%   .+�55      �� $��Total Duration:��6$   $��20(�6>   �� 56      -
   5        $ Duration��56   C   $��1000.2��6$   $��20(�65   6C   57      58      �� 59      -   5        
 ��Name$
 
directions��59   N   
�������$��0��61   $��2��6>   6N   �� 5;      
�������$��1��6C   $��0��6P   $ Vertical=�6`   .+�5<      �� $
��Bold"�6    $��Most visited Stations��62   $��30(�6R   �� 5=      $ 
Horizontal=�6%   .+�5>      �� $��
Departure:��6$   $��20(�69   �� 5?      -   5        $ mostVDeparture��5?   ?   $��DepartureStation��6$   $��20(�6W   5@      �� 5A      $ 
Horizontal=�6%   .+�5B      �� $��Arrival:��6$   $��20(�67   �� 5C      -   5        $ mostVArrival��5C   =   $��ArrivalStation��6$   $��20(�6S   5D      5G      #  LiveCharts.Wpf.CartesianChart  5H      -   5          Name$ chart��5H   d   
�������$��1��6G   $��1��6T   6d   5J      5K      5L      5N      5P      