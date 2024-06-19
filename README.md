## Windows Build and Run
1. Visual Studio Community 2022 for Mac
2. Open <project-path>/PDF_OCR_Parser/PDF_OCR_Parser.sln

## Macos Build and Run

# Satisfy Dependencies

1. Tesseract OCR, https://github.com/tesseract-ocr/tesseract
```
  $ brew install tesseract
  $ cp /usr/local/Cellar/tesseract/5.4.1/lib/libtesseract.5.dylib <project-path>/PDF_OCR_Parser/bin/Debug/net6.0/x64/libtesseract50.dylib
```

2. http://www.leptonica.org/source/leptonica-1.82.0.tar.gz
```
  $ tar zxvf ~/Downloads/leptonica-1.82.0.tar.gz
  $ cd leptonica-1.82.0
  $ ./configure
  $ make ; make install
  $ cp  /usr/local/lib/liblept.dylib <project-path>/PDF_OCR_Parser/bin/Debug/net6.0/x64/libleptonica-1.82.0.dylib
```
3. Visual Studio Community 2022 for Mac
4. Open <project-path>/PDF_OCR_Parser/PDF_OCR_Parser.sln
5. Hit '>' (play)
6. Visit <project-path>test-pdfs, 
```
  $ sh pdf-upload.sh <filename.pdf>
```
