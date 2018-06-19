Place a link (*.lnk file) here for any service to be launched
automaticly on startup.

The Link should follow this name format:

     <Index>.<Group> - <Name>

     Examples:
     
     0.0 - TeamSpeak 3 Server
     1.0 - TeamSpeak 3 Client


<Index>          Incremental Index:
                 Used to sort the services in the
                 correct start order.

<Group>          Virtual Desktop Group:
                 A number indecating the virtual desktop
                 to place all windows of the process.
                 The same number on two services means, 
                 the will be placed on the same virtual desktop
                 "0" is the first virtual desktop.
                 "1" is the second and so on.
                 Skipping a nummber will create a empty
                 virtual desktop in between.

<Name>           Service Name:
                 A frindly name for this service.

To disable a service, add a exclamation mark ("!") in front
of the name.

Example: !2.2 - Test Server



------------------------------
 2018 (c) icelane.net
