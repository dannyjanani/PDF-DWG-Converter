# PDF-DWG-Converter
A project to batch convert PDFs to DWGs and vice versa.

## Project Overview
At Con Edison, there were many times where they received drawings from contractors, that need to be watermarked with a confidential classification. The classification was usually placed by means of a watermark. To manually watermark 1000s of drawings, would in itself be a full time job. The project was to create a program to do this using VBA and AutoCAD API. I created a tool where the user would specify the directory of files they would like to process. The program will copy the directory to the server, add a new layer with the watermark, save the file as its original name, send an email to the user saying the Files have been processed with a link to the files. After the drawings have been processed, the files will be deleted after 7 days as set by the admin to avoid overloading the server. The challenge was that it had to be a tool that anyone can access, without having to download on their machine due to the limitations set by Con Edison, where only an admin can download any executables. The solution was to place this program and processed in the background on a server, having a File Listener. The File Listener would listen for any directories added in a certain location, and it will begin the process notifying the user when it is complete. 

## Project Goals

* Fully designed and implemented by me.
* Weekly meetings with manager showing progress for the week.
* Obtaining admin approval to create the executable.
* Obtaining a server to run the program on.
* Obtaining a no-reply email to be used to let user track progress.
* Add a watermark to many DWG files.
* Making it as convenient and as quick as possible for the user.
* Client Side:
  * User will specify the directory they would like to process.
  * The directory will be copied to the server for processing.
  * No additional downloads for the user.
  * The user will receive an email with the location to the processed files.
* Server Side: 
  * There is a File Listener, watching for a new directory.
  * When it sees a new directory, it will open a process of AutoCAD with an Embedded Drawing.
  * The drawings will be processed and stored on the server.
  * An email will be sent to the user with the location.
  * The files will be deleted after 7 days as specified by the admin.

## Skills Used
  
* Software:
  * Visual Studio
  * AutoCAD
  * AutoCAD API

* Languages:
  * VBA
  * C#
