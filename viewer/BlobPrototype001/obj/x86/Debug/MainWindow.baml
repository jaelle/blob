   M S B A M L   `   `   `  ����K  GBlobPrototype001, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BlobPrototype001BlobPrototype001  5   	   !    BlobPrototype001.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -myclr-namespace:BlobPrototype001     5   	   -   5        $��
mainWindow��5   (     � Title$  Blob Multitouch Viewer��6	    � WindowState$ 	Maximized=�6U   .��5      � -   5         �Name$ 
windowGrid��5      .+�5   
   o� -   5         o�Name$ mainMenu��5   3   $��50��6   $	��Top=�6   $��Stretch=�6E   
�������
���   �.@�5      m� -   5        $��Open Project...��5      $��Center=�6O   $��48��6j   $��0q�6v   $
��7,15q�6�   $
��Left=�6�   
�������$��2q�6�   
���   �
�������6  m� 5	      -   5        $��	mnuRecord��5	      $��Record a Session...��6(   $��Center=�6[   $��48��6v   $��0q�6�   $
��7,15q�6�   $
��Left=�6�   
�������$��2q�6�   
�������6�   m� 5
      -   5        $��mnuPlayback��5
      $��Playback a Session...��6*   $��Center=�6a   $��48��6|   $��0q�6�   $
��7,15q�6�   $
��Left=�6�   
�������$��2q�6�   
�������6  m� 5      -   5        $��mnuInk��5      $��	Start Ink��6%   $��Center=�6I   $��48��6d   $��0q�6p   $
��7,15q�6{   $
��Left=�6�   
�������$��2q�6�   
�������6�   m� 5      -   5        $��mnuErase��5      $��Eraser��6'   $��Center=�6H   $��48��6c   $��0q�6o   $
��7,15q�6z   $
��Left=�6�   
�������$��2q�6�   
�������6�   m� 5      -	   5        $��mnuClear��5      $��	Clear Ink��6'   $��Center=�6K   $��48��6f   $��0q�6r   $
��7,15q�6}   $
��Left=�6�   
�������$��2q�6�   
�������6�   m� 5      -
   5        $��mnuSave��5      $��Save Ink��6&   $��Center=�6H   $��48��6c   $��0q�6o   $
��7,15q�6z   $
��Left=�6�   
�������$��2q�6�   
�������6�   5      o� 5   
   -   5        $ iconMenu��5   3   $��50��6   $	��Top=�6   $��Right=�6E   
�������6z   o� 5   
   -   5        $��manipulateMenu��5   K   $��30��6   $��Stretch=�6   $��0,50,0,0q�69   $	��Top=�6a   
�������
���   �.@�5      m� -   5        $��Increase Friction��5      $��Center=�6K   $��28��6f   $��0q�6r   $	��7,7q�6}   $
��Left=�6�   
�������$��2q�6�   
�������6�   m� 5      -   5        $��Decrease Friction��5      $��Center=�6K   $��28��6f   $��0q�6r   $	��7,7q�6}   $
��Left=�6�   
�������$��2q�6�   
�������6�   5      (� 5   
   -   5        $��viewBox��5      $��0,80,0,0q�6!    (� Stretch$ Uniform=�63    (� StretchDirection$
 Both=�6E   .��5      �� -   5         ��Name$ scene��5       J� IsManipulationEnabled .   �� EditingMode$
 None=�6E   $
��1024��6X   $	��768��6e   .H�5      �� -   5        $��initTranslateDeceleration��5      $��10��6=   $y�Hidden=�6G   6]   �� 5      -   5        $��initExpandDeceleration��5      $	��0.1��6:   $y�Hidden=�6E   6[   �� 5      -   5        $��initRotateDeceleration��5      $��2��6:   $y�Hidden=�6C   6Y   5      5      5      5      