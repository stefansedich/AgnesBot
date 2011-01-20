# AgnesBot

AgnesBot is a pluggable and modular IRC bot written in C# using .NET 4.0, Castle and RavenDB.

##Features 

Various of modules are provided out of the box:

 - Comments Module
 - TinyUrl Module
 - Url Aggregator Module

RavenDB is used as a data store and 3rd party modules are able to hook directly into it for storage.

##Future

Future plans are to add modules for Teamcity/CC.NET and anything else I find useful.

###Why does git show that all of my files are modified?

AgnesBot is built by Windows users, so all of the text files have CRLF line 
endings. These line endings are stored as-is in git (which means we all have 
autocrlf turned off).
If you have autocrlf enabled, when you retrieve files from git, it will modify
all of your files. Your best bet is to turn off autocrlf, and re-create your
clone of AgnesBot.

1. Delete your local clone of the AgnesBot repository
1. Type: `git config --global core.autocrlf false`
1. Type: `git config --system core.autocrlf false`
1. Clone the AgnesBot repository again