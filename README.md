# EBCDIC.Net
This is port of EBCDIC project from http://www.yoda.arachsys.com/csharp/ebcdic/ to public Git repository

EBCDIC Encoding Library
-----------------------

Please see http://www.pobox.com/~skeet/csharp/ebcdic/ for more
general information.

Building from the command line
------------------------------

1) The character map reader (only used if you wish to add
   more encodings)
   
csc CharMapReader\CharMapReader.cs

(Produces CharMapReader.exe. Run in a directory containing EBCDIC-*
files to produce ebcdic.dat, which should then be copied into the
EbcdicEncoding directory.)

2) The encoding library

csc /target:library /out:EbcdicEncoding.dll /res:EbcdicEncoding\ebcdic.dat,EbcdicEncoding.ebcdic.dat EbcdicEncoding\*.cs

3) The test program

csc /r:EbcdicEncoding.dll Test\Test.cs
