   M S B A M L   `   `   `  ���� F  BHeavyClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:HeavyClient.Data.ViewModelsHeavyClient.Data.ViewModels  d `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf %    HeavyClient.Data.ViewModels.Map   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ;local)clr-namespace:HeavyClient.Data.ViewModels     `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     K
liveCharts4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     -     N� Title$	  Map��.��� .+��� .@��� $	��Map��.��'  !Microsoft.Maps.MapControl.WPF.Map  -     Name$ MyMap��+  %Microsoft.Maps.MapControl.WPF.MapCore   CredentialsProviderT  NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$r lCtZIdPSZBTCy9fZTEl71~wC65EwYLEkzMYSpTYg32nw~AoMVtPXE6rJFSrC0ybreB_BqDc1Im6VL36CaJIJUdtkh969Ca_apOmYTZtrO7jj- �� -    ��Name$ detailsItem��$��Details��.����  �� Orientation$ Vertical=�$
��Auto��.+��� $ 
Horizontal=�$��Stretch=�
�������$	��200��.+��� $ Vertical=�.+��� $ 
Horizontal=�.+��� $��
Departure:��$��20(��� -    ��Name$ DepartAdress��$��Adress��$��20(��� $ 
Horizontal=�.+��� $��Arrival:��$��20(��� -   $ ArriveAdress��$��Adress��$��20(��� $ 
Horizontal=�.+��� $��Total Distance:��$��20(��� -   $ Distance��$��1000.2��$��20(��� $ 
Horizontal=�.+��� $��Total Duration:��$��20(��� -   $ Duration��$��1000.2��$��20(��� $	��198��$	��409��$	��400��$��90,0,0,0q�$ Vertical=�.+��� -    ��Name$ 
directions��$	��198��$	��409��$
��Auto���� $ 
Horizontal=�$	��222��
�������.+��� $ Vertical=�.+��� $��Most visited Stations��$��20(��� $ 
Horizontal=�.+��� $��
Departure:��$��20(��� -	   $ mostVDeparture��$��DepartureStation��$��20(��� $ 
Horizontal=�.+��� $��Arrival:��$��20(��� -
   $ mostVArrival��$��ArrivalStation��$��20(��� $��	120,0,0,0q�$��Right=�
�������$	��412��.+�#  LiveCharts.Wpf.CartesianChart  -     Name$ chart��$	��178��