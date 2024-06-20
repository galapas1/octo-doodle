![Alt text](./imgs/octo-doodle.jpg?raw=true&width=200x "Octo Doodle Project")

# Overview
Octo Doodle is a simple webservice exposing a single endpoint that accepts a pdf file
```
POST /api/parsePDF
```

When invoked, the PDF is rendered from bytecode to text. If the PDF contains embedded images resulting from a document being scanned to create the PDF, Octo Doodle will extract the images from each page and pass each image in turn through an OCR process to obtain the text from the image.

The response to the POST will be a combination of processing traces and resultant text:
```
(info) page 9 has 1 image(s)
(tesseract) page 9, mean confidence: 0.82
(tesseract) recognized text:
Form 1142 Record of Authorization to...
```

Octo Doodle has been tested on Windows11 and MacOS using the steps below.

# Windows Build
1. Visual Studio Community 2022 for Mac
2. Open <project-path>/PDF_OCR_Parser/PDF_OCR_Parser.sln

# Macos Build
## Satisfy Dependencies

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

# Testing
Visit <project-path>test-pdfs, 
```
  $ sh pdf-upload.sh <filename.pdf>
```

